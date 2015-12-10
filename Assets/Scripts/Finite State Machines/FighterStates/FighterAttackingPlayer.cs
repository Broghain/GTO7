using UnityEngine;
using System.Collections;

public class FighterAttackingPlayer : FSMState {

    private GameManager gameManager;
    private PlayerController player;
    private FighterController fighter;

    public FighterAttackingPlayer(Transform thisObject)
    {
        gameManager = GameManager.instance;
        player = gameManager.GetPlayer().GetComponent<PlayerController>();
        fighter = thisObject.GetComponent<FighterController>();
    }

    public override void UpdateState()
    {
        //Move forward
        fighter.transform.Translate(Vector3.forward * Time.deltaTime * fighter.MoveSpeed);

        if (Vector3.Distance(fighter.transform.position, player.transform.position) < 4)
        {

        }
    }

    public override StateID GetStateID()
    {
        return StateID.AttackingPlayer;
    }
}
