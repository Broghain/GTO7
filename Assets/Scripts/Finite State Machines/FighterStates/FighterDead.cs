using UnityEngine;
using System.Collections;

public class FighterDead : FSMState {

    //controller
    private FighterController fighter;

    public FighterDead(Transform fighter)
    {
        this.fighter = fighter.GetComponent<FighterController>();
    }

    public override void UpdateState()
    {
        fighter.Die();
    }
    public override StateID GetStateID()
    {
        return StateID.Dead;
    }
}
