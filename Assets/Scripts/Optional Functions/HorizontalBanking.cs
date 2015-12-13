using UnityEngine;
using System.Collections;

public class HorizontalBanking : MonoBehaviour {

    [SerializeField]
    private float bankingSpeed = 100.0f; //Speed of rotation when ship moves right or left
    [SerializeField]
    private float bankingMaxRotation = 30.0f;
    private Vector3 bankingRotation;

    private float lastXPosition;

	// Use this for initialization
	void Start () {
        bankingRotation = transform.rotation.eulerAngles;
        lastXPosition = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        float xMoveDistance = lastXPosition - transform.position.x;
        if (Mathf.Abs(xMoveDistance) > 0.1f)
        {
            bankingRotation.y += Time.deltaTime * (Mathf.Sign(xMoveDistance) * bankingSpeed);
            bankingRotation.y = Mathf.Clamp(bankingRotation.y, -bankingMaxRotation, bankingMaxRotation);
        }
        else
        {
            if (bankingRotation.y < 0.0f)
            {
                bankingRotation.y += Time.deltaTime * bankingSpeed;
                bankingRotation.y = Mathf.Clamp(bankingRotation.y, -bankingMaxRotation, 0);
            }
            else if (bankingRotation.y > 0.0f)
            {
                bankingRotation.y -= Time.deltaTime * bankingSpeed;
                bankingRotation.y = Mathf.Clamp(bankingRotation.y, 0, bankingMaxRotation);
            }
        }
        transform.rotation = Quaternion.Euler(bankingRotation);
        lastXPosition = transform.position.x;
	}
}
