using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum WeaponType
{
    Bullet,
    Laser,
    Rocket,
    Other
}

[RequireComponent(typeof(ObjectPooler))]
public class WeaponController : MonoBehaviour {

    [SerializeField]
    private float projectileInterval = 1.0f;
    private float projectileTimer = 0.0f;

    [SerializeField]
    private float inaccuracy = 0.0f;

    [SerializeField]
    private WeaponType weaponType = WeaponType.Other;

    private ObjectPooler objectPooler;

    public float Interval
    {
        get { return projectileInterval; }
        set { projectileInterval = value; }
    }

	// Use this for initialization
	void Start () {
        projectileTimer = projectileInterval;

        objectPooler = GetComponent<ObjectPooler>();
	}

    public void Shoot()
    {
        if (projectileTimer >= projectileInterval)
        {
            projectileTimer = 0;

            GameObject projectile = objectPooler.GetNextDynamicInstance();

            float accuracy = Random.Range(-inaccuracy, inaccuracy);
            projectile.transform.up = transform.up + (transform.right * accuracy);
            projectile.name = objectPooler.GetPooledPrefab().name;
            projectile.transform.position = transform.position;
            projectile.SetActive(true);

            TrailResetter[] trailResetters = projectile.GetComponentsInChildren<TrailResetter>();
            foreach (TrailResetter resetter in trailResetters)
            {
                resetter.Reset();
            }

            LaserController laser = projectile.GetComponent<LaserController>();
            if (laser != null)
            {
                laser.SetStartObject(this.gameObject);
                AudioManager.instance.PlaySoundWithRandomPitch(laser.GetShotClip(), 0.5f, 1.5f);
            }

            ProjectileController proj = projectile.GetComponent<ProjectileController>();
            if (proj != null)
            {
                AudioManager.instance.PlaySoundWithRandomPitch(proj.GetShotClip(), 0.75f, 1.25f);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        projectileTimer += Time.deltaTime;
	}

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }
}
