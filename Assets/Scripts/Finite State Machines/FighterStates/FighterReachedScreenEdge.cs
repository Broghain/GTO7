using UnityEngine;
using System.Collections;

public class FighterReachedScreenEdge : FSMState {
    
    //controllers
    private PlayerController player;
    private FighterController fighter;

    public FighterReachedScreenEdge(Transform fighter, Transform player)
    {
        this.player = player.GetComponent<PlayerController>();
        this.fighter = fighter.GetComponent<FighterController>();
    }

    public override void UpdateState()
    {
        //Rotate to the player
        Vector3 approxPlayerPosition = player.transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0); //Get approximate player position
        Quaternion targetRotation = Quaternion.LookRotation(approxPlayerPosition - fighter.transform.position, new Vector3(0,0,-1));
        fighter.transform.rotation = Quaternion.Slerp(fighter.transform.rotation, targetRotation, Time.deltaTime * fighter.RotationSpeed);

        //Move forward
        fighter.transform.Translate(Vector3.forward * Time.deltaTime * fighter.MoveSpeed);
    }

    public override StateID GetStateID()
    {
        return StateID.ReachedScreenEdge;
    }
}
