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

    public float health;
    protected float moveSpeed;
    protected float rotationSpeed;

    protected float statMultiplier = 1; //Multiply stats for increased/decreased difficulty

    protected Vector3 screenUpperLeft;
    protected Vector3 screenLowerRight;
    protected bool isOffScreen = true;
    
    protected Collider[] triggers;

    protected void Initialize()
    {
        health = baseHealth * statMultiplier;
        moveSpeed = baseMoveSpeed * statMultiplier;
        rotationSpeed = baseRotationSpeed * statMultiplier;

        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        triggers = GetComponents<Collider>();
    }

    public int SpawnValue
    {
        get { return spawnValue; }
    }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { rotationSpeed = value; }
    }

    public float RotationSpeed
    {
        get { return rotationSpeed; }
        set { rotationSpeed = value; }
    }

    protected void IsOffScreen()
    {
        if ((transform.position.x < screenUpperLeft.x || transform.position.x > screenLowerRight.x || transform.position.y > screenUpperLeft.y || transform.position.y < screenLowerRight.y))
        {
            isOffScreen = true;
            foreach (Collider trigger in triggers)
            {
                if (trigger.enabled == true)
                {
                    trigger.enabled = false;
                }
            }
        }
        else
        {
            isOffScreen = false;
            foreach (Collider trigger in triggers)
            {
                if (trigger.enabled == false)
                {
                    trigger.enabled = true;
                }
            }
        }
    }

    protected void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Shoot()
    {

    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject collidingObject = collider.gameObject;
        if(collidingObject.tag == "Projectile")
        {
            ProjectileController projectile = collidingObject.GetComponent<ProjectileController>();
            projectile.DisableBullet();
            TakeDamage(projectile.GetDamage());
        }
    }

    
}
