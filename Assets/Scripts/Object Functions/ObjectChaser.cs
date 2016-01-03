using UnityEngine;
using System.Collections;

public class ObjectChaser : MonoBehaviour {

    [SerializeField]
    private float trackingDelay = 0.0f;
    private float trackingTimer = 0.0f;

    [SerializeField]
    private float minDelay = 0.1f;
    [SerializeField]
    private float maxDelay = 10.0f;

    [SerializeField]
    private float trackingPrecision = 1.0f;

    [SerializeField]
    private float minPrecision = 0.1f;
    [SerializeField]
    private float maxPrecision = 10.0f;

    [SerializeField]
    private string targetTag = "";

    private GameObject[] potentialTargets;
    private GameObject target = null;

    public float Delay
    {
        get { return trackingDelay; }
        set { trackingDelay = value; }
    }

    public float Precision
    {
        get { return trackingPrecision; }
        set { trackingPrecision = value; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (trackingTimer >= trackingDelay)
        {
            if (target == null || !target.activeSelf)
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

    public void Reset()
    {
        trackingTimer = 0.0f;
        target = null;
    }

    public float GetMinDelay()
    {
        return minDelay;
    }
    public float GetMaxDelay()
    {
        return maxDelay;
    }
    public float GetMinPrecision()
    {
        return minPrecision;
    }
    public float GetMaxPrecision()
    {
        return maxPrecision;
    }
}
