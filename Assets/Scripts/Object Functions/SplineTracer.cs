using UnityEngine;
using System.Collections;

public class SplineTracer : MonoBehaviour
{

    [SerializeField]
    private BezierSpline track;

    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private bool rotateWithSpline = false;

    [SerializeField]
    private float minOffset = -0.5f;
    [SerializeField]
    private float maxOffset = 0.5f;
    private float offset;

    [SerializeField]
    private bool loop;

    private float boost = 0.0f;

    private float t;

    // Use this for initialization
    void Start()
    {
        if (track == null)
        {
            BezierSpline[] tracks = GameObject.FindObjectsOfType<BezierSpline>();
            track = tracks[Random.Range(0, tracks.Length)];
        }
        t = 0.0f;
        transform.position = track.GetPoint(t);
        offset = Random.Range(minOffset, maxOffset);
    }

    // Update is called once per frame
    void Update()
    {
        if (speed > 0 && !GameManager.instance.GetPaused())
        {
            float moveTime = (10 / (speed + boost)) * Vector3.Distance(track.GetPoint(t), track.GetPoint(t + Time.deltaTime));
            if (moveTime == 0)
            {
                moveTime = 0.001f; //prevent divide by zero
            }
            Vector3 nextPosition;

            t += Time.deltaTime / moveTime;
            if (t >= 1)
            {
                if (loop)
                {
                    t = 0;
                }
                else
                {
                    PooledObjectBehaviour poolObj = GetComponent<PooledObjectBehaviour>();
                    if (poolObj != null)
                    {
                        poolObj.DisableInPool();
                        RemoveFromTrack();
                    }
                    else
                    {
                        Destroy(this.gameObject);
                        RemoveFromTrack();
                    }
                }
                
            }
            nextPosition = track.GetPoint(t + Time.deltaTime / moveTime);
            nextPosition += (transform.right * offset);

            Vector3 position = track.GetPoint(t);
            transform.position = position + (transform.right * offset);

            if (rotateWithSpline)
            {
                transform.LookAt(nextPosition, transform.up);
            }    
        }
    }

    public BezierSpline GetTrack()
    {
        return track;
    }

    public void SetTrack(BezierSpline track, float t, bool reverse)
    {
        this.track = track;
        this.t = t;
    }

    public void RemoveFromTrack()
    {
        track.RemoveTracer(this);
    }
}
