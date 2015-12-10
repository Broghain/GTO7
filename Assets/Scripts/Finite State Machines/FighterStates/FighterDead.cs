using UnityEngine;
using System.Collections;

public class FighterDead : FSMState {

    private FighterController fighter;

    public FighterDead(Transform thisObject)
    {
        this.fighter = thisObject.GetComponent<FighterController>();
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
