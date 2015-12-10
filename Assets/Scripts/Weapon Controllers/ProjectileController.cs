using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : PooledObjectBehaviour {

    [SerializeField]
    private float moveSpeed = 1.0f;

    [SerializeField]
    private float damage = 10.0f;

    private Vector3 screenUpperLeft;
    private Vector3 screenLowerRight;

    private TimeToLive ttl;
    private ObjectTracker tracker;
    private Collider trigger;

	// Use this for initialization
	void Start () {
        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        ttl = GetComponent<TimeToLive>();
        tracker = GetComponent<ObjectTracker>();
        trigger = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 nextPosition = transform.position + (transform.up * (Time.deltaTime * moveSpeed));
        transform.position = new Vector3(nextPosition.x, nextPosition.y, 0);

        if (ttl != null)
        {
            if (ttl.GetTimeToLive() <= 0)
            {
                DisableBullet();
            }
        }

        if ((transform.position.x < screenUpperLeft.x || transform.position.x > screenLowerRight.x || transform.position.y > screenUpperLeft.y || transform.position.y < screenLowerRight.y))
        {
            if (trigger.enabled == true)
            {
                trigger.enabled = false;
            }
        }
        else if (trigger.enabled == false)
        {
            trigger.enabled = true;
        }
	}

    public float GetDamage()
    {
        return damage;
    }

    public void DisableBullet()
    {
        gameObject.SetActive(false);
        gameObject.name = "Unused" + gameObject.name;
        transform.position = new Vector3(1000, 1000, 1000);
        trigger.enabled = true;
        
        if (ttl != null) 
        {
            ttl.Reset();
        }

        if (tracker != null)
        {
            tracker.Reset();
        }
    }
}
