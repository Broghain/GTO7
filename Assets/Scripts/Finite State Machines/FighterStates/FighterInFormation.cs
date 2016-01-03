using UnityEngine;
using System.Collections;

public class FighterInFormation : FSMState {

	//controllers
    private FighterController fighter;

    private Vector3 destination;

    public FighterInFormation(Transform fighter)
    {
        this.fighter = fighter.GetComponent<FighterController>();
    }

    public override void UpdateState()
    {
        //find or become leader
        if (fighter.GetLeader() == null && !fighter.IsMainLeader)
        {
            EnemyBehaviour[] potentialPartners = GameManager.instance.GetEnemiesOfType(EnemyType.Fighter);
            foreach (FighterController potentialPartner in potentialPartners)
            {
                if (potentialPartner.IsMainLeader)
                {
                    fighter.IsMainLeader = false;
                    break;
                }
                fighter.IsMainLeader = true; //no leader found, become leader
                fighter.Wing = 0;
            }
        }

        //find partner 0
        if ((fighter.IsMainLeader || fighter.Wing == -1) && fighter.GetPartner(0) == null)
        {
            EnemyBehaviour[] potentialPartners = GameManager.instance.GetEnemiesOfType(EnemyType.Fighter);
            foreach (FighterController potentialPartner in potentialPartners)
            {
                if (potentialPartner.GetLeader() == null)
                {
                    potentialPartner.SetLeader(fighter);
                    fighter.SetPartner(0, potentialPartner);
                    potentialPartner.Wing = -1;
                    break;
                }
            }
        }

        //find partner 1
        if ((fighter.IsMainLeader || fighter.Wing == 1) && fighter.GetPartner(1) == null)
        {
            EnemyBehaviour[] potentialPartners = GameManager.instance.GetEnemiesOfType(EnemyType.Fighter);
            foreach (FighterController potentialPartner in potentialPartners)
            {
                if (potentialPartner.GetLeader() == null)
                {
                    potentialPartner.SetLeader(fighter);
                    fighter.SetPartner(1, potentialPartner);
                    potentialPartner.Wing = 1;
                    break;
                }
            }
        }

        //if not main leader and has a leader, move toward leader
        if (!fighter.IsMainLeader && fighter.GetLeader() != null)
        {
            destination = (fighter.GetLeader().transform.position - fighter.GetLeader().transform.up) + (fighter.GetLeader().transform.right * fighter.Wing);
            //rotate toward destination
            Quaternion targetRotation = Quaternion.LookRotation(destination - fighter.transform.position, new Vector3(0, 0, -1));
            fighter.transform.rotation = Quaternion.Lerp(fighter.transform.rotation, targetRotation, Time.deltaTime * fighter.RotationSpeed);
            //Move forward
            fighter.transform.Translate(Vector3.forward * Time.deltaTime * (fighter.MoveSpeed * 1.5f));
        }
    }

    public override void ResetState()
    {
        fighter.Wing = 0;
    }

    public override StateID GetStateID()
    {
        return StateID.AttackingPlayer;
    }
}
