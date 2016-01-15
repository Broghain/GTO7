using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class BomberController : EnemyBehaviour {

    private FiniteStateMachine stateMachine;

    private Vector3 destination;
    
    [SerializeField]
    private float attackTime;
    private float attackTimer;

    public Vector3 Destination
    {
        get { return destination; }
        set { destination = value; }
    }

    public float AttackTimer
    {
        get { return attackTimer; }
        set { attackTimer = value; }
    }

	// Use this for initialization
	void Start () 
    {
        stateMachine = GetComponent<FiniteStateMachine>();
        stateMachine.InitializeStateMachine();
        stateMachine.AddState(new BomberReachedScreenEdge(this.transform));
        stateMachine.AddState(new BomberAttackingPlayer(this.transform, GameManager.instance.GetPlayer()));
        stateMachine.AddState(new BomberMovingToPosition(this.transform));
        stateMachine.AddState(new BomberDead(this.transform));
	}
	
	// Update is called once per frame
	void Update () 
    {
        stateMachine.SetValue("OffScreen", IsOffScreen());
        stateMachine.SetValue("Health", health);
        stateMachine.SetValue("DistanceToDestination", Vector3.Distance(transform.position, destination));
        stateMachine.SetValue("EndAttack", attackTimer >= attackTime);
        stateMachine.UpdateStateMachine();
	}
}
