using UnityEngine;
using System.Collections;

/** <summary>
 * This class is used to alter the velocity of boids. </summary> */
public abstract class VectorSet {
	
	/** <summary>
	 * The flock that the <see cref="VectorSet"/> is assigned to. </summary> */
	protected GameObject flock;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Finds the offset for the given boid. </summary> 
	 * <param name="boid"> The boid the offset is being calculated for </param>
	 * <param name="boids"> The boids important to the calculation </param> 
	 * <returns> The offset </returns> */
	public abstract Vector3 Get_vector(GameObject boid, ArrayList boids);
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Associate the passed flock with the <see cref="VectorSet"/> object. </summary>
	 * <param name="flock"> The flock for which to find vectors </param>
	 * <returns> Whether the operation was successful </returns> */
	public bool Set_flock(GameObject flock) {

		if (flock == null) return false;

		this.flock = flock;
		return true;
	}
}
