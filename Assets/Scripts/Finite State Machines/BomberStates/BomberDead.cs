using UnityEngine;
using System.Collections;

public class BomberDead : FSMState {

	//controller
    private BomberController bomber;

    public BomberDead(Transform bomber)
    {
        this.bomber = bomber.GetComponent<BomberController>();
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
