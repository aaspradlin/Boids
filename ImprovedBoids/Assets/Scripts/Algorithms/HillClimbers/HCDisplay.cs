using UnityEngine;
using System.Collections;

/** <summary>
 * Displays information about the executing Genetic Algorithm on the screen. </summary> */
public class HCDisplay : AlgorithmDisplay {
	
	/** <summary>
	 * Contains the information displayed about the algorithm in execution.</summary> */
	private GAGenome genome;
	
	/** <summary>
	 * Determines whether or not to display the box containing details about the genome.</summary> */
	private bool showing_details = false;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Called by the Algorithm under observation. This retrieves the GAGenome of the flock
	 * currently active in the scene so that updated information can be displayed. </summary> */
	override public void Update_listener() {
		
		genome = ((HillClimbing)subject).Get_current_genome ();
	}
	
	/** <summary>
	 * </summary> */
	override protected void Print_information () {
		
		Debug.Log ("genome: " + genome.BinaryString);
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Displays the algorithm's information to the screen.
	 * Called automatically.</summary> */
	void OnGUI() {
		
		//display default components on the screen
		base.DisplayGUI ();
		
		//create a text area to show the binary string
		GUI.Label(new Rect(side_padding,
		                   Screen.height - button_height - side_padding,
		                   Screen.width - (button_width + side_padding)*2 - side_padding,
		                   button_height), genome.BinaryString, custom_skin.GetStyle("Label"));

		//////////////
		
		//create a button to show more detailed information about the flock
		if (GUI.Button (new Rect (Screen.width - side_padding - button_width, 
		                          Screen.height - (button_height + side_padding)*2, 
		                          button_width, 
		                          button_height), "Details")) {
			
			//toggle detailed information
			showing_details = !showing_details;
		}
		
		//////////////
		
		//if the user has asked to see detailed information about the algorithm
		if (showing_details) {
			Show_details();
		}
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * On user request, displays a box containing information about the genome.</summary> */
	private void Show_details() {
		
		int box_height = 150;
		int box_width = 200;
		
		string details = "\n";
		
		//iterate through the coefficients array to build the details
		foreach (GACoefficient coefficient in genome.Coefficients) {
			details = details + "   " + coefficient.Name + " = " + coefficient.Value + "\n"; 
		}
		
		//display a box containing the information
		GUI.Box (new Rect (Screen.width - side_padding - box_width, 
		                   Screen.height - (button_height + side_padding)*2 - side_padding - box_height, 
		                   box_width, 
		                   box_height), details, custom_skin.GetStyle("Box"));
		
	}
}
