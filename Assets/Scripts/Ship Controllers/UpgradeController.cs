using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UpgradeType
{
    Hull,
    Shield,
    Bullet,
    Laser,
    Rocket,
    All
}

[RequireComponent(typeof(PlayerController))]
public class UpgradeController : MonoBehaviour {

    private int availablePoints = 3;

    private int addHullPts;
    private int addShieldPts;
    private int addBulletPts;
    private int addLaserPts;
    private int addRocketPts;

    private int hullRank = 1;
    private int shieldRank = 1;
    private int bulletRank = 0;
    private int laserRank = 0;
    private int rocketRank = 0;

    [System.Serializable]
    private class WeaponUnlock
    {
        [SerializeField]
        private int requiredRank;
        [SerializeField]
        private WeaponController weapon;

        public int RequiredRank
        {
            get { return requiredRank; }
        }

        public WeaponController Weapon
        {
            get { return weapon; }
        }
    }

    [SerializeField]
    private List<WeaponUnlock> bulletWeaponUnlocks;
    [SerializeField]
    private List<WeaponUnlock> rocketWeaponUnlocks;
    [SerializeField]
    private List<WeaponUnlock> laserWeaponUnlocks;

    [SerializeField]
    private ProjectileController playerBulletPrefab;
    [SerializeField]
    private ProjectileController playerRocketPrefab;
    [SerializeField]
    private LaserController playerLaserPrefab;

    private PlayerController player;

