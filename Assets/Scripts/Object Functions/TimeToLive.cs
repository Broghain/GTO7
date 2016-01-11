using UnityEngine;
using System.Collections;

public class TimeToLive : MonoBehaviour {

    [SerializeField]
    private float timeToLive = 1.0f;
    private float timer = 0.0f;

    [SerializeField]
    private bool overrideDisableFunction = false;

    private PooledObjectBehaviour pooledBehaviour;

    public float TTL
    {
        get { return timeToLive; }
        set { timeToLive = value; }
    }

	// Use this for initialization
	void Start () {
        timer = timeToLive;
        pooledBehaviour = GetComponent<PooledObjectBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (!overrideDisableFunction)
            {
                if (pooledBehaviour != null)
                {
                    pooledBehaviour.DisableInPool();
                    Reset();
                }
                else
                {
                    Destroy(gameObject);
                }
                
            }
        }
	}

    public float GetTimer()
    {
        return timer;
    }

    public void Reset()
    {
        timer = timeToLive;
    }
}
