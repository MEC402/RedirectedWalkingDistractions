using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spline))]
public class SpineInspector : Editor
{
    private const float nodeSize = 0.04f;
    private const float pickSize = 0.06f;
    private int selectedNodeIndex = -1;

    private void OnSceneGUI()
    {
        Spline spline = target as Spline;

        if (spline.nodes.Count < 2) return;

        Transform splineTransform = spline.transform;
        Quaternion splineRotation = (Tools.pivotRotation == PivotRotation.Local) ? splineTransform.rotation : Quaternion.identity;

        // Draw lines between nodes
        Handles.color = Color.white;
        for (int i=1; i<spline.pathPoints.Count; ++i)
        {
            Vector3 p0 = spline.pathPoints[i-1].realPosition;
            Vector3 p1 = spline.pathPoints[i].realPosition;
            Handles.DrawLine(p0, p1);
        }

        // Draw lines between redirected nodes
        Handles.color = Color.blue;
        spline.UpdatePathPoints();
        for (int i = 1; i < spline.pathPoints.Count; ++i)
        {
            Vector3 p0 = spline.pathPoints[i - 1].virtualPosition;
            Vector3 p1 = spline.pathPoints[i].virtualPosition;
            Handles.DrawLine(p0, p1);
        }

        // Draw node points
        Handles.color = Color.white;
        for (int i = 0; i < spline.nodes.Count; ++i)
        {
            Spline.Node node = spline.nodes[i];
            Vector3 point = node.position;
            if (Handles.Button(point, splineRotation, nodeSize, pickSize, Handles.DotCap))
            {
                selectedNodeIndex = i;
                EditorUtility.SetDirty(spline);
            }
            if (selectedNodeIndex == i)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, splineRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spline, "Move Point");
                    EditorUtility.SetDirty(spline);
                    node.position = point;
                }

                // Handle 1
                EditorGUI.BeginChangeCheck();
                Vector3 handle1Position = node.getHandle1Position();
                handle1Position = Handles.DoPositionHandle(handle1Position, splineRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spline, "Move Handle");
                    EditorUtility.SetDirty(spline);
                    node.handle1Length = (handle1Position - point).magnitude;
                    node.handleDegrees = Vector3.SignedAngle(Vector3.forward, handle1Position - point, Vector3.up);
                }
                // Handle 2
                EditorGUI.BeginChangeCheck();
                Vector3 handle2Position = node.getHandle2Position();
                handle2Position = Handles.DoPositionHandle(handle2Position, splineRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(spline, "Move Handle");
                    EditorUtility.SetDirty(spline);
                    node.handle2Length = (handle2Position - point).magnitude;
                    node.handleDegrees = 180+Vector3.SignedAngle(Vector3.forward, handle2Position - point, Vector3.up);
                }

                spline.nodes[i] = node;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Spline spline = target as Spline;
        if (GUILayout.Button("Add Node"))
        {
            Undo.RecordObject(spline, "Add Node");
            spline.AddNode(selectedNodeIndex);
            EditorUtility.SetDirty(spline);
        }

        if (GUILayout.Button("Remove Node"))
        {
            Undo.RecordObject(spline, "Remove Node");
            spline.RemoveNode(selectedNodeIndex);
            EditorUtility.SetDirty(spline);
        }

        if (selectedNodeIndex >= 0 && selectedNodeIndex < spline.nodes.Count) {
            Spline.Node node = spline.nodes[selectedNodeIndex];
            float newBend = EditorGUILayout.FloatField("Selected node bend:", node.bend);
            if (newBend != node.bend)
            {
                Undo.RecordObject(spline, "Change Node Bend");
                node.bend = newBend;
                spline.nodes[selectedNodeIndex] = node;
                spline.UpdatePathPoints();
                EditorUtility.SetDirty(spline);
            }
        }
    }
}