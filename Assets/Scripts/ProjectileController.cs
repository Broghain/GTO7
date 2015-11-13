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

    [SerializeField]
    private float timeToLive = 1.0f;
    private float timer;

    [SerializeField]
    private bool useTTL = true;

    private Vector3 screenUpperLeft;
    private Vector3 screenLowerRight;

    private Transform parent;

    private GameObject[] potentialTargets;
    private GameObject target = null;

	// Use this for initialization
	void Start () {
        timer = timeToLive;
        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
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

        if (useTTL)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DisableBullet();
            }
        }

        if (transform.position.x < screenUpperLeft.x || transform.position.x > screenLowerRight.x || transform.position.y > screenUpperLeft.y || transform.position.y < screenLowerRight.y)
        {
            //DisableBullet();
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

    private void DisableBullet()
    {
        gameObject.SetActive(false);
        gameObject.name = "Unused" + gameObject.name;
        transform.position = new Vector3(1000, 1000, 1000);
        timer = timeToLive;
        transform.parent = parent;

        //TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        //if (trail != null)
        //{
            //trail.time = 0;
        //}
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
        transform.parent = parent;
    }
}
