using UnityEngine;
using System.Collections;

public class GunnerDead : FSMState {

	//controller
    private GunnerController gunner;

    public GunnerDead(Transform gunner)
    {
        this.gunner = gunner.GetComponent<GunnerController>();
    }

    public override void UpdateState()
    {
        //do nothing
    }

    public override void ResetState()
    {
        //nothing to reset
    }

    public override StateID GetStateID()
    {
        return StateID.Dead;
    }
}
