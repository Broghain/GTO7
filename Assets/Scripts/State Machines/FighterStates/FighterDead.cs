using UnityEngine;
using System.Collections;

public class FighterDead : FSMState {

    public FighterDead()
    {

    }

    public override void UpdateState()
    {
        //Do nothing
    }
    public override FighterStateID GetStateID()
    {
        return FighterStateID.Dead;
    }
}
