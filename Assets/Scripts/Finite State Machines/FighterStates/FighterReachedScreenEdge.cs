using UnityEngine;
using System.Collections;

public class FighterReachedScreenEdge : FSMState {
    
    //controllers
    private PlayerController player;
    private FighterController fighter;

    private Vector3 playerPosition;

    public FighterReachedScreenEdge(Transform fighter, Transform player)
    {
        this.player = player.GetComponent<PlayerController>();
        this.fighter = fighter.GetComponent<FighterController>();
        playerPosition = Vector3.zero;
    }

    public override void UpdateState()
    {
        //Rotate to the player
        if (playerPosition == Vector3.zero)
        {
            playerPosition = player.transform.position; //Get player position
        }
        Quaternion targetRotation = Quaternion.LookRotation(playerPosition - fighter.transform.position, new Vector3(0, 0, -1));
        fighter.transform.rotation = Quaternion.Slerp(fighter.transform.rotation, targetRotation, Time.deltaTime * fighter.RotationSpeed);

        //Move forward
        fighter.transform.Translate(Vector3.forward * Time.deltaTime * fighter.MoveSpeed);
    }

    public override void ResetState()
    {
        playerPosition = Vector3.zero;
    }

    public override StateID GetStateID()
    {
        return StateID.ReachedScreenEdge;
    }
}
