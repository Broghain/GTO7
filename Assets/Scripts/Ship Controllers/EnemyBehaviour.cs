using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    //managers
    protected DifficultyManager diffMng;
    protected GameManager gameMng;

    //base attribute values
    [SerializeField]
    protected float baseHealth = 100;
    [SerializeField]
    protected float baseMoveSpeed = 5;
    [SerializeField]
    protected float baseRotationSpeed = 5;

    //cost to spawn this enemy
    [SerializeField]
    protected int spawnCost = 5; 

    //attributes
    protected float health;
    protected float moveSpeed;
    protected float rotationSpeed;

    //screen edge
    protected Vector3 screenUpperLeft;
    protected Vector3 screenLowerRight;
    protected bool isOffScreen = true;
    
    //colliders
    protected Collider[] triggers;

    //weapons
    protected WeaponController[] weapons;

    //life time of this agent (used for adaptive difficulty)
    protected float lifeTime = 0.0f;

    protected void Initialize()
    {
        gameMng = GameManager.instance;
        diffMng = DifficultyManager.instance;
        health = baseHealth * diffMng.GetStatMultiplier();
        moveSpeed = baseMoveSpeed * diffMng.GetStatMultiplier();
        rotationSpeed = baseRotationSpeed * diffMng.GetStatMultiplier();

        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        triggers = GetComponents<Collider>();

        weapons = GetComponentsInChildren<WeaponController>();
    }

    public int SpawnCost
    {
        get { return spawnCost; }
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
        foreach (WeaponController weapon in weapons)
        {
            weapon.Shoot();
        }
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
