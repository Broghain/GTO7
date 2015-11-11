using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    private float xSpeed;
    private float ySpeed;

    private GameManager gameManager;

    private Vector3 startPosition;

    //Movement range;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.instance;

        startPosition = transform.position;

        xSpeed = 1;
        ySpeed = 1;

        minX = Camera.main.ScreenToWorldPoint(new Vector3(60, 0, 0)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 60, 0, 0)).x;
        minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 60, 0)).y;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 60, 0)).y;
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
            newYPos = Mathf.Clamp(newYPos, minY, maxY);
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
            newXPos = Mathf.Clamp(newXPos, minX, maxX);
            transform.position = new Vector3(newXPos, transform.position.y, 0);
            xSpeed += (moveSpeed / 100);
        }
        else
        {
            xSpeed = 1;
        }
    }
}
