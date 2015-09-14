using UnityEngine;
using System.Collections;

/** <summary>
 * This class pairs values with names. //TODO improve comment </summary> */
public class Coefficient {
	
	/** <summary>
	 * //TODO add comment </summary> */
	private readonly string name;
	
	/** <summary>
	 * //TODO add comment </summary> */
	private float value;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * //TODO add comment </summary> */
	public Coefficient(float value, string name) {
		this.name = name;
		this.value = value;
	}

	/** <summary>
	 * //TODO add comment </summary> */
	public Coefficient(string name) {
		this.name = name;
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * //TODO add comment </summary> */
	public float Value {
		get { return this.value; }
		set { this.value = value; }
	}
	
	/** <summary>
	 * //TODO add comment </summary> */
	public string Name {
		get { return name; }
	}
}
