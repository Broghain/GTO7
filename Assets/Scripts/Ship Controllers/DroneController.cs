using UnityEngine;
using System.Collections;

public class DroneController : EnemyBehaviour {

	// Use this for initialization
	void Start () 
    {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () 
    {
        IsOffScreen();
	}
}
