using UnityEngine;
using System.Collections;

/** <summary>
 * This class is extended by classes that need to listen to the current state of an algorithm
 * in progress and receive updates from it when it changes state. </summary> */
public interface AlgorithmListener {

	/** <summary>
	 * Called by the algorithm to inform the listener that a change of state has
	 * occurred. </summary> */
	void Update_listener();

	/** <summary>
	 * The <see cref="Algorithm"/> on which to focus </summary> */
	Algorithm Subject {
		set;
	}
}
