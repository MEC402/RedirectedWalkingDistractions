using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAroundOrigin : MonoBehaviour {

    public float degreesPerSecond;

	// Update is called once per frame
	void Update () {
        transform.RotateAround(Vector3.zero, new Vector3(0, 1, 0), degreesPerSecond * Time.fixedDeltaTime);
	}
}
