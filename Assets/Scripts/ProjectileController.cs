using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : MonoBehaviour {

    [SerializeField]
    private bool isTracking = false;

    [SerializeField]
    private float trackingDelay = 0.0f;
    private float trackingTimer = 0.0f;

    [SerializeField]
    private float trackingPrecision = 1.0f;

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private string targetTag = "";

    private GameObject[] potentialTargets;
    private GameObject target = null;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + (transform.up * (Time.deltaTime * moveSpeed));
        if (isTracking)
        {
            if (trackingTimer >= trackingDelay)
            {
                if (target == null)
                {
                    GetClosestTarget();
                }
                else
                {
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(target.transform.position), trackingPrecision * Time.deltaTime);
                }
            }
            else
            {
                trackingTimer += Time.deltaTime;
            }
        }
	}

    private void GetClosestTarget()
    {
        potentialTargets = GameObject.FindGameObjectsWithTag(targetTag);
        float shortestDist = 999.9f;

        foreach (GameObject potentialTarget in potentialTargets)
        {
            float dist = Vector3.Distance(transform.position, potentialTarget.transform.position);
            if (dist <= shortestDist)
            {
                shortestDist = dist;
                target = potentialTarget;
            }
        }
    }
}
