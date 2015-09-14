using UnityEngine;
using System.Collections;

/** <summary>
 * This class pairs values with names for calcuting vector offsets. </summary> */
public class Coefficient {
	
	/** <summary>
	 * The name of the coefficient. </summary> */
	private readonly string name;
	
	/** <summary>
	 * The coefficient's value. </summary> */
	private float value;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Primary constructor. </summary> 
	 * <param name="value"> The coefficient's value </param> 
	 * <param name="name"> The coefficient's name </param> */
	public Coefficient(float value, string name) {
		this.name = name;
		this.value = value;
	}

	/** <summary>
	 * Constructor used when the value is not yet known. </summary> 
	 * <param name="name"> The coefficient's name </param> */
	public Coefficient(string name) {
		this.name = name;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * The coefficient's value. </summary> */
	public float Value {
		get { return this.value; }
		set { this.value = value; }
	}
	
	/** <summary>
	 * The coefficient's name. </summary> */
	public string Name {
		get { return name; }
	}
}
