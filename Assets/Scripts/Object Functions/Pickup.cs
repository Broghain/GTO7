using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour 
{
    private float value = 10.0f;

    public void SetValue(float value)
    {
        this.value = value;
    }

    public float GetValue()
    {
        return value;
    }
}
