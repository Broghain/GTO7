using UnityEngine;
using System.Collections;

public class BomberMovingToPosition : FSMState {

	private BomberController bomber;

    private Vector3 randomPos;

    public BomberMovingToPosition(Transform bomber)
    {
        this.bomber = bomber.GetComponent<BomberController>();
    }

    public override void UpdateState()
    {
        if (randomPos == Vector3.zero)
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            randomPos = new Vector3(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y), 0);
            bomber.Destination = randomPos;
        }

        Vector3 direction = Vector3.Normalize(bomber.Destination - bomber.transform.position);
        //if facing destination
        if (Vector3.Dot(bomber.transform.forward, direction) > 0.99f)
        {
            //move to destination
            bomber.transform.position = Vector3.MoveTowards(bomber.transform.position, bomber.Destination, Time.deltaTime * bomber.MoveSpeed);
        }
        else
        {
            //rotate toward destination
            Quaternion targetRotation = Quaternion.LookRotation(bomber.Destination - bomber.transform.position, new Vector3(0, 0, -1));
            bomber.transform.rotation = Quaternion.Lerp(bomber.transform.rotation, targetRotation, Time.deltaTime * bomber.RotationSpeed);
        }
        
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
