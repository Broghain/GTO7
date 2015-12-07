using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum FighterStateID
{
    None,
    ReachedScreenEdge,
    AttackingPlayer,
    Dead
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FighterController))]
public class FighterStateMachine : MonoBehaviour {

    private Animator stateMachine;
    private FighterController fighterController;

    private List<FSMState> states = new List<FSMState>();

    private Dictionary<int, FighterStateID> fighterStateHash = new Dictionary<int, FighterStateID>();
    
    public FighterStateID currentStateID;
    private FSMState currentState;

	// Use this for initialization
	public void InitializeStateMachine () 
    {
        stateMachine = GetComponent<Animator>();
        fighterController = GetComponent<FighterController>();

        if (!stateMachine.runtimeAnimatorController)
        {
            Debug.LogError("Animator Controller missing or not configured!");
        }

        //Cache all the hashes of the States in our State Machine (case sensitive!)
        foreach (FighterStateID state in (FighterStateID[])System.Enum.GetValues(typeof(FighterStateID)))
        {
            fighterStateHash.Add(Animator.StringToHash("Base Layer." + state.ToString()), state);
        }

        states.Add(new FighterReachedScreenEdge(transform));
        states.Add(new FighterAttackingPlayer(transform));
        states.Add(new FighterDead());
	}

    public void UpdateStateMachine()
    {
        FighterStateID nextStateID = fighterStateHash[stateMachine.GetCurrentAnimatorStateInfo(0).nameHash];
        if (nextStateID != currentStateID) //If state has changed
        {
            currentStateID = nextStateID;
            foreach (FSMState state in states) //Get state
            {
                if (state.GetStateID() == currentStateID)
                {
                    currentState = state;
                }
            }
        }

        if (currentState != null)
        {
            currentState.UpdateState();
        }

        SetStateVariables();
    }

    private void SetStateVariables()
    {
        stateMachine.SetBool("OffScreen", fighterController.IsOffScreen());
        stateMachine.SetFloat("Health", fighterController.GetHealth());
    }
}
