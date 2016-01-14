using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class GunnerController : EnemyBehaviour {

    private FiniteStateMachine stateMachine;

    private Vector3 destination;

    [SerializeField]
    private float attackTime = 5.0f;
    private float attackTimer;

    [SerializeField]
    private float lookForPartnerInterval = 5.0f;
    private float partnerTimer = 0.0f;
    public GunnerController partner;
    private bool isLeader;

    public GunnerController Partner
    {
        get { return partner; }
        set { partner = value; }
    }

    public bool IsLeader
    {
        get { return isLeader; }
        set { isLeader = value; }
    }

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
        stateMachine.AddState(new GunnerReachedScreenEdge(this.transform));
        stateMachine.AddState(new GunnerAttackingPlayer(this.transform));
        stateMachine.AddState(new GunnerMovingToPosition(this.transform));
        stateMachine.AddState(new GunnerInFormation(this.transform));
        stateMachine.AddState(new GunnerDead(this.transform));
	}
	
	// Update is called once per frame
	void Update () 
    {
        stateMachine.UpdateStateMachine();

        IsOffScreen();
        stateMachine.SetValue("OffScreen", isOffScreen);
        stateMachine.SetValue("Health", health);
        stateMachine.SetValue("DistanceToDestination", Vector3.Distance(transform.position, destination));
        stateMachine.SetValue("IsLeader", isLeader);
        stateMachine.SetValue("PartnerFound", partner != null);
        stateMachine.SetValue("EndAttack", attackTimer >= attackTime);

        if (partnerTimer >= lookForPartnerInterval && partner == null)
        {
            stateMachine.SetValue("LookForPartner");
            partnerTimer = 0.0f;
        }
        else
        {
            partnerTimer += Time.deltaTime;
        }
	}
}
