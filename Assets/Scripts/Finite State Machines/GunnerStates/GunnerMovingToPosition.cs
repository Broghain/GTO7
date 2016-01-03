using UnityEngine;
using System.Collections;

public class GunnerMovingToPosition : FSMState {

    private GunnerController gunner;

    private Vector3 randomPos;

    public GunnerMovingToPosition(Transform gunner)
    {
        this.gunner = gunner.GetComponent<GunnerController>();
    }

    public override void UpdateState()
    {
        if (randomPos == Vector3.zero)
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            randomPos = new Vector3(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y), 0);
            gunner.Destination = randomPos;
        }

        //move to destination
        gunner.transform.position = Vector3.MoveTowards(gunner.transform.position, gunner.Destination, Time.deltaTime * gunner.MoveSpeed);
    }

    public override void ResetState()
    {
        randomPos = Vector3.zero;
    }

    public override StateID GetStateID()
    {
        return StateID.MovingToPosition;
    }
	
}
