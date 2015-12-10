using UnityEngine;
using System.Collections;

public class BomberAttackingPlayer : FSMState {

    private GameManager gameManager;
    private BomberController bomber;
    private PlayerController player;

    private Vector3 randomPosition;

    public BomberAttackingPlayer(Transform thisObject)
    {
        gameManager = GameManager.instance;
        player = gameManager.GetPlayer().GetComponent<PlayerController>(); ;
        bomber = thisObject.GetComponent<BomberController>();
    }

    public override void UpdateState()
    {
        
    }

    public override StateID GetStateID()
    {
        return StateID.AttackingPlayer;
    }
}
