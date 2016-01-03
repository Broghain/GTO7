using UnityEngine;
using System.Collections;

public class BomberReachedScreenEdge : FSMState {

	//controllers
    private BomberController bomber;
    
    private Vector3 randomPos;

    public BomberReachedScreenEdge(Transform bomber)
    {
        this.bomber = bomber.GetComponent<BomberController>();
        randomPos = Vector3.zero;
    }

    public override void UpdateState()
    {
        if (randomPos == Vector3.zero)
        {
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            randomPos = new Vector3(Random.Range(bottomLeft.x, topRight.x), Random.Range(bottomLeft.y, topRight.y), 0);
        }
        else
        {
            //Rotate toward random position on the screen
            Quaternion targetRotation = Quaternion.LookRotation(randomPos - bomber.transform.position, new Vector3(0, 0, -1));
            bomber.transform.rotation = Quaternion.Slerp(bomber.transform.rotation, targetRotation, Time.deltaTime * bomber.RotationSpeed);

            //Move forward
            bomber.transform.position = Vector3.MoveTowards(bomber.transform.position, randomPos, Time.deltaTime * bomber.MoveSpeed);
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
