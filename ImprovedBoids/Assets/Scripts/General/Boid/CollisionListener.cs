using UnityEngine;
using System.Collections;

/** <summary>
 * This class is implemented by classes that are interested in collisions taking place. </summary> */
public interface CollisionListener {

	/** <summary>
	 * Called upon collision to notify the listener that a collision has occured. </summary> */
	void Notify_collision();
}
