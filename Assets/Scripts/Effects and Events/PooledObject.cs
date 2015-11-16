using UnityEngine;
using System.Collections;

public class PooledObject : MonoBehaviour {

    private Transform parent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetParent(Transform parent)
    {
        this.parent = parent;
        transform.parent = parent;
    }
}
