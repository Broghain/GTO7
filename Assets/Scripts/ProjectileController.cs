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
        Vector3 nextPosition = transform.position + (transform.up * (Time.deltaTime * moveSpeed));
        transform.position = new Vector3(nextPosition.x, nextPosition.y, 0);

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
                    Vector3 direction = (target.transform.position - transform.position).normalized;
                    transform.up = Vector3.Lerp(transform.up, direction, Time.deltaTime * trackingPrecision);
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
