using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    private float xSpeed = 1.0f;
    private float ySpeed = 1.0f;

    //Movement range;
    private Vector3 bottomLeft;
    private Vector3 topRight;

	// Use this for initialization
	void Start () {
        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMovement();
	}

    private void UpdateMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalInput) > 0)
        {
            float newYPos = transform.position.y + verticalInput * (Time.deltaTime * (moveSpeed + ySpeed));
            newYPos = Mathf.Clamp(newYPos, bottomLeft.y, topRight.y);
            transform.position = new Vector3(transform.position.x, newYPos, 0);
            ySpeed += (moveSpeed / 100);
        }
        else
        {
            ySpeed = 1;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0)
        {
            float newXPos = transform.position.x + horizontalInput * (Time.deltaTime * (moveSpeed + xSpeed));
            newXPos = Mathf.Clamp(newXPos, bottomLeft.x, topRight.x);
            transform.position = new Vector3(newXPos, transform.position.y, 0);
            xSpeed += (moveSpeed / 100);
        }
        else
        {
            xSpeed = 1;
        }
    }
}
