using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** <summary>
 * This class is used to determine how to change the velocity of a boid. </summary> */
public class NormalVectorSet : VectorSet {

	/** <summary>
	 * </summary> */
	private Coefficient followCoefficient;

	/** <summary>
	 * </summary> */
	private Coefficient cohesionCoefficient;

	/** <summary>
	 * </summary> */
	private Coefficient separationCoefficient;

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * Primary constructor. //TODO add parameters </summary> */
	public NormalVectorSet(float follow, float cohesion, float separation) {
		followCoefficient = new Coefficient(follow, "Follow");
		cohesionCoefficient = new Coefficient(cohesion, "Cohesion");
		separationCoefficient = new Coefficient(separation, "Separation");
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
		Vector3 directionVector = (Follow (boid, leader_boids) * followCoefficient.Value) 
			+ (Cohesion(boid, leader_boids) * cohesionCoefficient.Value) 
				+ (Separation (boid, leader_boids) * separationCoefficient.Value);
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
				if (thisSeparation.magnitude <= Boid.RADIUS) { 
					
					//if the angle between the bird's velocity vector and thisSeparation vector
					//is within the bird's angle of vision, add it to leaderBirds
					if (Vector3.Angle(boid.rigidbody.velocity, thisSeparation) < Boid.VISION_ANGLE) {
						leader_boids.Add (other_boid);
						//TODO uh oh this doesn't work
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
		
		if (boids.Count == 0) return flockOffset;
		
		else {
			//add the offsets from all the birds in the passed arraylist and average them
			foreach (GameObject other_boid in boids) 
				flockOffset += other_boid.transform.position - boid.transform.position;
			
			return flockOffset / boids.Count;
		}
	}
	
	/** <summary> 
	 * Finds the offset gotten from the positions of boids in the passed list. 
	 * This vector is relative to the boid's position. </summary>
	 * <param name="boid"> The boid this is being calculated for </param>
	 * <param name="boids"> The boids important to the calculation </param>
	 * <returns> The offset </returns> */
	private Vector3 Separation(GameObject boid, ArrayList boids) {
		
		Vector3 separationOffset = Vector3.zero;

		//TODO uh oh. this is not proportional

		//if the passed arraylist isn't empty
		if (boids.Count != 0) {
			
			foreach(GameObject other_boid in boids) {
				
				//vector pointing from the bird in the arraylist to this bird
				Vector3 thisOffset = boid.transform.position - other_boid.transform.position;
				
				//if this bird is too close, add the offset
				if (thisOffset.magnitude < Boid.SEPARATION_DISTANCE) {

					separationOffset += thisOffset;
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

	//TODO add bounding vector

	public Coefficient[] Coefficients {
		get { return new Coefficient[3]{separationCoefficient, cohesionCoefficient, followCoefficient}; }
	}

	public float SeparationCoefficient {
		get { return separationCoefficient.Value; }
		set { separationCoefficient.Value = value; }
	}

	public float CohesionCoefficient {
		get { return cohesionCoefficient.Value; }
		set { cohesionCoefficient.Value = value; }
	}

	public float FollowCoefficient {
		get { return followCoefficient.Value; }
		set { followCoefficient.Value = value; }
	}
}
