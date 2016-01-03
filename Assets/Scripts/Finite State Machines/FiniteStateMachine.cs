using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum StateID
{
    None,
    ReachedScreenEdge,
    AttackingPlayer,
    MovingToPosition,
    InFormation,
    Dead
}

[RequireComponent(typeof(Animator))]
public class FiniteStateMachine : MonoBehaviour {

    private Animator stateMachine;

    private List<FSMState> states = new List<FSMState>();

    private Dictionary<int, StateID> stateDictionary = new Dictionary<int, StateID>();
    
    public StateID currentStateID;
    private FSMState currentState;

	// Use this for initialization
	public void InitializeStateMachine () 
    {
        stateMachine = GetComponent<Animator>();

        //Cache all the hashes of the States in our State Machine (case sensitive!)
        foreach (StateID state in (StateID[])System.Enum.GetValues(typeof(StateID)))
        {
            stateDictionary.Add(Animator.StringToHash("Base Layer." + state.ToString()), state);
        }
	}

    public void UpdateStateMachine()
    {
        StateID nextStateID = stateDictionary[stateMachine.GetCurrentAnimatorStateInfo(0).fullPathHash];
        if (nextStateID != currentStateID) //If state has changed
        {
            currentStateID = nextStateID;
            foreach (FSMState state in states) //Get state
            {
                if (state.GetStateID() == currentStateID)
                {
                    if (currentState != null)
                    {
                        currentState.ResetState();
                    }
                    currentState = state;
                    break;
                }
            }
        }

        if (currentState != null)
        {
            currentState.UpdateState(); //update current state
        }
    }

    public void AddState(FSMState state)
    {
        states.Add(state);
    }

    //set values in mecanim state machine
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
    public void SetValue(string paramName)
    {
        stateMachine.SetTrigger(paramName);
    }
}
