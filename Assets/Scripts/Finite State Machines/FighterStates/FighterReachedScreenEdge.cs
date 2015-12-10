using UnityEngine;
using System.Collections;

public class FighterReachedScreenEdge : FSMState {

    private GameManager gameManager;
    private PlayerController player;
    private FighterController fighter;

    public FighterReachedScreenEdge(Transform thisObject)
    {
        gameManager = GameManager.instance;
        player = gameManager.GetPlayer().GetComponent<PlayerController>();
        fighter = thisObject.GetComponent<FighterController>();
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
