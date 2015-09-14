using UnityEngine;
using System.Collections;

/** <summary> 
 * This class holds all of the prefabs used in the program. </summary> */
public static class Prefab {
	
	/** <summary>
	 * The prefab for boid objects (see <see cref="Boid"/>). </summary> */
	public static readonly GameObject BOID_PREFAB = (GameObject)Resources.Load("Prefabs/Boid", typeof(GameObject));
	
	/** <summary>
	 * The prefab for flock objects (see <see cref="Flock"/>). </summary> */
	public static readonly GameObject FLOCK_PREFAB = (GameObject)Resources.Load("Prefabs/Flock", typeof(GameObject));

	/** <summary> 
	 * The prefab for GA flock objects (see <see cref="GAFlock"/>). </summary> */
	public static readonly GameObject GA_FLOCK_PREFAB = (GameObject)Resources.Load("Prefabs/GAFlock", typeof(GameObject));
}
