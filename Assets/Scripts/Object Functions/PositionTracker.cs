using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionTracker : MonoBehaviour {

    //tracking interval
    [SerializeField]
    private float trackingInterval = 0.5f;
    private float intervalTimer = 0;

    private Vector3[] trackedPositions;
    private int trackingIndex = 0;

    private Vector3 averagePosition = Vector3.zero;
    private float positionVariation = 0;

	// Use this for initialization
	void Start () 
    {
        trackedPositions = new Vector3[64];
        for (int i = 0; i < trackedPositions.Length; i++)
        {
            trackedPositions[i] = transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        intervalTimer += Time.deltaTime;
        if (intervalTimer >= trackingInterval)
        {
            trackedPositions[trackingIndex] = transform.position;
            trackingIndex++;
            if(trackingIndex >= trackedPositions.Length){
                trackingIndex = 0;
            }
            intervalTimer = 0;
        }
	}

    public Vector3 GetAveragePosition()
    {
        Vector3 summedPositions = Vector3.zero;
        foreach (Vector3 position in trackedPositions)
        {
            summedPositions += position;
        }
        averagePosition = new Vector3(summedPositions.x / trackedPositions.Length, summedPositions.y / trackedPositions.Length, summedPositions.z / trackedPositions.Length);
        
        return averagePosition;
    }

    public float GetDistanceMoved()
    {
        float distance = 0;
        Vector3 lastPosition = trackedPositions[0];
        foreach (Vector3 position in trackedPositions)
        {
            distance += Vector3.Distance(lastPosition, position);
        }
        return distance;
    }
}
