using UnityEngine;
using System.Collections;

public class BomberAttackingPlayer : FSMState {

    //controllers
    private BomberController bomber;
    private PlayerController player;

    private Vector3 randomPosition;

    public BomberAttackingPlayer(Transform bomber, Transform player)
    {
        this.player = player.GetComponent<PlayerController>(); ;
        this.bomber = bomber.GetComponent<BomberController>();
    }

    public override void UpdateState()
    {
        
    }

    public override StateID GetStateID()
    {
        return StateID.AttackingPlayer;
    }
}
