using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    //attributes
    [SerializeField]
    private float moveSpeed;
    private float xSpeed = 1.0f;
    private float ySpeed = 1.0f;
    [SerializeField]
    private float maxHealth = 100.0f;
    private float curHealth;
    [SerializeField]
    private float maxShield = 50.0f;
    private float curShield;
    [SerializeField]
    private float shieldRegenTime = 5.0f;
    private float regenTimer = 0.0f;
    [SerializeField]
    private float shieldRegenSpeed = 5.0f;
    [SerializeField]
    private float experienceRequirement = 100.0f;
    [SerializeField]
    private float experienceRequirementMultiplier = 1.5f;
    private float curExperience = 0.0f;

    private int currentLevel;

    //screen edge
    private Vector3 bottomLeft;
    private Vector3 topRight;

    //weapons
    private WeaponController[] weapons;

    //upgrader
    private UpgradeController upgrader;

    //particle effects
    [SerializeField]
    private ParticleSystem experiencePickupEffect;
    [SerializeField]
    private ParticleSystem healthPickupEffect;

    //sound effects
    [SerializeField]
    private AudioClip[] pickupSounds;

    //achievement
    private bool gotHit = false;

    public float Health
    {
        get { return curHealth; }
        set { curHealth = value; }
    }
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    public float Shield
    {
        get { return curShield; }
        set { curShield = value; }
    }
    public float MaxShield
    {
        get { return maxShield; }
        set { maxShield = value; }
    }
    public float Experience
    {
        get { return curExperience; }
    }
    public float MaxExperience
    {
        get { return experienceRequirement; }
    }

	// Use this for initialization
	void Start () {
        curHealth = maxHealth;
        curShield = maxShield;

        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        weapons = GetComponentsInChildren<WeaponController>(true);
        upgrader = GetComponent<UpgradeController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.instance.GetPaused())
        {
            UpdateMovement(); 

            if (Input.GetButton("Fire1"))
            {
                foreach (WeaponController weapon in weapons)
                {
                    if (weapon.gameObject.activeSelf)
                    {
                        weapon.Shoot();
                    }
                }
            }

            if (curShield < maxShield)
            {
                if (regenTimer >= shieldRegenTime)
                {
                    curShield += Time.deltaTime * shieldRegenSpeed;
                }
                else
                {
                    regenTimer += Time.deltaTime;
                }
            }
            else if(regenTimer > 0.0f)
            {
                regenTimer = 0.0f;
                curShield = maxShield;
            }
        }
	}

    private void UpdateMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalInput) > 0)
        {
            float newYPos = transform.position.y + verticalInput * (Time.deltaTime * (moveSpeed + ySpeed));
            newYPos = Mathf.Clamp(newYPos, bottomLeft.y, topRight.y);
            transform.position = new Vector3(transform.position.x, newYPos, 0);
            ySpeed += (moveSpeed / 100);
        }
        else
        {
            ySpeed = 1;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontalInput) > 0)
        {
            float newXPos = transform.position.x + horizontalInput * (Time.deltaTime * (moveSpeed + xSpeed));
            newXPos = Mathf.Clamp(newXPos, bottomLeft.x, topRight.x);
            transform.position = new Vector3(newXPos, transform.position.y, 0);
            xSpeed += (moveSpeed / 100);
        }
        else
        {
            xSpeed = 1;
        }
    }

    public void GainExperience(float amount)
    {
        curExperience += amount;
        if (curExperience > experienceRequirement)
        {
            currentLevel++;
            curExperience -= experienceRequirement;
            experienceRequirement *= experienceRequirementMultiplier;
            StatManager.instance.IncreaseScore(1000 * currentLevel);

            if (upgrader != null)
            {
                upgrader.AvailablePoints++;
            }
        }
    }

    public WeaponController[] GetWeapons()
    {
        return weapons;
    }

    private void TakeDamage(float damage, Vector3 hitPosition)
    {
        gotHit = true;
        if (curShield <= 0)
        {
            if (damage >= curHealth / 3)
            {
                ParticleManager.instance.SpawnHardHitParticle(hitPosition);
            }
            else
            {
                ParticleManager.instance.SpawnHitParticle(hitPosition);
            }
            curShield = 0;
            curHealth -= damage;
            if (curHealth <= 0)
            {
                GameManager.instance.GameOver();
            }
        }
        else
        {
            curShield -= damage;
        }

        

        regenTimer = 0.0f;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "ExperiencePickup")
        {
            Pickup exp = collider.gameObject.GetComponent<Pickup>();
            if (exp != null)
            {
                GainExperience(exp.GetValue());
                experiencePickupEffect.Play(true);
                Destroy(collider.gameObject);
                AudioManager.instance.PlaySoundWithRandomPitch(pickupSounds[Random.Range(0, pickupSounds.Length)], 0.75f, 1.25f);
            }
           
        }

        if (collider.tag == "HealthPickup")
        {
            Pickup hp = collider.gameObject.GetComponent<Pickup>();
            if (hp != null)
            {
                curHealth = Mathf.Clamp(curHealth + hp.GetComponent<Pickup>().GetValue(), 0, maxHealth);
                healthPickupEffect.Play(true);
                Destroy(collider.gameObject);
                AudioManager.instance.PlaySoundWithRandomPitch(pickupSounds[Random.Range(0, pickupSounds.Length)], 0.75f, 1.25f);
            }

        }

        if (collider.tag == "EnemyProjectile")
        {
            ProjectileController projectile = collider.gameObject.GetComponent<ProjectileController>();
            if (projectile != null)
            {
                TakeDamage(projectile.Damage, projectile.transform.position);
                AudioManager.instance.PlaySoundWithRandomPitch(projectile.GetHitClip(), 0.5f, 1.5f);
                projectile.Disable();
            }
        }

        if (collider.tag == "Enemy")
        {
            EnemyBehaviour enemy = collider.gameObject.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                TakeDamage(enemy.CollisionDamage, enemy.transform.position);
                AudioManager.instance.PlaySoundWithRandomPitch(enemy.GetRandomDeathSound(), 0.75f, 1.25f);
                enemy.TakeDamage(enemy.CollisionDamage, transform.position);
            }
        }
    }

    public bool PlayerWasHit()
    {
        return gotHit;
    }
}
