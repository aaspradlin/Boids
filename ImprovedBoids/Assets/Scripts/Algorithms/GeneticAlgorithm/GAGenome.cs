using UnityEngine;
using System.Collections;


/** <summary>
 * This class holds the binary string for a flock and is used to retrieve coefficient values. </summary> */
public sealed class GAGenome {

	/** <summary>
	 * The length of the binary string. </summary> */
	public static int LENGTH; 

	/** <summary>
	 * The fitness of the genome overall. This is calculated at the end of a flock's run. </summary> */
	private float fitness;
	
	/** <summary>
	 * The binary string containing the genetic information. </summary> */
	private string binaryString;
	
	/** <summary>
	 * An array holding the coefficients in the order they appear in the binary string. </summary> */
	private GACoefficient[] coefficientsArray;

	/** <summary> 
	 * Coefficient to tweak vector weights. </summary> */
	private GACoefficient cohesionCoefficient = new GACoefficient(1, 5, "Cohesion Coefficient", 0.02f);

	/** <summary> 
	 * Coefficient to tweak vector weights. </summary> */
	private GACoefficient separationCoefficient = new GACoefficient(0.5f, 5, "Separation Coefficient", 0.02f);

	/** <summary> 
	 * Coefficient to tweak vector weights. </summary> */
	private GACoefficient followCoefficient = new GACoefficient(0.5f, 5, "Follow Coefficient", 0.02f);
	
	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * This is called when the genetic algorithm is started. It is primarily used to determine the length
	 * that binary strings will be. </summary> */
	public static void Initialize() {

		//TODO this is a messy way of figuring out the length

		GACoefficient cohesion  = new GACoefficient(1, 5, "Desired Separation", 0.02f);
		GACoefficient separation = new GACoefficient(0.5f, 5, "Desired Separation", 0.02f);
		GACoefficient follow = new GACoefficient(0.5f, 5, "Desired Separation", 0.02f);

		LENGTH = GACoefficient.TotalLength;
	}

	/** <summary>
	 * Primary constructor. </summary>
	 * <param name="genome"> The string storing the coefficient values </param> */
	public GAGenome(string genome) {

		coefficientsArray = new GACoefficient[] {cohesionCoefficient, separationCoefficient, followCoefficient};

		//store the given string
		binaryString = genome;

		//interpret the binary string, loading the appropriate values into the coefficient objects
		Read_values();
	}
	
	/** <summary>
	 * Constructor that uses a randomly generated genome. </summary> */
	public GAGenome() : this(GenerateRandomMember()) { }
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Loads the values of the coefficients from the <see cref="binaryString"/>. </summary> */
	private void Read_values() {

		string splitBinaryString;
		float desiredValue = 0;
		int string_position = 0;
		
		//for each coefficient in the array
		for (int k = 0; k < coefficientsArray.Length; k++) {

			//get the substring specific to the coefficient
			splitBinaryString = binaryString.Substring(string_position, coefficientsArray[k].Length);
			
			//convert the parsed string to a float
			desiredValue = ToDecimal (splitBinaryString);
			
			//divide by 10 to the power given by the coefficient then add the added value of the coefficient
			desiredValue = (desiredValue*coefficientsArray[k].Multiplier) + coefficientsArray[k].Added_value;
			
			coefficientsArray[k].Value = desiredValue;
			
			//jump to beginning of next coefficient in the string
			string_position += coefficientsArray[k].Length;
		}
	}
	
	/** <summary>
	 * Converts a binary string to a float. </summary>
	 * <param name="binString"> The binary string to convert to a decimal number </param> 
	 * <returns> The decimal value of the binary string </returns> */ 
	private int ToDecimal(string binString) {
		
		char[] bin_char_array = binString.ToCharArray();
		int value = 0;
		
		//convert from binary to integer
		for (int i = 0; i < bin_char_array.Length; i++) {
			value += (int)(char.GetNumericValue (bin_char_array[i]) * Mathf.Pow (2, bin_char_array.Length - 1 - i));
		}
		
		return value;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Generates a random binary string for a genome. </summary>
	 * <returns> A random binary string </random> */
	private static string GenerateRandomMember() {

		string binaryString = "";
		
		//add a "0" or a "1" to the string
		for (int i = 0; i < LENGTH; i++) {
			binaryString += Random.Range(0,2).ToString();
		}
		
		return binaryString;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * The fitness of the genome overall. This is calculated at the end of a flock's run. </summary> */
	public float Fitness {
		get { return fitness; }
		set { this.fitness = value; }
	}

	/** <summary>
	 * The binary string containing the genetic information. </summary> */
	public string BinaryString {
		get { return binaryString; }
	}

	/** <summary> 
	 * Coefficient to tweak vector weights. </summary> */
	public float SeparationCoefficient {
		get { return separationCoefficient.Value; }
	}

	/** <summary> 
	 * Coefficient to tweak vector weights. </summary> */
	public float FollowCoefficient {
		get { return followCoefficient.Value; }
	}

	/** <summary> 
	 * Coefficient to tweak vector weights. </summary> */
	public float CohesionCoefficient {
		get { return cohesionCoefficient.Value; }
	}

	/** <summary>
	 * An array holding the coefficients in the order they appear in the binary string. </summary> */
	public GACoefficient[] Coefficients {
		get { return coefficientsArray; }
	}
}
