using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FighterStateMachine))]
public class FighterController : EnemyBehaviour {

    private FighterStateMachine stateMachine;

	// Use this for initialization
	void Start () {
        stateMachine = GetComponent<FighterStateMachine>();

        health = baseHealth * statMultiplier;
        moveSpeed = baseMoveSpeed * statMultiplier;
        rotationSpeed = baseRotationSpeed * statMultiplier;

        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        trigger = GetComponent<Collider>();

        stateMachine.InitializeStateMachine();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ((transform.position.x < screenUpperLeft.x || transform.position.x > screenLowerRight.x || transform.position.y > screenUpperLeft.y || transform.position.y < screenLowerRight.y))
        {
            isOffScreen = true;
            if (trigger.enabled == true)
            {
                trigger.enabled = false;
            }
        }
        else
        {
            isOffScreen = false;
            if (trigger.enabled == false)
            {
                trigger.enabled = true;
            }
        }

        stateMachine.UpdateStateMachine();
	}
}
