using UnityEngine;
using System.Collections;

public abstract class FSMState {
    // Controls behaviour of the npc
    public abstract void UpdateState();
    //Reset state variables
    public abstract void ResetState();
    //This state's ID
    public abstract StateID GetStateID();
}
