using UnityEngine;
using System.Collections;

/** <summary>
 * This class sets up the startup menu from which the <see cref="Algorithm"> is chosen. </summary> */
public sealed class StartMenu : MonoBehaviour {

	/** <summary>
	 * The <see cref="Algorithm"/>s that the user can choose from. </summary> */
	private Algorithm[] algorithm_options;
	
	/*--------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * This sets up the <see cref="Algorithm"/> options to be displayed (see <see cref="MonoBehaviour"/>). </summary>*/
	void Start () {

		//this is where you put the options in
		algorithm_options = new Algorithm[] { new Normal("Flocking"), new GeneticAlgorithm("Genetic Algorithm")};
	}

	/** <summary> 
	 * Displays the loader menu to the screen. Called automatically (see <see cref="MonoBehaviour"/>). </summary> */
	void OnGUI() {
		
		//set up the variables for spacing the menu items
		int button_width = 125;
		int button_height = 30;
		int menu_padding = 40;
		int button_padding = 10;

		//display the title of the program and the genome
		GUI.Box(new Rect((Screen.width - button_width)/2 - menu_padding, 
		                 (Screen.height/2) - ((button_height + button_padding)*algorithm_options.Length/2) - menu_padding, 
		                 button_width + (menu_padding * 2), 
		                 ((button_height + (button_padding*2))*algorithm_options.Length) + menu_padding), 
		        "Choose the Algorithm to Use"); 
		
		//create a button for each algorithm option and check if it's pressed
		for (int i = 0; i < algorithm_options.Length; i++) {
			
			//create a button for the option
			if (GUI.Button(new Rect((Screen.width - button_width)/2, 
			                        (Screen.height/2) - (button_height/2) - (((algorithm_options.Length - 1)*(button_height + button_padding))/2) + i*(button_height + button_padding), 
			                        button_width, 
			                        button_height), 
			               algorithm_options[i].Name)) { 
				
				//pass the algorithm to the overall flock controller
				SceneController.Start_algorithm(algorithm_options[i]); 
			} 
		}
	}
}
