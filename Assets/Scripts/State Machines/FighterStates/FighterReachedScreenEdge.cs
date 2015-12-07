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
        Quaternion targetRotation = Quaternion.LookRotation(approxPlayerPosition - fighter.transform.position);
        fighter.transform.rotation = Quaternion.Slerp(fighter.transform.rotation, targetRotation, Time.deltaTime * fighter.GetRotationSpeed());

        //Move forward
        fighter.transform.Translate(fighter.transform.forward * Time.deltaTime * fighter.GetMoveSpeed());
    }

    public override FighterStateID GetStateID()
    {
        return FighterStateID.ReachedScreenEdge;
    }
}
