using UnityEngine;
using System.Collections;

public class GunnerReachedScreenEdge : FSMState {

    //controllers
    private GunnerController gunner;

    private Vector3 randomPos;

	public GunnerReachedScreenEdge(Transform gunner)
    {
        this.gunner = gunner.GetComponent<GunnerController>();
        randomPos = Vector3.zero;
    }

    public override void UpdateState()
    {
        //Rotate toward random position on the screen
        if (randomPos == Vector3.zero)
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            randomPos = new Vector3(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y), 0);
        }
        else
        {
            //Move to random position
            gunner.transform.position = Vector3.MoveTowards(gunner.transform.position, randomPos, Time.deltaTime * gunner.MoveSpeed);
        }
        
    }

    public override void ResetState()
    {
        randomPos = Vector3.zero;
    }

    public override StateID GetStateID()
    {
        return StateID.ReachedScreenEdge;
    }
}
