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

    public void DestroyPickup(bool pickedUp)
    {
        DifficultyManager.instance.SetPickupPct(pickedUp);
        Destroy(this.gameObject);
    }
}
