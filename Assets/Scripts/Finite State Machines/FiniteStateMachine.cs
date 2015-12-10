using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StateID
{
    None,
    ReachedScreenEdge,
    AttackingPlayer,
    Dead
}

[RequireComponent(typeof(Animator))]
public class FiniteStateMachine : MonoBehaviour {

    private Animator stateMachine;

    private List<FSMState> states = new List<FSMState>();

    private Dictionary<int, StateID> stateHash = new Dictionary<int, StateID>();
    
    public StateID currentStateID;
    private FSMState currentState;

	// Use this for initialization
	public void InitializeStateMachine () 
    {
        stateMachine = GetComponent<Animator>();

        //Cache all the hashes of the States in our State Machine (case sensitive!)
        foreach (StateID state in (StateID[])System.Enum.GetValues(typeof(StateID)))
        {
            stateHash.Add(Animator.StringToHash("Base Layer." + state.ToString()), state);
        }
	}

    public void UpdateStateMachine()
    {
        StateID nextStateID = stateHash[stateMachine.GetCurrentAnimatorStateInfo(0).fullPathHash];
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
    }

    public void AddState(FSMState state)
    {
        states.Add(state);
    }

    public void SetValue(string paramName, bool value)
    {
        stateMachine.SetBool(paramName, value);
    }
    public void SetValue(string paramName, float value)
    {
        stateMachine.SetFloat(paramName, value);
    }
    public void SetValue(string paramName, int value)
    {
        stateMachine.SetInteger(paramName, value);
    }
}
