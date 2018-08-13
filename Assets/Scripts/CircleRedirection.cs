using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRedirection : MonoBehaviour {

	public Transform vrRig;
	public Transform head;
	public Transform target;
	public Transform gameEnvironment;
	public float circleRadius;

	private Vector3 previousHeadPosition = new Vector3();

	// Use this for initialization
	void Start () {
		gameEnvironment.position = new Vector3(0, 0, circleRadius);
	}
	
	// Update is called once per frame
	void Update () {
		// Get movement in forward direction
		// Vector3 headToTarget3D = target.position - head.position;
		Vector3 headToTarget3D = target.position - head.position;
		Vector2 headToTarget = new Vector2(headToTarget3D.x, headToTarget3D.z);
		Vector2 targetDirection = headToTarget.normalized;

		// Rotate camera rig proportional to forward movement
		Vector3 forwardMovement3D = head.position - previousHeadPosition;
		Vector2 forwardMovement = new Vector2(forwardMovement3D.x, forwardMovement3D.z);
		float movementTowardTarget = Vector2.Dot(forwardMovement, targetDirection);

		float turnRadians = movementTowardTarget / circleRadius;

		if(head.position != previousHeadPosition) {
			vrRig.RotateAround(head.position, Vector3.up, -Mathf.Rad2Deg*turnRadians);
		}

		previousHeadPosition = head.position;
	}
}
