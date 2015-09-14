using UnityEngine;
using System.Collections;

/** <summary>
 * This class displays information about the executing Genetic Algorithm on the screen. </summary> */
public class GADisplay : AlgorithmDisplay {

	/** <summary>
	 * Contains the information displayed about the genetic algorithm being executed. </summary> */
	private GAGenome genome;

	/** <summary>
	 * Determines whether or not to display the box containing details about the genome. </summary> */
	private static bool showing_details = false;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Called by the Algorithm under observation. This retrieves the GAGenome of the flock
	 * currently active in the scene so that updated information can be displayed. </summary> */
	override public void Update_listener() {

		genome = ((GeneticAlgorithm)subject).Get_current_genome ();
	}

	/** <summary>
	 * This writes information about the current state of the algorithm to a text file. </summary> */
	override protected void Print_information () {

		string message = "genome: " + genome.BinaryString + "\n";

		//iterate through the coefficients array to build the details
		foreach (GACoefficient coefficient in genome.Coefficients) 
			message += "   " + coefficient.Name + " = " + coefficient.Value + "\n"; 
		
		Debug.Log (message);

		//write the message to a file
		FileWriter.WriteToFile(message);
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary> 
	 * Displays the genetic algorithm's information on the screen.
	 * Called automatically (see <see cref="MonoBehaviour"/>). </summary> */
	void OnGUI() {

		//display default components on the screen
		base.DisplayGUI ();

		//create a text area to show the binary string
		GUI.Label(new Rect(side_padding,
		                      Screen.height - button_height - side_padding,
		                      Screen.width - (button_width + side_padding)*2 - side_padding,
		                      button_height), genome.BinaryString, custom_skin.GetStyle("Label"));

		//////////////

		string member_label = "Generation: " + ((GeneticAlgorithm)subject).Generation + "\nMember: " + (((GeneticAlgorithm)subject).Member_Number + 1);

		//create a text area to show which flock is currently in progress
		GUI.Label(new Rect(side_padding,
		                   button_height + side_padding,
		                   Screen.width - side_padding*2,
		                   button_height*1.8f), member_label, custom_skin.GetStyle("Label"));

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
	 * On user request, displays a box containing information about the genome (see <see cref="showing_details"/>). </summary> */
	private void Show_details() {

		int box_height = 150;
		int box_width = 200;

		string details = "\n";

		//iterate through the coefficients array to build the details
		foreach (GACoefficient coefficient in genome.Coefficients) 
			details += "   " + coefficient.Name + " = " + coefficient.Value + "\n"; 

		//if the fitness has been assigned, display its information
		details += "\n   Collisions: " + ((GeneticAlgorithm)subject).Collisions;
		details += "\n   Speed: " + ((GeneticAlgorithm)subject).Flock_Speed;

		details += "\n\n   Fitness: " + genome.Fitness; 

		//display a box containing the information
		GUI.Box (new Rect (Screen.width - side_padding - box_width, 
							Screen.height - (button_height + side_padding)*2 - side_padding - box_height, 
							box_width, 
							box_height), details, custom_skin.GetStyle("Box"));
	}
}
