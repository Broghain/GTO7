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
    protected float baseExpCount = 2.0f;
    [SerializeField]
    protected float baseScoreValue = 25.0f;
    [SerializeField]
    protected float baseHealthDropValue = 25.0f;

    [SerializeField]
    protected float collisionDamage = 15.0f;

    [SerializeField]
    protected float baseActiveWeaponCount = 2;

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
   
    protected float expCount;
    protected float scoreValue;
    protected float healthDropValue;

    protected float activeWeaponCount;

    //screen edge
    protected Vector3 screenUpperLeft;
    protected Vector3 screenLowerRight;
    
    //colliders
    protected Collider[] triggers;

    //weapons
    protected WeaponController[] weapons;

    //life time of this agent (used for adaptive difficulty)
    protected float spawnTime;

    private PooledObjectBehaviour poolBehaviour;

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

    public bool IsOffScreen()
    {
        if ((transform.position.x < screenUpperLeft.x || transform.position.x > screenLowerRight.x || transform.position.y > screenUpperLeft.y || transform.position.y < screenLowerRight.y))
        {
            return true;
        }
        else
        {
            return false;
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
            if (weapon.gameObject.activeSelf)
            {
                if (aim)
                {
                    weapon.transform.up = Vector3.Normalize(targetPosition - transform.position);
                }
                weapon.Shoot();
            }
        }
    }

    public void Die()
    {
        DifficultyManager.instance.SetAvgLifeTime(Time.time - spawnTime);

        ParticleManager.instance.SpawnEnemyExplodeParticle(transform.position);
        AudioManager.instance.PlaySoundWithRandomPitch(deathSounds[Random.Range(0, deathSounds.Length)], 0.75f, 1.25f);

        DropExperience();
        DropHealth();

        SplineTracer tracer = GetComponent<SplineTracer>();
        if (tracer != null) //is this enemy using a spline?
        {
            tracer.RemoveFromTrack(); //remove this enemy from the list in the splinetracer
        }

        if (poolBehaviour != null) //is this enemy pooled?
        {
            poolBehaviour.DisableInPool(); //disable this enemy
            if (!ignoreEnemyCount) //does this enemy count toward the wave's enemy count?
            {
                SpawnManager.instance.DecreaseEnemyCount(1); //decrease enemy count
                StatManager.instance.IncreaseKillCount(); //increase kill count
            }
        }

        StatManager.instance.IncreaseScore((int)scoreValue);
    }

    public void Reset()
    {
        spawnTime = Time.time;
        gameMng = GameManager.instance;
        diffMng = DifficultyManager.instance;

        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));

        triggers = GetComponents<Collider>();
        poolBehaviour = GetComponent<PooledObjectBehaviour>();

        SetStats();
        ActivateWeapons();

        TrailResetter[] trailResetters = GetComponentsInChildren<TrailResetter>();
        foreach (TrailResetter resetter in trailResetters)
        {
            resetter.Reset();
        }
    }

    private void SetStats()
    {
        health = baseHealth * diffMng.GetDifficultyMultiplier();
        moveSpeed = baseMoveSpeed * diffMng.GetDifficultyMultiplier();
        rotationSpeed = baseRotationSpeed * diffMng.GetDifficultyMultiplier();

        expCount = baseExpCount * diffMng.GetDifficultyMultiplier();
        scoreValue = baseScoreValue * diffMng.GetDifficultyMultiplier();
        healthDropValue = Random.Range(0, (baseHealthDropValue * 2) / diffMng.GetDifficultyMultiplier());
    }

    private void ActivateWeapons()
    {
        weapons = GetComponentsInChildren<WeaponController>(true);
        activeWeaponCount = Mathf.Clamp(baseActiveWeaponCount * diffMng.GetDifficultyMultiplier(), 1, weapons.Length);
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i <= activeWeaponCount)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

    private void DropExperience()
    {
        if (expCount > 0) //has experience points to drop?
        {
            float expRemaining = expCount;
            while (expRemaining > 0) //while remaining exp drops is higher than 0
            {
                GameObject expObj = gameMng.DropExperiencePickup(transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0));
                if (expRemaining >= 10) //has 10 or more exp points left?
                {
                    expObj.GetComponent<Pickup>().SetValue(10); //drop exp pickup with 10 points
                }
                else
                {
                    expObj.GetComponent<Pickup>().SetValue(expRemaining); //drop exp pickup with remaining points
                }
                expRemaining -= 10;
            }
        }
    }

    private void DropHealth()
    {
        if (healthDropValue > 0) //has health points to drop?
        {
            float healthDropRemaining = healthDropValue;
            while (healthDropRemaining > 0) //while remaining health drops is higher than 0
            {
                if (Random.Range(0, healthDropValue) > baseHealthDropValue)
                {
                    GameObject hpObj = gameMng.DropHealthPickup(transform.position + new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), 0));
                    if (healthDropRemaining >= 25) //has 10 or more health points left?
                    {
                        hpObj.GetComponent<Pickup>().SetValue(25); //drop health pickup with 25 points
                    }
                    else
                    {
                        hpObj.GetComponent<Pickup>().SetValue(healthDropRemaining); //drop health pickup with remaining points
                    }
                }
                healthDropRemaining -= 25;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject collidingObject = collider.gameObject;
        if(collidingObject.tag == "Projectile" && !IsOffScreen())
        {
            ProjectileController projectile = collidingObject.GetComponent<ProjectileController>();
            AudioManager.instance.PlaySoundWithRandomPitch(projectile.GetHitClip(), 0.5f, 1.5f);
            TakeDamage(projectile.Damage, projectile.transform.position);
            projectile.Disable();
            diffMng.SetHitPct(true, projectile.GetWeaponType());
        }
    }
}
