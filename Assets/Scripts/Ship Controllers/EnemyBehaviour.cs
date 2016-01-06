using UnityEngine;
using System.Collections;

public enum EnemyType
{
    Drone,
    Fighter,
    Bomber,
    Gunner
}

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
    [SerializeField]
    protected float baseExpDropCount = 2.0f;
    [SerializeField]
    protected float baseScoreValue = 25.0f;
    [SerializeField]
    protected float collisionDamage = 15.0f;

    [SerializeField]
    private bool ignoreEnemyCount = false;

    //cost to spawn this enemy
    [SerializeField]
    protected int spawnCost = 5; 

    //sound effects
    [SerializeField]
    private AudioClip[] deathSounds;

    //attributes
    protected float health;
    protected float moveSpeed;
    protected float rotationSpeed;
    protected float expDropCount;
    protected float scoreValue;

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

    private PooledObjectBehaviour poolBehaviour;

    protected void Initialize()
    {
        gameMng = GameManager.instance;
        diffMng = DifficultyManager.instance;
        health = baseHealth * diffMng.GetStatMultiplier();
        moveSpeed = baseMoveSpeed * diffMng.GetStatMultiplier();
        rotationSpeed = baseRotationSpeed * diffMng.GetStatMultiplier();
        expDropCount = baseExpDropCount * diffMng.GetStatMultiplier();
        scoreValue = baseScoreValue * diffMng.GetStatMultiplier();

        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        triggers = GetComponents<Collider>();

        weapons = GetComponentsInChildren<WeaponController>();

        poolBehaviour = GetComponent<PooledObjectBehaviour>();
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

    public float CollisionDamage
    {
        get { return collisionDamage; }
        set { collisionDamage = value; }
    }

    public AudioClip GetRandomDeathSound()
    {
        return deathSounds[Random.Range(0, deathSounds.Length)];
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

    public void TakeDamage(float damage, Vector3 hitPosition)
    {
        if (damage >= health / 3)
        {
            ParticleManager.instance.SpawnHardHitParticle(hitPosition);
        }
        else
        {
            ParticleManager.instance.SpawnHitParticle(hitPosition);
        }
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Shoot(bool aim, Vector3 targetPosition)
    {
        foreach (WeaponController weapon in weapons)
        {
            if (aim)
            {
                weapon.transform.up = Vector3.Normalize(targetPosition - transform.position);
            }
            weapon.Shoot();
        }
    }

    public void Die()
    {
        ParticleManager.instance.SpawnEnemyExplodeParticle(transform.position);
        AudioManager.instance.PlaySoundWithRandomPitch(deathSounds[Random.Range(0, deathSounds.Length)], 0.75f, 1.25f);

        if (expDropCount > 0)
        {
            for (int i = 0; i <= expDropCount; i++)
            {
                GameManager.instance.DropExperiencePickup(transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0));
            }
        }

        SplineTracer tracer = GetComponent<SplineTracer>();
        if (tracer != null)
        {
            tracer.RemoveFromTrack();
        }

        if (poolBehaviour != null)
        {
            poolBehaviour.DisableInPool();
            if (!ignoreEnemyCount)
            {
                SpawnManager.instance.DecreaseEnemyCount(1);
                StatManager.instance.IncreaseKillCount();
            }
        }

        StatManager.instance.IncreaseScore((int)scoreValue);
    }

    public void Reset()
    {
        gameMng = GameManager.instance;
        diffMng = DifficultyManager.instance;
        health = baseHealth * diffMng.GetStatMultiplier();
        moveSpeed = baseMoveSpeed * diffMng.GetStatMultiplier();
        rotationSpeed = baseRotationSpeed * diffMng.GetStatMultiplier();
        expDropCount = baseExpDropCount * diffMng.GetStatMultiplier();

        weapons = GetComponentsInChildren<WeaponController>();

        TrailResetter[] trailResetters = GetComponentsInChildren<TrailResetter>();
        foreach (TrailResetter resetter in trailResetters)
        {
            resetter.Reset();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject collidingObject = collider.gameObject;
        if(collidingObject.tag == "Projectile" && !isOffScreen)
        {
            ProjectileController projectile = collidingObject.GetComponent<ProjectileController>();
            AudioManager.instance.PlaySoundWithRandomPitch(projectile.GetHitClip(), 0.5f, 1.5f);
            TakeDamage(projectile.Damage, projectile.transform.position);
            projectile.Disable();
        }
    }
}
