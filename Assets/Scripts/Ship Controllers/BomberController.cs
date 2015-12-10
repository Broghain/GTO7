using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class BomberController : EnemyBehaviour {

    FiniteStateMachine stateMachine;

	// Use this for initialization
	void Start () {
        stateMachine = GetComponent<FiniteStateMachine>();
        stateMachine.InitializeStateMachine();
        Initialize();
	}
	
	// Update is called once per frame
	void Update () 
    {
        IsOffScreen();
	}
}
