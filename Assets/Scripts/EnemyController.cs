using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private GameManager gameManager;

    private Vector3 randomPos;

    [SerializeField]
    private float moveSpeed;

    //Movement range;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

	// Use this for initialization
	void Start () 
    {
        minX = Camera.main.ScreenToWorldPoint(new Vector3(60, 0, 0)).x;
        maxX = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 60, 0, 0)).x;
        minY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 100, 0)).y;
        maxY = Camera.main.ScreenToWorldPoint(new Vector3(0, 100, 0)).y;

        Debug.Log(minY + ", " + maxY);

        randomPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = Vector3.Lerp(transform.position, randomPos, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, randomPos) <= 0.1f)
        {
            randomPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidingObject = collision.collider.gameObject;
        if(collidingObject.tag == "Ball")
        {
            gameManager.InstantiateExplosion(transform.position);
            Destroy(this.gameObject);
        }
    }
}
