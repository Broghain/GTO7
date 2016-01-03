using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TrailRenderer))]
public class TrailResetter : MonoBehaviour {

    [SerializeField]
    private TimeToLive ttl = null;

    private TrailRenderer trail;
    private float trailTime;
    private float trailDelayedTime = 0.0f;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trailTime = trail.time;
    }

    void Update()
    {
        if (trailDelayedTime < trailTime)
        {
            trailDelayedTime += Time.deltaTime;
            trail.time = trailDelayedTime;
        }

        if (ttl != null && ttl.GetTimer() <= trailTime)
        {
            trail.time = ttl.GetTimer();
        }
    }

    public void Reset()
    {
        trailDelayedTime = 0.0f;
    }
}
