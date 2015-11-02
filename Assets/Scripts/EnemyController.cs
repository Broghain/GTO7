using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    private GameManager gameManager;

    private Vector3 randomPos;

    [SerializeField]
    private float moveSpeed;

	// Use this for initialization
	void Start () 
    {
        randomPos = new Vector3(Random.Range(-11, 11), Random.Range(-3, 4), 0);
        gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = Vector3.Lerp(transform.position, randomPos, Time.deltaTime * moveSpeed);
        if (Vector3.Distance(transform.position, randomPos) <= 0.01f)
        {
            randomPos = new Vector3(Random.Range(-11, 11), Random.Range(-3, 4), 0);
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
