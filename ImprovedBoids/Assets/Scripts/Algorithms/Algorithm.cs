using UnityEngine;
using System.Collections;

/** <summary> 
 * The class all the algorithms extend. See GoF Strategy design pattern. </summary> */
public abstract class Algorithm {
	
	/** <summary> 
	 * The name of the algorithm. </summary> */
	private readonly string name;
	
	/** <summary> 
	 * The name of the scripts currently displaying the algorithm's information to the GUI. </summary> */
	private ArrayList display_script_names = new ArrayList();

	/** <summary> 
	 * The algorithm's listeners </summary> */
	private ArrayList listeners = new ArrayList();

	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Primary constructor. </summary>
	 * <param name="name"> The name of the algorithm </param>
	 * <param name="display_script_name"> The name of the script containing the algorithm's GUI </param> */
	public Algorithm(string name) { 

		this.name = name;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Called to start running the algorithm in the scene. Implementation is 
	 * specific to the child type. </summary> */
	public abstract void Initiate_algorithm();
	
	/** <summary> 
	 * Called to stop running the algorithm in the scene. Implementation is specific
	 * to the child type. </summary> */
	public abstract void Stop_algorithm ();

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * Called by the child's <see cref="Stop_algorithm()"/> method to remove the display and stop all 
	 * coroutines. </summary> */
	protected void Default_stop() {

		//end any coroutines running
		GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().StopAllCoroutines();

		//remove all of the currently displayed scripts from the scene
		foreach (string display_name in display_script_names.ToArray())
			RemoveDisplay(display_name);
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Creates a flock <see cref="GameObject"/>, sets its velocity, gives it a <see cref="VectorSet"/>, and populates it with boids. </summary>
	 * <param name="vector_set"> The <see cref="VectorSet"/> to be used by the flock </param>
	 * <param name="num_boids"> The number of boids with which to populate the flock </param>
	 * <param name="prefab"> The <see cref="GameObject"/> template to use </param> */
	protected GameObject Create_flock(GameObject prefab, VectorSet vector_set, int num_boids) {
		
		//create the flock and provide it with a speed
		return SceneController.Create_flock(prefab, Vector3.zero, num_boids, vector_set);
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * Tells the <see cref="SceneController"/> to start the given coroutine. </summary>
	 * <param name="coroutine"> The <see cref="IEnumerator"/> coroutine to run </param> */
	protected void StartCoroutine(IEnumerator coroutine) {

		GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().StartCoroutine(coroutine);
	}

	/** <summary> 
	 * Tells the <see cref="SceneController"/> to stop the given coroutine. </summary>
	 * <param name="coroutine"> The coroutine to stop </param> */
	protected void StopCoroutine(string coroutine) {
		
		GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>().StopCoroutine(coroutine);
	}

	/** <summary>
	 * Adds the display to the scene using the <see cref="GameController"/>. </summary>
	 * <param name="display_name"> The name of the display's script </param>
	 * <param name="addListener"> Whether or not to add the display as a listener </param> */
	protected void AddDisplay(string display_name, bool addListener) {

		//add the GUI object to the scene
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GUIController>().Add_display (display_name);

		//add it to the ArrayList
		display_script_names.Add(display_name);

		if (addListener)
			Attach((AlgorithmListener)GameObject.FindGameObjectWithTag("GameController").GetComponent(display_name));
	}

	/** <summary>
	 * Removes the display from the scene using the <see cref="GameController"/>. </summary>
	 * <param name="display_name"> The name of the display's script </param> */
	protected void RemoveDisplay(string display_name) {

		//remove the display from the list
		display_script_names.Remove (display_name);

		//remove the display as a listener
		Detach((AlgorithmListener)GameObject.FindGameObjectWithTag("GameController").GetComponent(display_name));
		
		//remove the GUI object from the scene
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GUIController>().Remove_display (display_name);
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Registers a listener with the algorithm </summary>
	 * <param name="listener"> The <see cref="AlgorithmListener"/>  </param> 
	 * <returns> Whether the operation was successful </returns> */
	public bool Attach(AlgorithmListener listener) {

		if (listener == null) return false;

		listeners.Add (listener);
		listener.Subject = this;
		return true;
	}

	/** <summary>
	 * Removes a listener from the algorithm </summary>
	 * <param name="listener"> The <see cref="AlgorithmListener"/>  </param> */
	public void Detach(AlgorithmListener listener) {
		listeners.Remove (listener);
	}

	/** <summary>
	 * Tell the listeners that the state of the algorithm has changed </summary> */
	protected void Notify() {
		foreach (AlgorithmListener listener in listeners) 
			listener.Update_listener ();
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * The name of the algorithm. </summary> */
	public string Name {
		get { return name; }
	}
}