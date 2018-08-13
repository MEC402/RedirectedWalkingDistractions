using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedupRedirect : MonoBehaviour {

    public Transform head;
    public Transform vrRig;
    public float speedupFactor;

    private Vector3 previousHeadPosition;

    void Start () {
        previousHeadPosition = head.localPosition;
    }
	
	void Update () {

        Vector3 deltaHeadPosition = head.localPosition - previousHeadPosition;
        deltaHeadPosition.y = 0;

        vrRig.position += deltaHeadPosition * (speedupFactor - 1);

        previousHeadPosition = head.localPosition;
    }
}
