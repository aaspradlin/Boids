using UnityEngine;
using System.Collections;

/** <summary>
 * This class is used to apply a random vector to the <see cref="Boid"/> that owns it. </summary> */
public sealed class RandomVector {
	
	/** <summary>
	 * The time interval over which to apply the <see cref="randomOffset"/> before randomizing it. </summary> */
	private const float INTERVAL = 1.5f;
	
	/** <summary>
	 * The maximum magnitude of each component of the <see cref="randomOffset"/>. </summary> */
	private const float MAGNITUDE = 4f;
	
	/*-----------------------------------------------------------------------------------------*/

	/** <summary> 
	 * Applied over time to the movement of the bird. This is randomly generated every <see cref="INTERVAL"/>. </summary> */
	private Vector3 randomOffset = new Vector3(Random.Range(-MAGNITUDE, MAGNITUDE), 
	                                           Random.Range(-MAGNITUDE, MAGNITUDE), 
	                                           Random.Range(-MAGNITUDE, MAGNITUDE));
	
	/** <summary>
	 * Tracks the time between changes of the <see cref="randomOffset"/> vector. </summary> */
	private float randomCounter = 0;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Returns a random offset vector that is applied over time. This vector is relative to the boid's 
	 * own position. </summary> */
	public Vector3 Get_random_vector() {
		
		//if the counter has reached a certain point
		if (randomCounter > INTERVAL) {
			
			//reset the counter
			randomCounter = 0;
			
			//generate a new random vector to alter the bird's path over a second
			randomOffset = new Vector3(Random.Range(-MAGNITUDE, MAGNITUDE), 
			                           Random.Range(-MAGNITUDE, MAGNITUDE), 
			                           Random.Range(-MAGNITUDE, MAGNITUDE));
		}
		
		//add the passed time to the counter
		randomCounter += Time.deltaTime;
		
		//divide the passed time by the interval over which the random vector will be applied
		//and multiply that by the random vector
		return (Time.deltaTime / INTERVAL) * randomOffset;
	}
}
