using UnityEngine;
using System.Collections;

public class TimeToLive : MonoBehaviour {

    [SerializeField]
    private float timeToLive = 1.0f;
    private float timer;

	// Use this for initialization
	void Start () {
        timer = timeToLive;
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
	}

    public float GetTimeToLive()
    {
        return timer;
    }

    public void Reset()
    {
        timer = timeToLive;
    }
}
