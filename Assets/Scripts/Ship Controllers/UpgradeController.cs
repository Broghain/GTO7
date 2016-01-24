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
        
        if (DataManager.instance.IsLoaded) //did we load a save?
        {
            GameData data = DataManager.instance.GetData(); //get saved data
            availablePoints = data.GetUpgradePts();
            addHullPts = data.GetHullRank() - 1; //-1 because default value is 1
            addShieldPts = data.GetShieldRank() - 1; //-1 because default value is 1
            addBulletPts = data.GetBulletRank();
            addLaserPts = data.GetLaserRank();
            addRocketPts = data.GetRocketRank();
            ApplyUpgrade();
        }
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
        UpgradeWeapons(addRocketPts, playerLaserPrefab, WeaponType.Laser);
        UpgradeHealth(addHullPts);
        UpgradeShield(addShieldPts);

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
            UpgradeProjectile(projectilePrefab, weaponType);

            foreach (WeaponController weapon in player.GetWeapons())
            {
                if (weapon.GetWeaponType() == weaponType)
                {
                    //increase weapon stats
                    if (GetWeaponRank(weaponType) < 5)
                    {
                        weapon.Interval /= 1.25f;
                    }
                    else if (GetWeaponRank(weaponType) < 10)
                    {
                        weapon.Interval /= 1.15f;
                    }
                    else
                    {
                        weapon.Interval /= 1.05f;
                    }

                    ObjectPooler pooler = weapon.GetComponent<ObjectPooler>();
                    if (pooler != null)
                    {
                        //increase stats of pooled projectiles
                        foreach (GameObject obj in pooler.GetPooledObjects())
                        {
                            ProjectileController projectile = obj.GetComponent<ProjectileController>();
                            UpgradeProjectile(projectile, weaponType);
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
                    //increase weapon stats
                    if (GetWeaponRank(weaponType) < 5)
                    {
                        weapon.Interval /= 1.25f;
                    }
                    else if (GetWeaponRank(weaponType) < 10)
                    {
                        weapon.Interval /= 1.15f;
                    }
                    else
                    {
                        weapon.Interval /= 1.05f;
                    }

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

    private void UpgradeProjectile(ProjectileController projectile, WeaponType weaponType)
    {
        if (GetWeaponRank(weaponType) < 5)
        {
            projectile.Damage *= 1.25f;
        }
        else if (GetWeaponRank(weaponType) < 10)
        {
            projectile.Damage *= 1.15f;
        }
        else
        {
            projectile.Damage *= 1.05f;
        }

        ObjectChaser chaser = projectile.GetComponent<ObjectChaser>();
        if (chaser != null)
        {
            //increase weapon stats
            if (GetWeaponRank(weaponType) < 5)
            {
                chaser.Delay /= 1.25f;
                chaser.Precision *= 1.25f;
            }
            else if (GetWeaponRank(weaponType) < 10)
            {
                chaser.Delay /= 1.15f;
                chaser.Precision *= 1.15f;
            }
            else
            {
                chaser.Delay /= 1.05f;
                chaser.Precision *= 1.05f;
            }
        }
    }

    private void UpgradeLaser(LaserController laser)
    {
        if (GetWeaponRank(WeaponType.Laser) < 5)
        {
            laser.Damage *= 1.25f;
        }
        else if (GetWeaponRank(WeaponType.Laser) < 10)
        {
            laser.Damage *= 1.15f;
        }
        else
        {
            laser.Damage *= 1.05f;
        }
    }

    private void UpgradeHealth(int iterationCount)
    {
        float healthPct = (player.Health / player.MaxHealth) * 100;
        for(int i = 0; i < iterationCount; i++)
        {
            player.MaxHealth += player.MaxHealth * 0.75f;
        }
        player.Health = (player.MaxHealth / 100) * healthPct;
    }

    private void UpgradeShield(int iterationCount)
    {
        float shieldPct = (player.Shield / player.MaxShield) * 100;
        for (int i = 0; i < iterationCount; i++)
        {
            player.MaxShield += player.MaxShield * 0.5f;
        }
        player.Shield = (player.MaxShield / 100) * shieldPct;
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

    private int GetWeaponRank(WeaponType weaponType)
    {
        int rank = 0;
        switch (weaponType)
        {
            case WeaponType.Bullet: rank = GetRank(UpgradeType.Bullet, false);
                break;
            case WeaponType.Rocket: rank = GetRank(UpgradeType.Rocket, false);
                break;
            case WeaponType.Laser: rank = GetRank(UpgradeType.Laser, false);
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
