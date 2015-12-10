using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class FighterController : EnemyBehaviour {

    private FiniteStateMachine stateMachine;

	// Use this for initialization
	void Start () {
        stateMachine = GetComponent<FiniteStateMachine>();
        stateMachine.InitializeStateMachine();
        stateMachine.AddState(new FighterAttackingPlayer(transform));
        stateMachine.AddState(new FighterReachedScreenEdge(transform));
        stateMachine.AddState(new FighterDead(transform));

        Initialize();
        transform.rotation = Quaternion.LookRotation(GameManager.instance.GetPlayer().position - transform.position, new Vector3(0, 0, -1));
	}
	
	// Update is called once per frame
	void Update () 
    {
        IsOffScreen();
        stateMachine.SetValue("OffScreen", isOffScreen);
        stateMachine.SetValue("Health", health);
        stateMachine.UpdateStateMachine();
	}
}
