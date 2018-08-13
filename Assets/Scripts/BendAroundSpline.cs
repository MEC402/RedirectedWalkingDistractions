using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendAroundSpline : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        Spline spline = FindObjectOfType<Spline>();
        if (spline == null) return;

        if (spline.pathPoints.Count == 0) spline.UpdatePathPoints();
        if (spline.pathPoints.Count == 0) return;

        float distance = transform.position.x;
        float widthOffset = transform.position.z;
        for (int i = 0; i < spline.pathPoints.Count - 1; ++i)
        {
            if (distance < spline.pathPoints[i].distanceAlongPath)
            {
                Vector3 pathForward = new Vector3(1, 0, 0);
                Vector3 offsetFromPoint = transform.position - pathForward * spline.pathPoints[i].distanceAlongPath;
                Vector3 edge = spline.pathPoints[i + 1].virtualPosition - spline.pathPoints[i].virtualPosition;
                float edgeRotation = Vector3.SignedAngle(pathForward, edge, Vector3.up);
                Vector3 bentPosition = spline.pathPoints[i].virtualPosition + Quaternion.Euler(0, edgeRotation, 0) * offsetFromPoint;
                transform.position = bentPosition;
                transform.rotation = Quaternion.Euler(0, edgeRotation, 0) * transform.rotation;
                print(i);
                return;
            }
        }
    }
}
