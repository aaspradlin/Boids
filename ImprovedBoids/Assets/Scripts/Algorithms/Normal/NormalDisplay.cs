using UnityEngine;
using System.Collections;

/** <summary>
 * This class displays information about the flock to the screen. </summary> */
public class NormalDisplay : AlgorithmDisplay {

	/** <summary>
 	* The strings currently in the text fields. </summary> */
	private string[] coefficients = new string[] {"1.5", "1.5", "1.5"};

	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Called by the Algorithm under observation. </summary> */
	override public void Update_listener() { 

		//update each of the coefficients
		for (int i = 0; i < coefficients.Length; i++) {
			coefficients[i] = ((Normal)subject).Coefficients[i].Value.ToString();
		}
	}
	
	/** <summary>
	 * This writes information about the current state of the flock to a text file. </summary> */
	override protected void Print_information () {

		string message = "Coefficients: \n";

		//add each of the coefficients to the message
		foreach (Coefficient coefficient in ((Normal)subject).Coefficients) {
			message += "   " + coefficient.Name + ": " + coefficient.Value + "\n";
		}

		Debug.Log (message);
		
		//write the message to a file
		FileWriter.WriteToFile(message);
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Displays the flock's information to the screen.
	 * Called automatically (see <see cref="MonoBehaviour"/>). </summary> */
	void OnGUI() {

		//display default components on the screen
		base.DisplayGUI ();

		//check if enter has been pressed
		if (Event.current.keyCode == KeyCode.Return && TextFieldIsFocus()) Update_Values();

		//for the labels above the text fields
		GUI.skin.label.fontSize = 11;
		int textFieldLength = 50;
		int textFieldHeight = 20;
		int maxFieldChars = 5;

		//create textfields
		for (int i = 0; i < coefficients.Length; i++) {

			//gives the textfields control names so that they can be identified as focuses
			GUI.SetNextControlName(i.ToString ());

			coefficients[i] = GUI.TextField (new Rect(side_padding + ((side_padding + textFieldLength) * i), 
			                                          Screen.height - textFieldHeight - side_padding,
			                                          textFieldLength, textFieldHeight), coefficients[i], maxFieldChars);

			GUI.Label (new Rect(side_padding + ((side_padding + textFieldLength) * i),
			                    Screen.height - 40,
			                    Screen.width - (side_padding * 2),
			                    button_height), ((Normal)subject).Coefficients[i].Name);
		}

		//create a button to reset the flock
		if (GUI.Button (new Rect (Screen.width - side_padding - button_width, 
		                          Screen.height - (button_height + side_padding)*2, 
		                          button_width, button_height), "Reset")) {
			((Normal)subject).Reset();
			Update_listener();
		}

		//create a button to change the flock based on the content of the text areas
		if (GUI.Button (new Rect (180, 
		                          Screen.height - button_height - side_padding, 
		                          button_width, button_height), "Apply")) {
			Update_Values();
		}
	}

	/** <summary>
	 * Takes the values in the TextFields and updates the flock's coefficients
	 * based on their contents. </summary> */
	private void Update_Values() {

		float acceptableMin = 0.1f;
		float acceptableMax = 10;

		//validate each input
		for (int i = 0; i < coefficients.Length; i++) {
			
			try {				
				if (float.Parse (coefficients[i]) < acceptableMin) coefficients[i] = acceptableMin.ToString();
				if (float.Parse (coefficients[i]) > acceptableMax) coefficients[i] = acceptableMax.ToString();
				((Normal)subject).Coefficients[i].Value = float.Parse (coefficients[i]);
			} catch (System.FormatException e) {
				coefficients[i] = ((Normal)subject).Coefficients[i].Value.ToString();
			}
		}
	}

	/** <summary>
	 * Checks if a TextField is the current focus of the GUI. </summary>
	 * <returns> Whether a TextField is the current focus </returns> */
	private bool TextFieldIsFocus() {
		string focus = GUI.GetNameOfFocusedControl();
		for (int i = 0; i < coefficients.Length; i++) {
			if (focus == i.ToString ()) return true;
			
		}
		return false;
	}
}
