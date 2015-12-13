using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class BomberController : EnemyBehaviour {

    private FiniteStateMachine stateMachine;

	// Use this for initialization
	void Start () 
    {
        Initialize();

        stateMachine = GetComponent<FiniteStateMachine>();
        stateMachine.InitializeStateMachine();
	}
	
	// Update is called once per frame
	void Update () 
    {
        IsOffScreen();
        stateMachine.UpdateStateMachine();
	}
}
