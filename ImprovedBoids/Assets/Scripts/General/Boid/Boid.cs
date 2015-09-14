using UnityEngine;
using System.Collections;

/** <summary>
 * This class controls the position of the boid object and monitors for collisions. </summary> */
public class Boid : MonoBehaviour {
	
	/** <summary> 
	 * A listener that receives notifications of collisions. </summary> */
	private ArrayList listeners = new ArrayList();
	
	/** <summary>
	 * Used to apply random vectors to the boid over time. </summary> */
	private RandomVector randomizer = new RandomVector();

	/** <summary>
	 * The boid's angle of vision. This is from the center to the left or right. </summary> */
	public static readonly int VISION_ANGLE = 95; //TODO figure out this bug

	/** <summary>
	 * The distance at which a boid no longer considers another boid as in its flock. </summary> */
	public static readonly int RADIUS = 12;

	/** <summary>
	 * The distance that a boid tries to stay away from each of its neighboring boids. </summary> */
	public static readonly float SEPARATION_DISTANCE = 6;
	
	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Updates the position and rotation of the boid based on the <see cref="VectorSet"/>. </summary>
	 * <param name="direction_vector"> The vector used to update the boid's velocity </param> */
	public void Update_velocity (Vector3 direction_vector) {

		//calculate bird's new velocity based on the directionVector and the update time
		Vector3 new_velocity = rigidbody.velocity + (direction_vector + randomizer.Get_random_vector()) * Time.deltaTime;
		
		//apply a turn
		Quaternion target_rotation = Quaternion.LookRotation(new_velocity);
		transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, 30*Time.deltaTime);

		//set the new velocity
		rigidbody.velocity = new_velocity;
	}
	
	/** <summary> 
	 * Register a CollisionListener that will receive notifications of collisions. </summary>
	 * <param name="listener"> The listener set to be informed whenever a collision occurs </param>
	 * <returns> Whether the operation was successful </returns> */
	public bool Register_collision_listener(CollisionListener listener) {

		if (listener == null) return false;

		listeners.Add (listener);
		return true;
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * On collision notify any listeners that a collision has occurred. </summary> 
	 * <param name="collision"> The <see cref="Collision"/> object created by the event </param> */
	void OnCollisionEnter(Collision collision) {
		
		foreach(CollisionListener listener in listeners) listener.Notify_collision ();
	}

	/** <summary>
	 * Sets the color of the boid object. </summary>
	 * <param name="color"> The color </param> */
	public void SetColor(Color color) {

		gameObject.renderer.material.color = color;
	}
}
