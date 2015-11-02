using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;

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

        minX = Camera.main.ScreenToWorldPoint(new Vector3(60, 0, 0)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 60, 0, 0)).x;
        minY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 60, 0)).y;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, 60, 0)).y;
	}
	
	// Update is called once per frame
	void Update () {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0)
        {
            float newXPos = transform.position.x + horizontalInput * (Time.deltaTime * moveSpeed);
            newXPos = Mathf.Clamp(newXPos, minX, maxX);
            transform.position = new Vector3(newXPos , startPosition.y, 0);
        }
	}
}
