using UnityEngine;
using System.Collections;

/** <summary>
 * //TODO add comment </summary> */
public class Normal : Algorithm {
	
	/** <summary>
	 * The number of boids initially generated in a flock. </summary> */
	private const int NUM_BOIDS = 20;

	/** <summary>
	 * The current flock being run. </summary> */
	private GameObject flock;

	/** <summary>
	 * The VectorSet controlling the flight of the flock. </summary> */
	private NormalVectorSet vectorSet = new NormalVectorSet (1.5f, 1.5f, 1.5f);

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Constructor that uses default values. </summary>
	 * <param name="name"> The name of the algorithm </param> */
	public Normal(string name) : base(name) { }

	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Sets up the scene to begin running the algorithm. </summary> */
	override public void Initiate_algorithm() {

		//add the default display to the GUI
		base.AddDisplay("NormalDisplay", true);
		Notify ();

		//create the flock
		Start_new_flock ();
	}
	
	/** <summary>
	 * Stops the algorithm and ends the life of the current flock. </summary>  */
	override public void Stop_algorithm() {
		
		//perform general stopping procedures
		base.Default_stop ();
		
		//kill off the flock
		Stop_current_flock();
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Creates a new flock, instantiating it, populating it, changes the camera target, and assigns a <see cref="VectorSet"/>. 
	 * Most of this is done outside the class. </summary> */
	private void Start_new_flock() {

		//create a new flock. base takes care of everything
		flock = base.Create_flock(Prefab.FLOCK_PREFAB, vectorSet, NUM_BOIDS);
	}
	
	/** <summary>
	 * Ends the life of the current flock. </summary> */
	private void Stop_current_flock() {
		if (flock != null)	flock.GetComponent<Flock>().End_life();
	}

	/** <summary>
	 * Resets the algorithm back to its original state by destroying the current flock and
	 * creating a new one with the default coefficient values. </summary> */
	public void Reset() {

		//kill the flock
		Stop_current_flock ();

		//reset the coefficient values
		vectorSet.SeparationCoefficient = 1.5f;
		vectorSet.CohesionCoefficient = 1.5f;
		vectorSet.FollowCoefficient = 1.5f;

		//make a new flock
		Start_new_flock ();
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * The coefficients controlling the flight of the flock </summary> */
	public Coefficient[] Coefficients {
		get { return vectorSet.Coefficients; }
	}
}
