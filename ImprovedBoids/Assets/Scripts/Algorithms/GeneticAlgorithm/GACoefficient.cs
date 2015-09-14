using UnityEngine;
using System.Collections;

/** <summary>
 * This class is used exclusively by <see cref="GAGenome"/> to store information about the coefficients relevant to the
 * flock's flight. </summary> */
public class GACoefficient : Coefficient {

	//TODO extend coefficient

	/** <summary>
	 * The cumulative length of all existing coefficients. </summary> */
	private static int totalLength = 0;

	/** <summary>
	 * The length of the binary string segment for this value. </summary> */
	private int length = 0;
	
	/** <summary>
	 * The value to be multiplied with the binary representation of the value. </summary> */
	private float multiplier;
	
	/** <summary>
	 * The value to add to the coefficient after division. </summary> */
	private float added_value;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Primary Constructor. </summary>
	 * <param name="minValue"> The minimum allowed value for the coefficient </param>
	 * <param name="maxValue"> The goal maximum value for the coefficient </param>
	 * <param name="name"> The name of the coefficient </param>
	 * <param name="accuracy"> The desired lowest increment for the value </param> */
	public GACoefficient(float minValue, float maxValue, string name, float accuracy) : base(name) {
		
		//set the values
		added_value = minValue;
		multiplier = accuracy;
		
		//sets the length of the coefficient in a binary string
		Calculate_length (maxValue, minValue);
	}


	/** <summary>
	 * This takes the coefficient's goal maximum value, minimum value, and multiplier (specifies accuracy) and
	 * uses them to find the necessary number of bits needed to store the coefficient. </summary> 
	 * <param name="maxValue"> The goal maximum value of the coefficient </param> 
	 * <param name="minValue"> The minimum value of the coefficient </param> */
	private void Calculate_length(float maxValue, float minValue) {

		float goalMax = maxValue - minValue;

		//sets the length of the coefficient in a binary string
		//uses change of base formula
		int tempLength = Mathf.CeilToInt (Mathf.Log ((goalMax/multiplier) + 1) / Mathf.Log (2));

		//adjust the length and multiplier to have the max value be within 20% of desired
		while ( (Mathf.Pow(2, tempLength) - 1)*multiplier > (goalMax * 1.25f)) {

			//increase the accuracy
			multiplier *= 0.9f;

			//recalculate the length
			tempLength = Mathf.CeilToInt (Mathf.Log ((goalMax/multiplier) + 1) / Mathf.Log (2));
		}

		//finally save this value
		length = tempLength;

		//add this to the cumulative total
		if (totalLength != -1) totalLength += length;
	}

	
	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * The value to be multiplied with the binary representation of the value. </summary> */
	public float Multiplier {
		get { return multiplier; }
	}

	/** <summary>
	 * The length of the binary string segment for this value. </summary> */
	public int Length {
		get { return length; }
	}

	/** <summary>
	 * The value to add to the coefficient after division. </summary> */
	public float Added_value {
		get { return added_value; }
	}

	/** <summary>
	 * The cumulative length of all existing coefficients. </summary> */
	public static int TotalLength {
		get { int tempTotal = totalLength; totalLength = -1; return tempTotal; }
		set { totalLength = value; }
	}
}
