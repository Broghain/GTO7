using UnityEngine;
using System.Collections;

public class Magnetic : MonoBehaviour {

    [SerializeField]
    private float range = 2.0f;

    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private GameObject toObject;
    [SerializeField]
    private string toObjectWithTag;

	// Use this for initialization
	void Start () 
    {
        if (toObject == null)
        {
            if (toObjectWithTag != null)
            {
                toObject = GameObject.FindGameObjectWithTag(toObjectWithTag);
            }
            else
            {
                Debug.LogError("No gameobject found! Please set toObject or toObjectWithTag value in inspector");
                Destroy(this.gameObject);
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Vector3.Distance(toObject.transform.position, transform.position) <= range)
        {
            transform.position = Vector3.Lerp(transform.position, toObject.transform.position, Time.deltaTime * speed);
        }
	}
}
