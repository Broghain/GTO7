using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FiniteStateMachine))]
public class FighterController : EnemyBehaviour {

    private FiniteStateMachine stateMachine;

    private FighterController[] partners;
    private FighterController leader;
    private bool isMainLeader;
    private int wing; //-1 = left wing, 1 = right wing, 0 = main leader

    [SerializeField]
    private float lookForFormationInterval = 5.0f;
    private float partnerTimer = 0.0f;

	// Use this for initialization
	void Start () 
    {
        transform.rotation = Quaternion.LookRotation(GameManager.instance.GetPlayer().position - transform.position, new Vector3(0, 0, -1));

        stateMachine = GetComponent<FiniteStateMachine>();
        stateMachine.InitializeStateMachine();
        stateMachine.AddState(new FighterAttackingPlayer(transform, gameMng.GetPlayer()));
        stateMachine.AddState(new FighterReachedScreenEdge(transform, gameMng.GetPlayer()));
        stateMachine.AddState(new FighterDead(transform));

        partners = new FighterController[2];
	}
	
	// Update is called once per frame
	void Update () 
    {
        IsOffScreen();
        stateMachine.SetValue("OffScreen", isOffScreen);
        stateMachine.SetValue("Health", health);
        stateMachine.UpdateStateMachine();
	}

    public void FormationShoot(Vector3 position)
    {
        //if fighter has partner(s), make partner(s) shoot too
        if (GetPartner(0) != null)
        {
            GetPartner(0).Shoot(true, position);
            GetPartner(0).FormationShoot(position);
        }
        if (GetPartner(1) != null)
        {
            GetPartner(1).Shoot(true, position);
            GetPartner(1).FormationShoot(position);
        }
    }

    public void SetPartner(int index, FighterController partner)
    {
        partners[index] = partner;
    }

    public FighterController GetPartner(int index)
    {
        return partners[index];
    }

    public FighterController GetLeader()
    {
        return leader;
    }

    public void SetLeader(FighterController fighter)
    {
        leader = fighter;
    }

    public bool IsMainLeader
    {
        get { return isMainLeader; }
        set { isMainLeader = value; }
    }

    public int Wing
    {
        get { return wing; }
        set { wing = value; }
    }
}
