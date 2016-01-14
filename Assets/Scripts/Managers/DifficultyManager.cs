using UnityEngine;
using System.Collections;

public class DifficultyManager : MonoBehaviour {

    public static DifficultyManager instance;

    //multiplies stats for increased/decreased difficulty (min 0.5, max 2.0)
    private float diffMultiplier = 1; 

    //average lifetime of enemy ships in seconds
    private float avgLifeTime = 10.0f;

    //Percentage of pickups taken
    private float pickupTakenPct;
   
    private int pickupsDropped = 0;
    private int pickupsTaken = 0;

    //player
    private PlayerController player;

    //player projectile hits
    private float bulletHitPct;
    private float laserHitPct;
    private float rocketHitPct;

    private int bulletsFired = 0;
    private int lasersFired = 0;
    private int rocketsFired = 0;
    private int bulletsHit = 0;
    private int lasersHit = 0;
    private int rocketsHit = 0;

    //player health variation per minute
    private float playerHealthPerMinute;
    private float timer;

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

	}

    public void SetAvgLifeTime(float lifeTime)
    {
        avgLifeTime = (avgLifeTime + lifeTime) / 2;
        Debug.Log(avgLifeTime);
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

    public float GetDifficultyMultiplier()
    {
        return diffMultiplier;
    }
}
