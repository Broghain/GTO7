using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    private float xSpeed = 1.0f;
    private float ySpeed = 1.0f;

    private GameManager gameManager = null;

    private Vector3 startPosition = Vector3.zero;

    //Movement range;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float rotationSpeed = 100.0f; //Speed of rotation when ship moves right or left
    private Vector3 rotation;

	// Use this for initialization
	void Start () {
        gameManager = GameManager.instance;

        startPosition = transform.position;

        minX = Camera.main.ScreenToWorldPoint(new Vector3(60, 0, 0)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 60, 0, 0)).x;
        minY = Camera.main.ScreenToWorldPoint(new Vector3(0, 60, 0)).y;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 60, 0)).y;

        rotation = transform.rotation.eulerAngles;
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

            rotation.y += Time.deltaTime * (Mathf.Sign(-horizontalInput) * rotationSpeed);
            rotation.y = Mathf.Clamp(rotation.y, -10, 10);
            transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            if (rotation.y < 0.0f)
            {
                rotation.y += Time.deltaTime * rotationSpeed;
                rotation.y = Mathf.Clamp(rotation.y, -10, 0);
                transform.rotation = Quaternion.Euler(rotation);
            }
            else if(rotation.y > 0.0f)
            {
                rotation.y -= Time.deltaTime * rotationSpeed;
                rotation.y = Mathf.Clamp(rotation.y, 0, 10);
                transform.rotation = Quaternion.Euler(rotation);
            }
            xSpeed = 1;
        }
    }
}
