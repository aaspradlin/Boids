using UnityEngine;
using System.Collections;

/** <summary>
 * This class is used to determine how to change the velocity of a boid. </summary> */
public class GAVectorSet : VectorSet {
	
	/** <summary>
	 * The script containing <see cref="GACoefficient"/> information. </summary> */
	private GAGenome genome;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Primary constructor. </summary>
	 * <param name="genome"> The <see cref="GACoefficient"/> information to use </param> */ 
	public GAVectorSet(GAGenome genome) {
		this.genome = genome;
	}
	
	/** <summary>
	 * Finds the offset for the given boid. </summary> 
	 * <param name="boid"> The boid the offset is being calculated for </param>
	 * <param name="boids"> The boids important to the calculation </param> 
	 * <returns> The offset </returns> */
	override public Vector3 Get_vector(GameObject boid, ArrayList boids) {
		
		//grab the group of boids in front of the current boid
		ArrayList leader_boids = Get_leaders(boid, boids);
		
		//set up the force added vector, no normalizing
		//IMPORTANT: this is where to add other vectors for offsetting
		Vector3 directionVector = (Follow (boid, leader_boids) * genome.FollowCoefficient) 
		                                                         + (Cohesion(boid, leader_boids) * genome.CohesionCoefficient) 
		                                                         + (Separation (boid, leader_boids) * genome.SeparationCoefficient);
		return directionVector;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Updates the struct that stores special groups of interest within the flock. </summary>
	 * <param name="boid"> The boid this is being calculated for </param>
	 * <param name="boids"> The boids important to the calculation </param>
	 * <returns> The boids ahead of the focus boid </returns*/
	private ArrayList Get_leaders(GameObject boid, ArrayList boids) {
		
		ArrayList leader_boids = new ArrayList ();
		
		//fill the localBirds arraylist and the leaderBirds arraylist
		foreach (GameObject other_boid in boids) {
			if (other_boid != boid.gameObject) {
				
				//vector pointing from this bird's position to the bird in the arraylist's position
				Vector3 thisSeparation = other_boid.transform.position - boid.transform.position;
				
				//add to the localBirds array if it's within the radius 
				if (thisSeparation.sqrMagnitude <= Boid.RADIUS * Boid.RADIUS) { 
					
					//if the angle between the bird's velocity vector and thisSeparation vector
					//is within the bird's angle of vision, add it to leaderBirds
					if (Vector3.Angle(boid.rigidbody.velocity, thisSeparation) < Boid.VISION_ANGLE) {
						leader_boids.Add (other_boid);
					} 
				}
			}
		}

		return leader_boids;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Finds the offset gotten from the position of the flock. 
	 * This vector is relative to the boid's position. </summary>
	 * <param name="boid"> The boid this is being calculated for </param>
	 * <param name="boids"> The boids important to the calculation </param>
	 * <returns> The offset </returns> */
	private Vector3 Cohesion(GameObject boid, ArrayList boids) {
		
		//vector pointing from this bird's position to the flock's position
		Vector3 flockOffset = Vector3.zero;

		if (boids.Count != 0) {

			//add the offsets from all the birds in the passed arraylist and average them
			foreach (GameObject other_boid in boids) 
				flockOffset += other_boid.transform.position - boid.transform.position;
			
			return flockOffset / boids.Count;
		}

		return flockOffset;
	}
	
	/** <summary> 
	 * Finds the offset gotten from the positions of boids in the passed list. 
	 * This vector is relative to the boid's position. </summary>
	 * <param name="boid"> The boid this is being calculated for </param>
	 * <param name="boids"> The boids important to the calculation </param>
	 * <returns> The offset </returns> */
	private Vector3 Separation(GameObject boid, ArrayList boids) {
		
		Vector3 separationOffset = Vector3.zero;
		
		//if the passed arraylist isn't empty
		if (boids.Count != 0) {
			
			foreach(GameObject other_boid in boids) {
				
				//vector pointing from the bird in the arraylist to this bird
				Vector3 thisOffset = boid.transform.position - other_boid.transform.position;

				//if this bird is too close, add the offset
				if (thisOffset.magnitude < Boid.SEPARATION_DISTANCE) {

					separationOffset += (Boid.SEPARATION_DISTANCE/(thisOffset.magnitude*2))*thisOffset;
				}  
			}
		} 
		
		//return the offset without normalizing or averaging
		return separationOffset;
	}

	/** <summary>
	 * Returns the offset gotten from the velocities of boids in the passed list.
	 * This vector is relative to the boid's velocity. </summary>
	 * <param name="boid"> The boid this is being calculated for </param>
	 * <param name="boids"> The boids important to the calculation </param>
	 * <returns> The offset </returns> */
	private Vector3 Follow(GameObject boid, ArrayList boids) {

		if (boids.Count == 0) {

			//vector pointing from this bird's velocity vector to the flock's 
			return flock.rigidbody.velocity - boid.rigidbody.velocity;

		} else {

			Vector3 followerVector = Vector3.zero;

			//add all the velocities and average them
			foreach (GameObject other_boid in boids) {
				
				followerVector += other_boid.rigidbody.velocity;
			}

			followerVector /= boids.Count;
				
			//vector pointing from this bird's velocity vector to the averaged velocities of passed birds
			return followerVector - boid.rigidbody.velocity;
		}
	}
	
	///--------additional offset vectors----------------------------------------------------------------------------///
	
	/** <summary>
	 * Returns a vector attracting the bird to one point in space. </summary> 
	 * //TODO optimize this method for use*/
	private Vector3 Bound(GameObject boid) {
		
		Vector3 origin = new Vector3(10,1,5);
		Vector3 boundingOffset = new Vector3(0.01f,0.01f,0.01f);
		
		//multiply resulting vector by the magnitude of the vector between the bird and the origin
		boundingOffset = Vector3.Scale (boundingOffset, -(boid.transform.position - origin));
		
		boundingOffset += boid.transform.right*0.01f*(boid.transform.position - origin).magnitude;	
		
		return boundingOffset;
	}
	
	/** <summary>
	 * Returns a vector constraining the bird to a bounding box. </summary>
	 * //TODO this is so messy */
	private Vector3 BoundingBox(GameObject boid) {
		
		float height = 20, length = 40, width = 40;
		float pullbackStrength = 0.04f;
		Vector3 origin = new Vector3(0, 0, 0), directionToTurn, goalDirection, boundingoffset = Vector3.zero;
		
		//the distances between the edges of the bounding box and the bird's position
		float xOffset = boid.transform.position.x - origin.x, yOffset = boid.transform.position.y - origin.y, zOffset = boid.transform.position.z - origin.z;
		
		//if its x is out of bounds
		if (Mathf.Abs(xOffset) > length) {
			
			//if it's to the left of the bounding box
			if (xOffset < 0) {
				
				goalDirection = Vector3.right;
				
				//if it's also moving forwards
				if (boid.rigidbody.velocity.z > 0) {
					directionToTurn = boid.transform.right;
					
				//if it's also moving backwards
				} else {
					directionToTurn = -boid.transform.right;
				}
				
				//if it's to the right of the bounding box
			} else {
				
				goalDirection = -Vector3.right;
				
				//if it's also moving forwards
				if (boid.rigidbody.velocity.z > 0) {
					directionToTurn = -boid.transform.right;
					
				//if it's also moving backwards
				} else {
					directionToTurn = boid.transform.right;
				}
			}
			boundingoffset += directionToTurn * Mathf.Abs (Vector3.Angle (boid.transform.forward, goalDirection)) * pullbackStrength;
		}
		
		//if its y is out of bounds
		if (Mathf.Abs(yOffset) > height) {
			
			//if it's below the box
			if (yOffset < 0) {
				goalDirection = Vector3.up;
				directionToTurn = boid.transform.up;
				
				//if it's above the box
			} else {
				goalDirection = -Vector3.up;
				directionToTurn = -boid.transform.up;
			}
			boundingoffset += directionToTurn * Mathf.Abs (Vector3.Angle (boid.transform.forward, goalDirection)) * pullbackStrength;
		}
		
		//if its z is out of bounds
		if (Mathf.Abs(zOffset) > width) {
			
			//if it's behind the bounding box
			if (zOffset < 0) {
				
				goalDirection = Vector3.forward;
				
				//if it's also moving to the right
				if (boid.rigidbody.velocity.x > 0) {
					directionToTurn = -boid.transform.right;
					
					//if it's also moving to the left
				} else {
					directionToTurn = boid.transform.right;
				}
				
				//if it's in front of the bounding box
			} else {
				
				goalDirection = -Vector3.forward;
				
				//if it's also moving to the right
				if (boid.rigidbody.velocity.x > 0) {
					directionToTurn = boid.transform.right;
					
					//if it's also moving to the left
				} else {
					directionToTurn = -boid.transform.right;
				}
			}
			boundingoffset += directionToTurn * Mathf.Abs (Vector3.Angle (boid.transform.forward, goalDirection)) * pullbackStrength;
		}
		
		return boundingoffset;
	}
	
	
	//	/* */
	//	private Vector3 ConstrainSpeed() { //TODO
	//		
	//		Vector3 speedOffset = Vector3.zero;
	//		float speedDifference;
	//		
	//		//if the bird's velocity is less than the minimum speed
	//		if (rigidbody.velocity.magnitude < myFlock.MinSpeed) {
	//			
	//			speedDifference = myFlock.MinSpeed - rigidbody.velocity.magnitude;
	//			speedOffset += myFlock.Drag * speedDifference;
	//			
	//		} //if the bird's velocity is greater than the maximum speed 
	//		else if (rigidbody.velocity.magnitude > myFlock.MaxSpeed) {
	//			
	//			speedDifference = rigidbody.velocity.magnitude - myFlock.MaxSpeed;
	//			speedOffset += - myFlock.Drag * speedDifference;
	//		}
	//		
	//		return speedOffset;
	//	}
	
	
	//	/* */
	//	private void ObstacleHandling(Vector3 newVelocity) {
	//	
	//		Vector3 normalOffset;
	//		RaycastHit hit;
	//		Ray landingRay = new Ray(transform.position, newVelocity);
	//		Debug.DrawRay(transform.position, newVelocity*20, Color.black);
	//	
	//		//if a ray hits in the direction of the newVelocity
	//		if (Physics.Raycast (landingRay, out hit, rayLength)) {
	//	
	//			normalOffset = hit.normal - newVelocity;
	//	
	//			//vector projection
	//		}
	//	}
	
	
	///--------additional methods------------------------------------------------------------------------------------///
	
	
	//	/* */
	//	public bool IsInFront(ArrayList leaderBirds) {
	//	
	//		//check if there are any birds directly in front of this bird
	//		foreach (GameObject bird in leaderBirds) {
	//	
	//			//if the bird has no bird directly in front
	//			if (Vector3.Angle (transform.forward, bird.transform.position - transform.position) < 10) {
	//				return false;
	//			}
	//		}
	//		return true;
	//	}
}
