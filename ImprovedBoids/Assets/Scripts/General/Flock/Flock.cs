using UnityEngine;
using System.Collections;

/** <summary>
 * This class holds boids and updates the velocity of each boid in the flock as well as keeping track of the center of
 * the flock. </summary> */
public class Flock : MonoBehaviour {
	
	/** <summary>
	 * The maximum speed at which birds in the flock should fly. </summary> */
	public static readonly float MAX_SPEED = 14f; 

	/** <summary>
	 * The minimum speed at which birds in the flock should fly. </summary> */
	public static readonly float MIN_SPEED = 10f;
	
	/*-----------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * The boids in the flock. </summary> */
	protected ArrayList boids = new ArrayList();
	
	/** <summary> 
	 * The flock's <see cref="VectorSet"/>. </summary> */
	private VectorSet vector_set; 
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Adds the given number of boids to the flock, instantiating them. </summary>
	 * <param name="num_boids"> The number of boids to instantiate and add to the flock </param> */
	public virtual void Populate_flock(int num_boids) {
		
		GameObject boid;
		Vector3 boid_position;
		
		//create the birds and add to the array
		for (int i = 0; i < num_boids; i++) {

			//TODO the spawn range should be based on math i.e. on the number of boids
			float spawnRange = 20;

			//determine where to position the boid
			boid_position = new Vector3(Random.Range(transform.position.x - (spawnRange/2), transform.position.x + (spawnRange/2)), 
			                            Random.Range(transform.position.y + (spawnRange/2) + 20, transform.position.y + (spawnRange/2) + 20), 
			                            Random.Range(transform.position.z - (spawnRange/2), transform.position.z + (spawnRange/2))); 
			
			//create the boid
			boid = Instantiate(Prefab.BOID_PREFAB, boid_position, Quaternion.identity) as GameObject;
			
			//set the boid's velocity to the flock's velocity
			boid.rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, rigidbody.velocity.z);
			
			//apply a rotation to the boid
			boid.transform.rotation = Quaternion.LookRotation(boid.rigidbody.velocity);
			
			//add the boid to the flock
			boids.Add(boid);
		}

		Update_average_transform();
	}
	
	
	/** <summary>
	 * Ends the life of the flock and all boids in the flock. </summary> */
	public void End_life() {
		
		//destroy all of the birds in the flock
		foreach (GameObject boid in boids) {
			Destroy(boid);
		}
		
		//destroys this object
		Destroy (gameObject);
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Called once per frame (see <see cref="MonoBehaviour"/>). </summary> */
	void FixedUpdate () {

		//perform default update actions
		Default_update();
	}

	/** <summary>
	 * This updates the velocity of each boid in the flock and updates the position of the flock itself. </summary> */
	protected void Default_update() {

		//update position of all the birds in the flock
		foreach (GameObject boid in boids) {
			
			//pass a Vector3 determined by the vector_set to the boid to modify its velocity
			boid.GetComponent<Boid>().Update_velocity(vector_set.Get_vector(boid, boids));
		}
		
		//update position of the flock
		Update_average_transform();
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Finds and sets the average position and velocity of the boids in the flock. </summary> */
	private void Update_average_transform() {

		if (boids.Count == 0) return;

		Vector3 average_position = Vector3.zero;
		Vector3 average_velocity = Vector3.zero;
		
		//add the velocity and position vectors of all birds in the flock
		foreach (GameObject boid in boids) {
			
			average_position += boid.transform.position;
			average_velocity += boid.rigidbody.velocity;
		}
		
		//change the velocity
		rigidbody.velocity = average_velocity / boids.Count; 
		
		//change the position
		transform.position = average_position / boids.Count;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Associates the flock with the <see cref="VectorSet"/> and the <see cref="VectorSet"/> with the flock. </summary>
	 * <param name="vector_set"> The <see cref="VectorSet"/> to associate with </param> 
	 * <returns> Whether the operation was successful </returns> */
	public bool Set_vector_set(VectorSet vector_set) {

		if (vector_set == null) return false;

		this.vector_set = vector_set;
		vector_set.Set_flock(gameObject);
		return true;
	}
}
