using UnityEngine;
using System.Collections;

/** */
public class HillClimbing : Algorithm {

	/** */
	private GameObject flock;

	/** */
	private int NUM_BOIDS = 30;

	/** */
	private GAGenome genome;

	/*-------------------------------------------------------------------------------------------------------------*/

	/** */
	public HillClimbing(string name) : base(name) {


	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary> Called to start running the algorithm in the scene. Implementation is 
	 * specific to the child type. </summary> */
	override public void Initiate_algorithm() {

		genome = new GAGenome();

		//create a new flock
		flock = base.Create_flock(Prefab.FLOCK_PREFAB, new HCVectorSet(genome), NUM_BOIDS);

		//notify all listeners
		Notify ();
	}
	
	/** <summary> Called to stop running the algorithm in the scene. </summary> */
	override public void Stop_algorithm () {
			
		//perform general stopping procedures
		base.Default_stop ();
		
		//kill off the flock
		Stop_current_flock();
	}

	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** */
	private void Stop_current_flock() {
		if (flock != null)	flock.GetComponent<Flock>().End_life();
	}

	/** <summary> Returns the GAGenome of the flock currently active. </summary> */
	public GAGenome Get_current_genome() {
		return genome;
	}
}
