using UnityEngine;
using System;

/** <summary>
 * This class controls the rotation and position of the camera. </summary> */
public class CameraController : MonoBehaviour {

	/** <summary>
	 * The <see cref="GameObject"/> to look at. </summary>*/
	public Transform target;
	
	/** <summary>
	 * The offset from the <see cref="target"/>. </summary>*/
	public Vector3 offset;

	/** <summary>
	 * This is used when calculating theta for camera rotation. It is necessary to store it because
	 * small approximation errors for calculating the radius propagate. </summary> */
	private float radius = -1;

	/*-----------------------------------------------------------------------------------------*/

	/** <summary>
	 * The speed at which to rotate the camera around the target when a key is pressed. </summary> */
	private const float ROT_FACTOR = 0.006f;

	/** <summary>
	 * The amount to adjust the <see cref="offset"/> by when keys are pressed. </summary> */
	private const float PAN_FACTOR = 0.05f;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * For initialization (see <see cref="MonoBehaviour"/>). </summary> */
	void Start () {
		//setup the camera for its target
		NewTarget ();
	}

	/** <summary>
	 * Called when a new target has been chosen. </summary> */
	private void NewTarget() {
		//look at the target
		radius = -1;
		transform.position = target.position + offset;
		transform.LookAt(target);
	}
	
	/** <summary> 
	 * Called once per frame (see <see cref="MonoBehaviour"/>). </summary> */
	void FixedUpdate () {

		if (target != null) {

			//check the important keys
			bool upPressed = Input.GetKey (KeyCode.UpArrow);
			bool wPressed = Input.GetKey (KeyCode.W);
			bool downPressed = Input.GetKey (KeyCode.DownArrow);
			bool sPressed = Input.GetKey (KeyCode.S);
			bool leftPressed = Input.GetKey (KeyCode.LeftArrow);
			bool aPressed = Input.GetKey (KeyCode.A);
			bool rightPressed = Input.GetKey (KeyCode.RightArrow);
			bool dPressed = Input.GetKey (KeyCode.D);

			//if up or down are pressed
			if (upPressed || wPressed || downPressed || sPressed) {

				radius = -1;
				if ((upPressed || wPressed) && ((target.position - transform.position).magnitude >= 2)) offset -= offset*PAN_FACTOR; //zoom in
				else if ((target.position - transform.position).magnitude < 60) offset += offset*PAN_FACTOR; //zoom out

			//if left or right are pressed use polar coordinates
			} else if (leftPressed || aPressed || rightPressed || dPressed) {

				//find the radius and current angle
				if (radius == -1) radius = (new Vector2(target.position.x, target.position.z) - new Vector2(transform.position.x, transform.position.z)).magnitude;

				float theta = (float)Math.Atan2(offset.z, offset.x); //use Atan2 because it's not quadrant neutral
				float modifier = ROT_FACTOR * Mathf.PI;

				//if left is pressed
				if (leftPressed || aPressed) theta -= modifier;

				//if right is pressed
				else theta += modifier;

				//modify the offset
				offset.x = radius * Mathf.Cos (theta);
				offset.z = radius * Mathf.Sin (theta);
			}

			//set the position of the camera
			transform.position = target.position + offset;

			//set up the rotation of the camera
			transform.LookAt(target);
		} 
	}

	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * The <see cref="target"/> to follow with the camera. </summary> */
	public Transform Target {
		set { target = value; NewTarget(); }
	}
}
