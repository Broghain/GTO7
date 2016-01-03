using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class BomberController : EnemyBehaviour {

    private FiniteStateMachine stateMachine;

    private Vector3 destination;
    
    [SerializeField]
    private float minAttackTime;
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
        Initialize();

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
        IsOffScreen();
        stateMachine.SetValue("OffScreen", isOffScreen);
        stateMachine.SetValue("Health", health);
        stateMachine.SetValue("DistanceToDestination", Vector3.Distance(transform.position, destination));
        if (attackTimer > minAttackTime && Random.Range(0.0f, minAttackTime*1.5f) <= minAttackTime)
        {
            stateMachine.SetValue("EndAttack", true);
        }
        else
        {
            stateMachine.SetValue("EndAttack", false);
        }
        stateMachine.UpdateStateMachine();
	}
}
