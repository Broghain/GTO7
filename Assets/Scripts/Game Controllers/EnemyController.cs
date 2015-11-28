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
        gameManager = GameManager.instance;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidingObject = collision.collider.gameObject;
        if(collidingObject.tag == "Projectile")
        {
            gameManager.InstantiateExplosion(transform.position);
            Destroy(this.gameObject);
        }
    }
}
