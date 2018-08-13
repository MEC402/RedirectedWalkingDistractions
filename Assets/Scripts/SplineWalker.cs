using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineWalker : MonoBehaviour {

	public Transform head;
	public Transform vrRig;
    public Spline spline;

    private int currentEdge = 0;
    private Vector3 crossPoint = new Vector3();
    private float crossAngleChange = 0;

	void Start () {
        
	}
	
	void Update () {
        advanceEdges();

        if (currentEdge == 0) return;

        Vector3 edgeStart = spline.pathPoints[currentEdge].realPosition;
        Vector3 edgeEnd = spline.pathPoints[currentEdge + 1].realPosition;
        
        // Find closest point on edge to player position
        Vector3 pointOnEdge = closestPointOnLineSegmentToPoint(edgeStart, edgeEnd, head.localPosition);

        // Find t for how far along the edge the point is
        float t = InverseLerp(edgeStart, edgeEnd, pointOnEdge);

        // Find the two separating planes for this edge
        Vector3 ra = spline.pathPoints[currentEdge-1].realPosition;
        Vector3 rb = spline.pathPoints[currentEdge].realPosition;
        Vector3 rc = spline.pathPoints[currentEdge + 1].realPosition;
        Vector3 rd = spline.pathPoints[currentEdge + 2].realPosition;
        Vector3 previousRealEdgeDirection = (rb - ra).normalized;
        Vector3 currentRealEdgeDirection = (rc - rb).normalized;
        Vector3 nextRealEdgeDirection = (rd - rc).normalized;
        Vector3 realDividingPlaneNormal1 = Vector3.Lerp(previousRealEdgeDirection, currentRealEdgeDirection, 0.5f).normalized;
        Vector3 realDividingPlaneNormal2 = Vector3.Lerp(currentRealEdgeDirection, nextRealEdgeDirection, 0.5f).normalized;

        Vector3 va = spline.pathPoints[currentEdge - 1].virtualPosition;
        Vector3 vb = spline.pathPoints[currentEdge].virtualPosition;
        Vector3 vc = spline.pathPoints[currentEdge + 1].virtualPosition;
        Vector3 vd = spline.pathPoints[currentEdge + 2].virtualPosition;
        Vector3 previousVirtualEdgeDirection = (vb - va).normalized;
        Vector3 currentVirtualEdgeDirection = (vc - vb).normalized;
        Vector3 nextVirtualEdgeDirection = (vd - vc).normalized;
        Vector3 virtualDividingPlaneNormal1 = Vector3.Lerp(previousVirtualEdgeDirection, currentVirtualEdgeDirection, 0.5f).normalized;
        Vector3 virtualDividingPlaneNormal2 = Vector3.Lerp(currentVirtualEdgeDirection, nextVirtualEdgeDirection, 0.5f).normalized;

        // Lerp between the two plane's normals by t
        Vector3 realEdgeDirection = Vector3.Lerp(realDividingPlaneNormal1, realDividingPlaneNormal2, t).normalized;
        Vector3 virtualEdgeDirection = Vector3.Lerp(virtualDividingPlaneNormal1, virtualDividingPlaneNormal2, t).normalized;
        float angleChange = Vector3.SignedAngle(realEdgeDirection, virtualEdgeDirection, Vector3.up);

        Vector3 displacementFromEdgeStart = head.localPosition - spline.pathPoints[currentEdge].realPosition;
        Vector3 redirectedPosition = spline.pathPoints[currentEdge].virtualPosition + displacementFromEdgeStart;

        vrRig.position = redirectedPosition - head.localPosition;
        vrRig.rotation = Quaternion.identity;
        vrRig.RotateAround(spline.pathPoints[currentEdge].virtualPosition, Vector3.up, angleChange);

        print("Edge: " + currentEdge + ", angle change: " + angleChange + ", t:" + t);
    }

    private void advanceEdges()
    {
        while (true)
        {
            // Stop if there's not another edge after this one
            if (currentEdge >= spline.pathPoints.Count-2)
            {
                return;
            }

            Vector2 a = v3to2(spline.pathPoints[currentEdge].realPosition);
            Vector2 b = v3to2(spline.pathPoints[currentEdge + 1].realPosition);
            Vector2 c = v3to2(spline.pathPoints[currentEdge + 2].realPosition);
            Vector2 currentEdgeDirection = (b - a).normalized;
            Vector2 nextEdgeDirection = (c - b).normalized;
            Vector2 dividingPlaneNormal = Vector2.Lerp(currentEdgeDirection, nextEdgeDirection, 0.5f).normalized;
            Vector2 middlePointToPlayerDirection = (v3to2(head.localPosition) - b).normalized;
            if (Vector2.Dot(middlePointToPlayerDirection, dividingPlaneNormal) > 0)
            {
                print("a to b:" + (b - a) + ", mid:" + b + ", player:" + head.localPosition + ", mid to player:" + (b - v3to2(head.localPosition)) + ", dot:" + Vector2.Dot(middlePointToPlayerDirection, dividingPlaneNormal));
                crossAngleChange = calculateLerpedAngleChange();
                crossPoint = head.localPosition;
                ++currentEdge;
            }
            else
            {
                return;
            }
        }
    }

    private float calculateAngleChange()
    {
        Vector3 realEdgeDirection = (spline.pathPoints[currentEdge + 1].realPosition - spline.pathPoints[currentEdge].realPosition).normalized;
        Vector3 virtualEdgeDirection = (spline.pathPoints[currentEdge + 1].virtualPosition - spline.pathPoints[currentEdge].virtualPosition).normalized;
        float angleChange = Vector3.SignedAngle(realEdgeDirection, virtualEdgeDirection, Vector3.up);
        return angleChange;
    }

    private float calculateLerpedAngleChange()
    {
        Vector3 displacementFromCrossPoint = head.localPosition - crossPoint;
        float normalizedDistanceFromCrossPoint = displacementFromCrossPoint.magnitude / ((spline.pathPoints[currentEdge + 1].realPosition - crossPoint).magnitude*0.2f);
        float currentEdgeAngleChange = calculateAngleChange();
        float smoothAngleChange = Mathf.LerpAngle(crossAngleChange, currentEdgeAngleChange, normalizedDistanceFromCrossPoint);
        return smoothAngleChange;
    }

    private Vector3 closestPointOnLineSegmentToPoint(Vector3 start, Vector3 end, Vector3 pnt)
    {
        var line = (end - start);
        var len = line.magnitude;
        line.Normalize();

        var v = pnt - start;
        var d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return start + line * d;
    }

    public float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    private Vector2 v3to2(Vector3 src)
    {
        return new Vector2(src.x, src.z);
    }
}
