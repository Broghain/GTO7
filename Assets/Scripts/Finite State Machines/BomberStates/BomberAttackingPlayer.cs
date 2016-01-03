using UnityEngine;
using System.Collections;

public class BomberAttackingPlayer : FSMState {

    //controllers
    private BomberController bomber;
    private PlayerController player;

    public BomberAttackingPlayer(Transform bomber, Transform player)
    {
        this.player = player.GetComponent<PlayerController>(); ;
        this.bomber = bomber.GetComponent<BomberController>();
    }

    public override void UpdateState()
    {
        //rotate towards player
        Vector3 playerPosition = player.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(playerPosition - bomber.transform.position, new Vector3(0, 0, -1));
        bomber.transform.rotation = Quaternion.Slerp(bomber.transform.rotation, targetRotation, Time.deltaTime * bomber.RotationSpeed);

        bomber.Shoot(false, Vector3.zero);
        bomber.AttackTimer += Time.deltaTime;
    }

    public override void ResetState()
    {
        bomber.AttackTimer = 0.0f;
    }

    public override StateID GetStateID()
    {
        return StateID.AttackingPlayer;
    }
}
