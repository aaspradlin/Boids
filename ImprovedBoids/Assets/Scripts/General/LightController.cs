using UnityEngine;
using System.Collections;

/** <summary> 
 * This class controls a light that is aimed at a target. </summary> */
public class LightController : MonoBehaviour {
	
	/** <summary> 
	 * The <see cref="GameObject"/> to look at. </summary> */
	public Transform target;
	
	/** <summary>
	 * The offset from the <see cref="target"/>. </summary> */
	public Vector3 offset;
	
	
	///------monobehaviour------------------------------------------------------------------------------------------///
	
	
	/** <summary>
	 * For initialization (see <see cref="MonoBehaviour"/>). </summary> */
	void Start () {
		
		//look at the target on start
		transform.position = target.position + offset;
		transform.LookAt(target);
	}
	
	/** <summary> 
	 * Called once per frame (see <see cref="MonoBehaviour"/>). </summary> */
	void Update () {
		
		if (target != null) {
			
			//set up the rotation of the light
			transform.LookAt(target.position);

			//set the position of the camera
			transform.position = target.position + offset;
		} else {
			Debug.Log ("Light target is null");
		}
	}

	///---------properties------------------------------------------------------------------------------------------///
	
	/** <summary>
	 * The <see cref="target"/> to follow with the light. </summary> */
	public Transform Target {
		set { target = value; }
	}
}
