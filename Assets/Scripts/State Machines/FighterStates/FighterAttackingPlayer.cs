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
        fighter.transform.Translate(fighter.transform.forward * Time.deltaTime * fighter.GetMoveSpeed());
    }

    public override FighterStateID GetStateID()
    {
        return FighterStateID.AttackingPlayer;
    }
}
