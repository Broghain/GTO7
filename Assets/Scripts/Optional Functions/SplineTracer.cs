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
    }

    // Update is called once per frame
    void Update()
    {
        if (speed > 0)
        {
            float moveTime = (10 / (speed + boost)) * Vector3.Distance(track.GetPoint(t), track.GetPoint(t + Time.deltaTime));
            Vector3 nextPosition;

            t += Time.deltaTime / moveTime;
            if (t >= 1)
            {
                t = 0;
            }
            nextPosition = track.GetPoint(t + Time.deltaTime / moveTime);

            Vector3 position = track.GetPoint(t);
            transform.position = position;

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
}
