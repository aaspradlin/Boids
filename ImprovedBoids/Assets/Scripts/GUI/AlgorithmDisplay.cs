using UnityEngine;
using System.Collections;

/** <summary> 
 * This class provides default GUI Components and is extended by other classes to provide 
 * information about the algorithm being performed. </summary> */
public abstract class AlgorithmDisplay : MonoBehaviour, AlgorithmListener {

	/** <summary> 
	 * The Algorithm object to monitor and display information about. </summary> */
	protected Algorithm subject;

	/** <summary>
	 * The pixel width of buttons displayed on the screen. </summary> */
	protected int button_width = 100;

	/** <summary>
	 * The pixel height of buttons displayed on the screen. </summary> */
	protected int button_height = 30;

	/** <summary>
	 * The padding (blank space) between GUI components displayed on the screen. </summary> */
	protected int side_padding = 5;

	/** <summary>
	 * Contains needed style information for the GUI about labels, buttons, etc. This can be modified from within Unity. </summary> */
	protected GUISkin custom_skin = (GUISkin)Resources.Load("CustomFonts/BinaryString", typeof(GUISkin));


	/*--------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * Records information about the algorithm (see <see cref="subject"/>) in a text file 
	 * upon user request. The information recorded is specific to the algorithm being run. </summary> */
	protected abstract void Print_information ();

	/** <summary>
	 * This is called when the state of the algorithm has changed and the display needs to be updated
	 * to reflect the changes (see <see cref="AlgorithmListener"/>). </summary> */
	public abstract void Update_listener();

	/*--------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Displays the default GUI components on the screen. This includes the name of the algorithm,
	 * a button for stopping the algorithm, and a button for recording information about the current scene. </summary> */
	protected void DisplayGUI() {

		//create a label to show the name of the algorithm
		GUI.Label(new Rect(side_padding,
		                   side_padding,
		                   Screen.width - side_padding*2,
		                   button_height), subject.Name, custom_skin.GetStyle("Label"));


		//create a button for stopping the algorithm
		if (GUI.Button (new Rect (Screen.width - button_width - side_padding, 
		                        Screen.height - button_height - side_padding, 
		                        button_width, 
		                        button_height), "End Run")) {

			//stop the algorithm
			SceneController.Stop_algorithm();
		}


		//create a button for recording information about the current scene
		if (GUI.Button (new Rect (Screen.width - (button_width + side_padding)*2, 
		                          Screen.height - button_height - side_padding, 
		                          button_width, 
		                          button_height), "Print")) {
			
			//record relevant information
			Print_information();
		}
	}

	/*--------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Assigns the display to the given Algorithm (see <see cref="AlgorithmListener"/>). </summary>
	 * <param name="subject"> algorithm to monitor </param> */
	public Algorithm Subject {
		set { subject = value; }
	}
}
