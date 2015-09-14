using UnityEngine;
using System.Collections;


/** <summary>
 * This class displays information after a generation ends. </summary> */
public class GAGenerationDisplay : AlgorithmDisplay {
	
	/** <summary>
	 * Called by the Algorithm under observation. </summary> */
	override public void Update_listener() { }
	
	/** <summary>
	 * This writes information about the current state of the algorithm to a text file. </summary> */
	override protected void Print_information () {

		//TODO figure out what to write to a file

		string message = "generation fitness: ";
		Debug.Log (message);
		
		//write the message to a file
		FileWriter.WriteToFile(message);
	}

	/** <summary> 
	 * Displays the genetic algorithm's information on the screen.
	 * Called automatically (see <see cref="MonoBehaviour"/>). </summary> */
	void OnGUI() {

		//display basic algorithm things
		base.DisplayGUI();

		string message = "Generation: " + ((GeneticAlgorithm)subject).Generation.ToString () + "\nAverage Fitness: " + ((GeneticAlgorithm)subject).AverageFitness.ToString();

		//display the title of the program and the genome
		GUI.Box(new Rect((Screen.width - button_width)/2 - 30, (Screen.height/2) - 40, 250, 40), message); 
	}
}
