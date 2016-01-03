using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileController : PooledObjectBehaviour {

    //attributes
    [SerializeField]
    private float moveSpeed = 1.0f;
    [SerializeField]
    private float baseDamage = 10.0f;
    private float damageIncrease = 0.0f;

    [SerializeField]
    private float minMoveSpeed = 1.0f;
    [SerializeField]
    private float maxMoveSpeed = 10.0f;
    [SerializeField]
    private float minDamage = 1.0f;
    [SerializeField]
    private float maxDamage = 100.0f;

    //sound effects
    [SerializeField]
    private AudioClip[] shotSounds;
    [SerializeField]
    private AudioClip[] hitSounds;

    //screen edge
    private Vector3 screenUpperLeft;
    private Vector3 screenLowerRight;

    //optional functionality
    private TimeToLive ttl;
    private ObjectChaser tracker;
    private Collider trigger;

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float Damage
    {
        get { return baseDamage + damageIncrease; }
        set { damageIncrease = value - baseDamage; }
    }

	// Use this for initialization
	void Start () {
        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        ttl = GetComponent<TimeToLive>();
        tracker = GetComponent<ObjectChaser>();
        trigger = GetComponent<Collider>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 nextPosition = transform.position + (transform.up * (Time.deltaTime * moveSpeed));
        transform.position = new Vector3(nextPosition.x, nextPosition.y, 0);

        if (ttl != null)
        {
            if (ttl.GetTimer() <= 0)
            {
                Disable();
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

    public void Disable()
    {
        if (trigger.enabled)
        {
            ParticleManager.instance.SpawnHitParticle(transform.position);
        }
        else
        {
            trigger.enabled = true;
        }
        
        DisableInPool();
        
        if (ttl != null) 
        {
            ttl.Reset();
        }

        if (tracker != null)
        {
            tracker.Reset();
        }
    }

    public float GetMinSpeed()
    {
        return minMoveSpeed;
    }
    public float GetMaxSpeed()
    {
        return maxMoveSpeed;
    }
    public float GetMinDmg()
    {
        return minDamage;
    }
    public float GetMaxDmg()
    {
        return maxDamage;
    }

    public AudioClip GetShotClip()
    {
        return shotSounds[Random.Range(0, shotSounds.Length)];
    }
    public AudioClip GetHitClip()
    {
        return hitSounds[Random.Range(0, hitSounds.Length)];
    }
}
