using UnityEngine;
using System.Collections;

public class GunnerInFormation : FSMState {

    private GunnerController gunner;

    public GunnerInFormation(Transform gunner)
    {
        this.gunner = gunner.GetComponent<GunnerController>();
    }

    public override void UpdateState()
    {
        //delete partner if partner is inactive
        if (gunner.Partner != null)
        {
            if (!gunner.Partner.gameObject.activeSelf)
            {
                gunner.Partner = null;
            }
        }

        //look for partner
        if (gunner.Partner == null)
        {
            EnemyBehaviour[] potentialPartners = GameManager.instance.GetEnemiesOfType(EnemyType.Gunner);
            foreach (GunnerController potentialPartner in potentialPartners)
            {
                if (potentialPartner != gunner && (potentialPartner.Partner == null || !potentialPartner.Partner.gameObject.activeSelf))
                {
                    potentialPartner.Partner = gunner;
                    gunner.Partner = potentialPartner;
                    gunner.IsLeader = true;
                    potentialPartner.IsLeader = false;
                    break;
                }
            }
        }

        //if not leader and has partner, mirror partner's movements
        if (!gunner.IsLeader && gunner.Partner != null)
        {
            Vector3 mirrorDestination = new Vector3(-gunner.Partner.Destination.x, gunner.Partner.Destination.y, 0);
            gunner.transform.position = Vector3.MoveTowards(gunner.transform.position, mirrorDestination, Time.deltaTime * gunner.MoveSpeed);
        }
    }

    public override void ResetState()
    {
        
    }

    public override StateID GetStateID()
    {
        return StateID.InFormation;
    }
	
}
