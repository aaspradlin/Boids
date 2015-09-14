using UnityEngine;
using System.Collections;

/** <summary>
 * This class controls the scene, keeping track of the camera and the algorithm in use. Creation of flocks is
 * handled by this class. </summary> */
public class SceneController : MonoBehaviour { 
	
	/** <summary>
	 * The algorithm currently employed in the scene. </summary> */
	private static Algorithm algorithm;
	
	/** <summary>
	 * The script used to aim the camera. </summary> */
	private static CameraController camera_controller;

	/** <summary>
	 * The script used to add and remove displays from the screen. </summary> */
	private static GUIController gui_controller;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Sets the current algorithm used in the scene. </summary>
	 * <param name="algorithm"> The <see cref="Algorithm"/> object to initiate </param> */
	public static void Start_algorithm(Algorithm algorithm) {
		
		//set this to the current algorithm
		SceneController.algorithm = algorithm;
		
		//remove the start menu from the scene
		gui_controller.Remove_display ("StartMenu");
		
		//start setting up the scene with the algorithm
		algorithm.Initiate_algorithm();
	}
	
	/** <summary>
	 * Ends the algorithm's execution and launches the start menu for the user. </summary> */
	public static void Stop_algorithm() {

		//stop the algorithm
		algorithm.Stop_algorithm ();

		//display the start menu
		gui_controller.Add_display ("StartMenu"); 
	}
	
	/** <summary>
	 * Creates a new flock for the scene. </summary>
	 * <param name="flockPrefab"> The template for the flock <see cref="GameObject"/> to instantiate </param>
	 * <param name="position"> The position at which to instantiate the object </param>
	 * <param name="num_boids"> The number of boids initially in the flock </param>
	 * <param name="vector_set"> The flock's <see cref="VectorSet"/> object </param>
	 * <returns> The <see cref="GameObject"/> created </returns> */
	public static GameObject Create_flock(GameObject flockPrefab, Vector3 position, int num_boids, VectorSet vector_set) {
		
		//create the flock
		GameObject flock = Instantiate(flockPrefab, position, Quaternion.identity) as GameObject;
		
		//set the velocity
		flock.rigidbody.velocity = new Vector3(1, 0, (Flock.MIN_SPEED + Flock.MAX_SPEED) / 2);
		
		//point the camera at the flock
		Set_camera_target(flock);
		
		//assign a vector set to the flock
		flock.GetComponent<Flock>().Set_vector_set(vector_set);
		
		//add boids to the flock
		flock.GetComponent<Flock>().Populate_flock(num_boids);
		
		return flock;
	}
	
	/** <summary>
	 * Sets the camera's target. </summary>
	 * <param name="target"> The <see cref="GameObject"/> towards which to point the camera </param> */
	private static void Set_camera_target(GameObject target) {
		
		camera_controller.Target = target.transform;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * This sets up the scene, grabbing GameObjects and displaying a start menu. </summary> */
	void Start () {
		
		//get the script for the camera using the camera's tag in the scene
		camera_controller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
		
		//add a start menu to the scene
		gui_controller = gameObject.GetComponent<GUIController>(); 
		gui_controller.Add_display("StartMenu");
	}
}
