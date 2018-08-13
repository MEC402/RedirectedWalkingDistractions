using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionScaler : MonoBehaviour, NodeMotionTarget {

	public float scale = 1.1f;
	public void MotionUpdate(RigNodeMotion node) {
		gameObject.transform.localPosition = node.position * (scale - 1);
	}
}
