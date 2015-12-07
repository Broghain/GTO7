using UnityEngine;
using System.Collections;

public abstract class FSMState {
    // Controls behaviour of the npc
    public abstract void UpdateState();
    //This state's ID
    public abstract FighterStateID GetStateID();
}
