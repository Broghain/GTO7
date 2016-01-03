using UnityEngine;
using System.Collections;

public class FighterAttackingPlayer : FSMState {

    //controllers
    private PlayerController player;
    private FighterController fighter;

    public FighterAttackingPlayer(Transform fighter, Transform player)
    {
        this.player = player.GetComponent<PlayerController>();
        this.fighter = fighter.GetComponent<FighterController>();
    }

    public override void UpdateState()
    {
        //Move forward
        fighter.transform.Translate(Vector3.forward * Time.deltaTime * fighter.MoveSpeed);

        if (Vector3.Distance(fighter.transform.position, player.transform.position) < 5)
        {
            fighter.Shoot(true, player.transform.position);
            if (fighter.IsMainLeader)
            {
                fighter.FormationShoot(player.transform.position);
            }
        }
    }

    public override void ResetState()
    {
        //nothing to reset
    }

    public override StateID GetStateID()
    {
        return StateID.AttackingPlayer;
    }
}
