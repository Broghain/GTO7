using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    [SerializeField]
    protected float baseHealth = 100;
    [SerializeField]
    protected float baseMoveSpeed = 5;
    [SerializeField]
    protected float baseRotationSpeed = 5;

    [SerializeField]
    protected int spawnValue = 5; //Point cost to spawn this enemy

    protected float health;
    protected float moveSpeed;
    protected float rotationSpeed;

    protected float statMultiplier = 1; //Multiply stats for increased/decreased difficulty

    protected Vector3 screenUpperLeft;
    protected Vector3 screenLowerRight;
    protected bool isOffScreen = true;
    
    protected Collider trigger;

    public int GetSpawnValue()
    {
        return spawnValue;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public bool IsOffScreen()
    {
        return isOffScreen;
    }

    protected void TakeDamage(float damage)
    {
        health -= damage;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject collidingObject = collider.gameObject;
        if(collidingObject.tag == "Projectile")
        {
            TakeDamage(1);
        }
    }

    
}
