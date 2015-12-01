using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum BezierControlPointMode
{
    Free,
    Aligned,
    Mirrored
}

public class BezierSpline : MonoBehaviour
{

    //source: http://catlikecoding.com/unity/tutorials/curves-and-splines/

    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    private BezierControlPointMode[] modes;

    [SerializeField]
    private int smoothness;

    private bool loop;

    public int GetSmoothness()
    {
        if (smoothness > 500)
        {
            return 500;
        }
        else if (smoothness < 10)
        {
            return 10;
        }
        return smoothness;
    }

    public bool GetLoop()
    {
        return loop;
    }

    public void SetLoop(bool value)
    {
        loop = value;
        if (value == true)
        {
            modes[modes.Length - 1] = modes[0];
            SetControlPoint(0, points[0]);
        }
    }

    public int GetControlPointCount()
    {
        return points.Length;
    }

    public void Reset()
    {
        points = new Vector3[] {
			new Vector3(0f, 0f, 0f),
			new Vector3(0f, 0f, 5f),
			new Vector3(0f, 0f, 10f),
			new Vector3(0f, 0f, 15f)
		};

        modes = new BezierControlPointMode[] {
			BezierControlPointMode.Free,
			BezierControlPointMode.Free
		};
    }

    //Get point in spline 
    public Vector3 GetPoint(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * GetCurveCount();
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * GetCurveCount();
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(
            Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public void AddCurve()
    {
        //point = last point's position
        Vector3 point = points[points.Length - 1];

        //Resize array to add 3 points
        Array.Resize(ref points, points.Length + 3);

        //Next point's position.z + 5 for each new point
        point.z += 5f;
        points[points.Length - 3] = point;
        point.z += 5f;
        points[points.Length - 2] = point;
        point.z += 5f;
        points[points.Length - 1] = point;

        //Increase size of forcemode array (one for each curve)
        Array.Resize(ref modes, modes.Length + 1);
        //New mode is same as last mode
        modes[modes.Length - 1] = modes[modes.Length - 2];

        EnforceMode(points.Length - 4);

        if (loop) //If spline loops
        {
            points[points.Length - 1] = points[0]; //Last point's position equals first point's position
            modes[modes.Length - 1] = modes[0]; //Last mode equals first mode
            EnforceMode(0);
        }
    }

    public void RemoveCurve()
    {
        if (points.Length > 4)
        {
            Array.Resize(ref points, points.Length - 3);

            Array.Resize(ref modes, modes.Length - 1);
        }
        else
        {
            Reset();
        }
    }

    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }

    //Set control point points[index] at position point
    public void SetControlPoint(int index, Vector3 point)
    {
        if (index % 3 == 0) //If index is multiple of 3 (end of a curve)
        {
            Vector3 delta = point - points[index]; //Delta position is new position - current position
            if (loop) //If this point loops back to start of spline
            {
                if (index == 0) //If point is the first point
                {
                    points[1] += delta; //Second point's position is position + delta position
                    points[points.Length - 2] += delta; //Second to last point's position is position + delta position
                    points[points.Length - 1] = point; //last point's position is position + delta position
                }
                else if (index == points.Length - 1) //If point is the last point
                {
                    points[0] = point; //new position is position of first point
                    points[1] += delta; //Second point's position is position + delta position
                    points[index - 1] += delta; //Second to last point's position is position + delta position
                }
                else
                {
                    points[index - 1] += delta; //Previous point's position is position + delta position
                    points[index + 1] += delta; //Next point's position is position + delta position
                }
            }
            else //Doesn't loop
            {
                if (index > 0) //If point is not first point
                {
                    points[index - 1] += delta; //Previous point's position is position + delta position
                }
                if (index + 1 < points.Length) //If point is not last point
                {
                    points[index + 1] += delta;//Next point's position is position + delta position
                }
            }
        }
        points[index] = point; //Point position is new position
        EnforceMode(index);
    }

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }

    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if (loop)
        {
            if (modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if (modeIndex == modes.Length)
            {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }

    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
        {
            return;
        }

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if (fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if (enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if (fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if (enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }

        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;
    }

    public int GetCurveCount()
    {
        return (points.Length - 1) / 3;
    }
}
