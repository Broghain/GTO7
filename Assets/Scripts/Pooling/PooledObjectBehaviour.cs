using UnityEngine;
using System.Collections;

public class PooledObjectBehaviour : MonoBehaviour {
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }

    public void DisableInPool()
    {
        gameObject.SetActive(false);
        gameObject.name = "Unused" + gameObject.name;
        transform.position = new Vector3(1000, 1000, 1000);
    }
}
