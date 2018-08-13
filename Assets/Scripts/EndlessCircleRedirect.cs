using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessCircleRedirect : MonoBehaviour {

    public Transform head;
    public float radius;
    public float visibleRange;

    // private float distanceTraveled = 0;
    private int turns = 0;
    private float previousYAngle;
    // private Vector3 previousHeadPosition;

	// Use this for initialization
	void Start () {
        // previousHeadPosition = head.position;
	}
	
	// Update is called once per frame
	void Update () {

        // Get movement tangent to circle
        //Vector3 headMovementDelta = head.position - previousHeadPosition;
        //Vector2 headMovementDelta2D = new Vector2(headMovementDelta.x, headMovementDelta.z);
        //
        //Vector3 rigCenterToHead = head.localPosition;
        //Vector2 rigCenterToHead2D = new Vector2(rigCenterToHead.x, rigCenterToHead.z).normalized;
        //Vector2 circleTangent = new Vector2(rigCenterToHead2D.y, -rigCenterToHead2D.x);
        //float forwardMovement = headMovementDelta2D.magnitude * Vector2.Dot(headMovementDelta2D.normalized, circleTangent);

        float yAngle = Mathf.Atan2(head.position.z, -head.position.x);
        // float previousYAngle = Mathf.Atan2(previousHeadPosition.x, -previousHeadPosition.z);
        float totalAngle = previousYAngle + 2 * Mathf.PI * turns;
        float circumferenceTraveled = totalAngle * radius;
        print(totalAngle);

        // float previousCircumferenceTraveled = previousYAngle * radius;
        // float deltaCircumferenceTraveled = circumferenceTraveled - previousCircumferenceTraveled;

        // distanceTraveled += deltaCircumferenceTraveled;
        //print(distanceTraveled);

        BendAroundCircle[] pathObjects = FindObjectsOfType<BendAroundCircle>();
        foreach (BendAroundCircle obj in pathObjects)
        {
            // obj.UpdateVisibility(distanceTraveled, visibleRange);
            obj.UpdateVisibility(circumferenceTraveled, visibleRange);
        }

        // previousHeadPosition = head.localPosition;
        if(previousYAngle - yAngle > Mathf.PI) {
            turns++;
        } else if(yAngle - previousYAngle > Mathf.PI)
        {
            turns--;
        }
        previousYAngle = yAngle;
	}
}
