using UnityEngine;
using System.Collections;

public class PooledObjectBehaviour : MonoBehaviour {
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
    }
}
