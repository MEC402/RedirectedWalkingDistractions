using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour {
    [System.Serializable]
    public struct Node
    {
        public Vector3 position;
        public float bend;
        public float handleDegrees;
        public float handle1Length;
        public float handle2Length;

        public Vector3 getHandle1Position()
        {
            return position + Quaternion.Euler(0, handleDegrees, 0) * Vector3.forward * handle1Length;
        }
        public Vector3 getHandle2Position()
        {
            return position + Quaternion.Euler(0, 180+handleDegrees, 0) * Vector3.forward * handle2Length;
        }
    }

    public struct PathPoint
    {
        public Vector3 realPosition;
        public Vector3 virtualPosition;
        public float bend;
        public float distanceAlongPath;
    }
    
    public List<Node> nodes = new List<Node>();
    public List<PathPoint> pathPoints = new List<PathPoint>();
    [Range(1,100)]
    public int pathResolution;

    public void Reset()
    {
        for (int i = 0; i<3; ++i)
        {
            Node newNode = new Node();
            newNode.position.x = i - 1;
            newNode.handleDegrees = 45 * (i-1);
            newNode.handle1Length = 0.3f;
            newNode.handle2Length = 0.3f;
            nodes.Add(newNode);
        }
    }

    public void Start()
    {
        UpdatePathPoints();
    }

    public void AddNode(int index)
    {
        Node newNode = new Node();

        if (index < nodes.Count-1)
        {
            newNode.position = Vector3.Lerp(nodes[index].position, nodes[index + 1].position, 0.5f);
            newNode.bend = nodes[index].bend;
            newNode.handleDegrees = Mathf.LerpAngle(nodes[index].handleDegrees, nodes[index + 1].handleDegrees, 0.5f);
            newNode.handle1Length = nodes[index].handle2Length;
            newNode.handle2Length = nodes[index+1].handle1Length;
            nodes.Insert(index + 1, newNode);
        }
        else
        {
            newNode = nodes[index];
            newNode.position.x += 1;
            nodes.Add(newNode);
        }
    }

    public void RemoveNode(int index)
    {
        if (index >= 0 && index < nodes.Count) nodes.RemoveAt(index);
    }

    public void UpdatePathPoints()
    {
        pathPoints.Clear();

        float totalDistance = 0;
        // Generate real-space path points
        for (int i=0; i<nodes.Count-1; ++i)
        {
            float t = 0;
            while (t < 1)
            {
                PathPoint point = new PathPoint();

                // Bezier curve
                Vector3 p0 = nodes[i].position;
                Vector3 p3 = nodes[i+1].position;
                Vector3 p1 = nodes[i].getHandle2Position();
                Vector3 p2 = nodes[i + 1].getHandle1Position();
                point.realPosition = (1-t)* (1 - t) * (1 - t) * p0 + 3*(1-t)*(1-t)*t*p1 + 3*(1-t)*t*t*p2 + t*t*t*p3;

                if (pathPoints.Count > 0)
                {
                    totalDistance += (point.realPosition - pathPoints[pathPoints.Count - 1].realPosition).magnitude;
                }

                point.virtualPosition = point.realPosition;
                point.bend = nodes[i].bend;
                point.distanceAlongPath = totalDistance;
                pathPoints.Add(point);
                t += 1.0f / pathResolution;
            }
        }

        // Calculate virtual-space path points
        float rotSum = 0;
        for (int i=0; i<pathPoints.Count; ++i)
        {
            PathPoint point = pathPoints[i];

            if (i >= 2)
            {
                Vector3 offsetFromPreviousNode = pathPoints[i].realPosition - pathPoints[i - 1].realPosition;
                Vector3 a = pathPoints[i - 2].realPosition;
                Vector3 b = pathPoints[i-1].realPosition;
                Vector3 c = pathPoints[i].realPosition;
                float angleCosine = Vector3.Dot((a - b).normalized, (c - b).normalized);
                float angle = pathPoints[i-1].bend * (Mathf.PI - Mathf.Acos(angleCosine));
                rotSum += angle;
                Vector3 rotatedOffset = Quaternion.Euler(0, Mathf.Rad2Deg * rotSum, 0) * offsetFromPreviousNode;
                point.virtualPosition = rotatedOffset + pathPoints[i - 1].virtualPosition;
                pathPoints[i] = point;
            }
        }
    }
}