using UnityEngine;
using System.Collections;

public class DifficultyManager : MonoBehaviour {

    public static DifficultyManager instance;

    //multiplies stats for increased/decreased difficulty (min 0.5, max 2.0)
    private float diffMultiplier = 1; 

    //average lifetime of enemy ships in seconds
    private float avgLifeTime = 0.0f;

    //Percentage of pickups taken
    public float pickupTakenPct;
   
    private int pickupsDropped = 0;
    private int pickupsTaken = 0;

    //player
    private PlayerController player;

    //player projectile hits
    public float bulletHitPct = 0.0f;
    public float laserHitPct = 0.0f;
    public float rocketHitPct = 0.0f;

    private float bulletsFired = 0;
    private float lasersFired = 0;
    private float rocketsFired = 0;
    private float bulletsHit = 0;
    private float lasersHit = 0;
    private float rocketsHit = 0;

    //player health variation per minute
    private float playerHealthPerMinute = 0;
    private float lastHealth = 100;
    private float playerShieldPerMinute = 0;
    private float lastShield = 100;
    private float timer = 0;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
        if (timer >= 60)
        {
            playerHealthPerMinute = lastHealth - player.Health;
            playerShieldPerMinute = lastShield - player.Shield;
            lastHealth = player.Health;
            lastShield = player.Shield;
        }
	}

    public void SetAvgLifeTime(float lifeTime)
    {
        avgLifeTime = (avgLifeTime + lifeTime) / 2;
    }

    public void SetHitPct(bool hit, WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.Bullet:
                bulletsFired++;
                if(hit)
                {
                    bulletsHit++;
                }
                bulletHitPct = (bulletsHit / bulletsFired) * 100;
                break;
            case WeaponType.Laser:
                lasersFired++;
                if (hit)
                {
                    lasersHit++;
                }
                laserHitPct = (lasersHit / lasersFired) * 100;
                break;
            case WeaponType.Rocket:
                rocketsFired++;
                if (hit)
                {
                    rocketsHit++;
                }
                rocketHitPct = (rocketsHit / rocketsFired) * 100;
                break;
        }
    }

    public void SetPickupPct(bool taken)
    {
        pickupsDropped++;
        if (taken)
        {
            pickupsTaken++;
        }
        pickupTakenPct = (pickupsDropped / pickupsTaken) * 100;
    }

    private void SetDifficultyMultiplier()
    {
        float playerMovementDistance = -1;
        PositionTracker tracker = player.GetComponent<PositionTracker>();
        if (tracker != null)
        {
            playerMovementDistance = tracker.GetDistanceMoved();
        }


    }

    public float GetDifficultyMultiplier()
    {
        return diffMultiplier;
    }
}
