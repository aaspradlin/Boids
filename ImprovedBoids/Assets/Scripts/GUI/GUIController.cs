using UnityEngine;
using System.Collections;

/** <summary>
 * This class creates and deletes all GUI related scripts within the scene. </summary> */
public class GUIController : MonoBehaviour {

	/** <summary>
	 * Holds the names of all GUI scripts being displayed in the scene. </summary> */
	private ArrayList scripts_displayed = new ArrayList();

	/*--------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * Adds a GUI script to the scene that will be displayed (see <see cref="scripts_displayed"/>). </summary>
	 * <param name="display_script_name"> The name of the script to add to the scene </param> */
	public void Add_display(string display_script_name) {

		gameObject.AddComponent (display_script_name);
		scripts_displayed.Add (display_script_name);
	}

	/** <summary> 
	 * Removes the script with the specified name from the scene (see <see cref="scripts_displayed"/>). </summary>
	 * <param name="display_script_name"> The name of the script to remove from the scene </param> */
	public void Remove_display(string display_script_name) {

		Destroy (gameObject.GetComponent(display_script_name));
		scripts_displayed.Remove (display_script_name);
	}

	/** <summary>
	 * Clears all GUI scripts from the scene (see <see cref="scripts_displayed"/>). </summary> */
	public void Clear_display() {

		//remove each display from the screen
		foreach (string script_name in scripts_displayed) {
			Destroy (gameObject.GetComponent(script_name));
		}

		//replace the arraylist
		scripts_displayed = new ArrayList ();
	}
}