	// Use this for initialization
	void Start () 
    {
        player = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    //Available upgrade points
    public int AvailablePoints
    {
        get { return availablePoints; }
        set { availablePoints = value; }
    }

    //If any upgrade type is higher than 0 (changes were made), returns true
    //Else, return false
    public bool StatsChanged(UpgradeType upgrade)
    {
        bool value = false;
        switch (upgrade)
        {
            case UpgradeType.Hull: value = addHullPts > 0;
                break;
            case UpgradeType.Shield: value = addShieldPts > 0;
                break;
            case UpgradeType.Bullet: value = addBulletPts > 0;
                break;
            case UpgradeType.Laser: value = addLaserPts > 0;
                break;
            case UpgradeType.Rocket: value = addRocketPts > 0;
                break;
            case UpgradeType.All: value = addHullPts > 0 || addShieldPts > 0 || addBulletPts > 0 || addLaserPts > 0 || addRocketPts > 0;
                break;
        }
        return value; 
    }

    public void ApplyUpgrade()
    {
        hullRank += addHullPts;
        shieldRank += addShieldPts;
        bulletRank += addBulletPts;
        laserRank += addLaserPts;
        rocketRank += addRocketPts;

        player.MaxHealth += player.MaxHealth * (hullRank * 0.5f);
        player.MaxShield += player.MaxShield * (shieldRank * 0.25f);
        player.Health = player.MaxHealth;
        player.Shield = player.MaxShield;

        //unlock weapons
        foreach (WeaponUnlock unlock in bulletWeaponUnlocks)
        {
            if (bulletRank >= unlock.RequiredRank)
            {
                unlock.Weapon.gameObject.SetActive(true);
            }
        }
        foreach (WeaponUnlock unlock in laserWeaponUnlocks)
        {
            if (laserRank >= unlock.RequiredRank)
            {
                unlock.Weapon.gameObject.SetActive(true);
            }
        }
        foreach (WeaponUnlock unlock in rocketWeaponUnlocks)
        {
            if (rocketRank >= unlock.RequiredRank)
            {
                unlock.Weapon.gameObject.SetActive(true);
            }
        }

        UpgradeWeapons(addBulletPts, playerBulletPrefab, WeaponType.Bullet);
        UpgradeWeapons(addRocketPts, playerRocketPrefab, WeaponType.Rocket);

        addHullPts = 0;
        addShieldPts = 0;
        addBulletPts = 0;
        addLaserPts = 0;
        addRocketPts = 0;
    }

    private void UpgradeWeapons(int iterationCount, ProjectileController projectilePrefab, WeaponType weaponType)
    {
        //upgrade weapons and projectiles for each upgrade point
        for (int i = 0; i < iterationCount; i++)
        {
            //increase stats of projectile prefab
            UpgradeProjectile(projectilePrefab);

            foreach (WeaponController weapon in player.GetWeapons())
            {
                if (weapon.GetWeaponType() == weaponType)
                {
                    //increase weapon stats
                    float intervalPct = (weapon.Interval / weapon.GetMaxInterval()) * 100;
                    float newInterval = weapon.Interval - (intervalPct / 200);
                    weapon.Interval = newInterval;

                    ObjectPooler pooler = weapon.GetComponent<ObjectPooler>();
                    if (pooler != null)
                    {
                        //increase stats of pooled projectiles
                        foreach (GameObject obj in pooler.GetPooledObjects())
                        {
                            ProjectileController projectile = obj.GetComponent<ProjectileController>();
                            UpgradeProjectile(projectile);
                        }
                    }
                }
            }
        }
    }

    private void UpgradeWeapons(int iterationCount, LaserController laserPrefab, WeaponType weaponType)
    {
        //upgrade weapons and projectiles for each upgrade point
        for (int i = 0; i < iterationCount; i++)
        {
            //increase stats of projectile prefab
            UpgradeLaser(laserPrefab);

            foreach (WeaponController weapon in player.GetWeapons())
            {
                if (weapon.GetWeaponType() == weaponType)
                {
                    //increase weapon stats
                    float intervalPct = (weapon.Interval / weapon.GetMaxInterval()) * 100;
                    float newInterval = weapon.Interval - (intervalPct / 200);
                    weapon.Interval = newInterval;

                    ObjectPooler pooler = weapon.GetComponent<ObjectPooler>();
                    if (pooler != null)
                    {
                        //increase stats of pooled projectiles
                        foreach (GameObject obj in pooler.GetPooledObjects())
                        {
                            LaserController laser = obj.GetComponent<LaserController>();
                            UpgradeLaser(laser);
                        }
                    }
                }
            }
        }
    }

    private void UpgradeProjectile(ProjectileController projectile)
    {
        float damagePct = (projectile.Damage / projectile.GetMaxDmg()) * 100;
        float newDamage = projectile.Damage + (damagePct / 10);
        projectile.Damage = newDamage;

        ObjectChaser chaser = projectile.GetComponent<ObjectChaser>();
        if (chaser != null)
        {
            //increase stats of object chaser
            float delayPct = (chaser.Delay / chaser.GetMaxDelay()) * 100;
            float newDelay = chaser.Delay - (delayPct / 75);
            chaser.Delay = newDelay;

            float precisionPct = (chaser.Precision / chaser.GetMaxPrecision()) * 100;
            float newPrecision = chaser.Precision + (precisionPct / 75);
            chaser.Precision = newPrecision;
        }
    }

    private void UpgradeLaser(LaserController laser)
    {
        float damagePct = (laser.Damage / laser.GetMaxDmg()) * 100;
        float newDamage = laser.Damage + (damagePct / 10);
        laser.Damage = newDamage;

        TimeToLive ttl = laser.GetComponent<TimeToLive>();
        if (ttl != null)
        {
            float ttlPct = (ttl.TTL/ ttl.GetMaxTTL()) * 100;
            float newTTL = ttl.TTL + (ttlPct / 60);
            ttl.TTL = newTTL;
        }
    }

    public void UndoUpgrade()
    {
        addHullPts = 0;
        addShieldPts = 0;
        addBulletPts = 0;
        addLaserPts = 0;
        addRocketPts = 0;
    }

    public void ChangePts(UpgradeType upgrade, int amount)
    {
        switch (upgrade)
        {
            case UpgradeType.Hull: addHullPts += amount;
                break;
            case UpgradeType.Shield: addShieldPts += amount;
                break;
            case UpgradeType.Bullet: addBulletPts += amount;
                break;
            case UpgradeType.Laser: addLaserPts += amount;
                break;
            case UpgradeType.Rocket: addRocketPts += amount;
                break;
        }
    }

    public int GetRank(UpgradeType upgrade, bool includeUnapplied)
    {
        int rank = 0;
        switch (upgrade)
        {
            case UpgradeType.Hull: rank = hullRank;
                if (includeUnapplied)
                {
                    rank += addHullPts;
                }
                break;
            case UpgradeType.Shield: rank = shieldRank;
                if (includeUnapplied)
                {
                    rank += addShieldPts;
                }
                break;
            case UpgradeType.Bullet: rank = bulletRank;
                if (includeUnapplied)
                {
                    rank += addBulletPts;
                }
                break;
            case UpgradeType.Laser: rank = laserRank;
                if (includeUnapplied)
                {
                    rank += addLaserPts;
                }
                break;
            case UpgradeType.Rocket: rank = rocketRank;
                if (includeUnapplied)
                {
                    rank += addRocketPts;
                }
                break;
        }
        return rank;
    }

    //returns true if the player has a weapon at rank 1 or higher or is unlocking a weapon (to prevent starting game without weapon)
    public bool HasWeapon(bool includeUpgrade)
    {
        if (includeUpgrade)
        {
            return bulletRank > 0 || laserRank > 0 || rocketRank > 0 || addBulletPts > 0 || addLaserPts > 0 || addRocketPts > 0;
        }
        else
        {
            return bulletRank > 0 || laserRank > 0 || rocketRank > 0;
        }
    }
}
