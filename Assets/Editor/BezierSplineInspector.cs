using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineInspector : Editor
{
    //source: http://catlikecoding.com/unity/tutorials/curves-and-splines/
    
    private static Color[] modeColors = {
		Color.white,
		Color.yellow,
		Color.cyan
	};

    private const float directionScale = 10f;

    private const float handleSize = 0.04f;
    private const float pickSize = 0.06f;

    private int selectedIndex = -1;

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;

    private void OnSceneGUI()
    {
        spline = target as BezierSpline;
        handleTransform = spline.transform;
        handleRotation = Tools.pivotRotation == PivotRotation.Local ?
            handleTransform.rotation : Quaternion.identity;

        Vector3 p0 = ShowPoint(0);
        for (int i = 1; i < spline.GetControlPointCount(); i += 3)
        {
            Vector3 p1 = ShowPoint(i);
            Vector3 p2 = ShowPoint(i + 1);
            Vector3 p3 = ShowPoint(i + 2);

            Handles.color = Color.gray;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p2, p3);

            Handles.color = Color.white;
            Vector3 lineStart = spline.GetPoint(i);
            for (int step = 0; step <= spline.GetSmoothness(); step++)
            {
                Vector3 lineEnd = spline.GetPoint(step / (float)spline.GetSmoothness());
                Handles.DrawLine(lineStart, lineEnd);
                lineStart = lineEnd;
            }
            //Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 5f);
            p0 = p3;
        }

        //ShowDirections();

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        spline = target as BezierSpline;
        
        EditorGUI.BeginChangeCheck();
        bool loop = EditorGUILayout.Toggle("Loop Last Point", spline.GetLoop());    
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Loop Last Point");
            EditorUtility.SetDirty(spline);
            spline.SetLoop(loop);
        }
        
        if (selectedIndex >= 0 && selectedIndex < spline.GetControlPointCount())
        {
            DrawSelectedPointInspector();
        }
        
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
        
        if (GUILayout.Button("Remove Curve"))
        {
            Undo.RecordObject(spline, "Remove Curve");
            spline.RemoveCurve();
            EditorUtility.SetDirty(spline);
        }
    }

    private void DrawSelectedPointInspector()
    {
        GUILayout.Label("Selected Point");
        EditorGUI.BeginChangeCheck();
        Vector3 point = EditorGUILayout.Vector3Field("Position", spline.GetControlPoint(selectedIndex));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.SetControlPoint(selectedIndex, point);
        }
    }

    private void ShowDirections()
    {
        Handles.color = Color.green;
        Vector3 point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetDirection(0f) * directionScale);
        int steps = spline.GetSmoothness();
        for (int i = 1; i <= steps; i++)
        {
            point = spline.GetPoint(i / (float)steps);
            Handles.DrawLine(point, point + spline.GetDirection(i / (float)steps) * directionScale);
        }
    }

    //Returns position of the control point
    private Vector3 ShowPoint(int index)
    {
        //Position of spline's control point at index in array of control points
        Vector3 point = handleTransform.TransformPoint(spline.GetControlPoint(index));
        
        //Set the size of control point
        float size = HandleUtility.GetHandleSize(point);
        if (index == 0)
        {
            size *= 2f; //Increase size of first control point (useful when spline is looping)
        }

        //Change color based on controlmode (none, aligned, mirrored)
        Handles.color = modeColors[(int)spline.GetControlPointMode(index)];
        if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotCap))
        {
            selectedIndex = index;
            Repaint();
        }

        if (selectedIndex == index)
        {
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, handleRotation); //Show position tool at controlpoint
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(spline, "Move Point");
                EditorUtility.SetDirty(spline);
                spline.SetControlPoint(index, handleTransform.InverseTransformPoint(point)); //Set controlpoint at new position
            }
        }
        return point;
    }
}
