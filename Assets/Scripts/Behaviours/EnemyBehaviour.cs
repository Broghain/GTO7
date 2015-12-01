using UnityEngine;
using System.Collections;

public class EnemyBehaviour : PooledObjectBehaviour {

    [SerializeField]
    protected float baseHealth = 100;
    [SerializeField]
    protected float baseMoveSpeed = 5;

    [SerializeField]
    protected int spawnValue = 5; //Point cost to spawn this enemy

    protected float health;
    protected float moveSpeed;

    public int GetSpawnValue()
    {
        return spawnValue;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collidingObject = collider.gameObject;
        if(collidingObject.tag == "Projectile")
        {
                   
        }
    }

    
}
