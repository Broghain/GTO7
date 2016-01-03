using UnityEngine;
using System.Collections;

public class GunnerAttackingPlayer : FSMState {

	//controllers
    private GunnerController gunner;

    public GunnerAttackingPlayer(Transform gunner)
    {
        this.gunner = gunner.GetComponent<GunnerController>();
    }

    public override void UpdateState()
    {
        //shoot
        gunner.Shoot(false, Vector3.zero);

        //if gunner has a partner and this gunner is the leader, make partner shoot too
        if (gunner.IsLeader && gunner.Partner != null)
        {
            gunner.Partner.Shoot(false, Vector3.zero);
        }

        gunner.AttackTimer += Time.deltaTime;
        
    }

    public override void ResetState()
    {
        gunner.AttackTimer = 0.0f;
    }

    public override StateID GetStateID()
    {
        return StateID.AttackingPlayer;
    }
}
