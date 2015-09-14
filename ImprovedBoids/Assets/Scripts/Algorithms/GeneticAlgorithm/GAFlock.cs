using UnityEngine;
using System.Collections;

/** <summary>
 * This class adds functionality to the <see cref="Flock"/> class. This is necessary to calculate the
 * fitness of the flock for the genetic algorithm. This added functionality includes tracking the number of
 * collisions experienced by boids and the average distance of each boid from the flock's center. </summary> */
public class GAFlock : Flock, CollisionListener {

	/** <summary>
	 * Counts the number of collisions that have occured within the flock. </summary> */
	private int collision_counter = 0;
	
	/** <summary> 
	 * The average distance between each bird and the center of the flock. </summary> */
	private float average_distance = 0; 

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Called on Start. Begins any relevant coroutines (see <see cref="MonoBehaviour"/>). </summary> */
	void Start() {

		StartCoroutine("Calculate_average_distance");
	}

	/** <summary>
	 * Populates the flock with the given number of boids and registers the flock as 
	 * a <see cref="CollisionListener"/> of each boid. </summary> 
	 * <param name="num_boids> The number of boids to generate for the flock </param> */
	override public void Populate_flock(int num_boids) {

		//populate the flock normally
		base.Populate_flock(num_boids);

		//give the flock a random color
		Color color = new Color(Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
		Boid boidScript;

		//register the flock as a listener to each boid in the flock and sets the color of each boid
		foreach (GameObject boid in boids) {
			boidScript = boid.GetComponent<Boid>();
			boidScript.Register_collision_listener(this);
			boidScript.SetColor(color);
		}
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Used in a coroutine to track the average distance between boids in the flock
	 * and the position of the flock. </summary> */
	IEnumerator Calculate_average_distance() {

		float timeToStart = 1.5f;
		float updateTime = 0.25f;

		//wait for the flock to reach a balance before measuring 
		yield return new WaitForSeconds(timeToStart);

		//repeat this process over the course of the flock's life
		for (int i = 1; i < 10000; i++) { 
			
			float distance_subtotal = 0;
			
			//add the distance between each boid and the flock to an accumulator
			foreach (GameObject boid in boids) {
				distance_subtotal += (boid.transform.position - transform.position).magnitude;
			}
			
			//recalculate the average distance from the flock
			average_distance = (average_distance*(i - 1) + (distance_subtotal / boids.Count)) / i;
			
			//wait 0.5 s to do it again
			yield return new WaitForSeconds(updateTime);
		}
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Called when a boid in the flock has experienced a collision (see <see cref="collision_counter"/>). </summary> */
	public void Notify_collision() {
		collision_counter++;
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * The number of collisions that have occured between boids in the flock. </summary> */
	public int Num_Collisions {
		get { return collision_counter; }
	}

	/** <summary>
	 * The average distance between each bird and the center of the flock. </summary> */
	public float Average_Distance {
		get { return average_distance; }
	}
}
