using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(ObjectPooler))]
public class WeaponController : MonoBehaviour {

    [SerializeField]
    private float projectileInterval = 1.0f;
    private float projectileTimer = 0.0f;

    [SerializeField]
    private float inaccuracy = 0.0f;

    private ObjectPooler objectPooler;

	// Use this for initialization
	void Start () {
        projectileTimer = projectileInterval;

        objectPooler = GetComponent<ObjectPooler>();
	}

	// Update is called once per frame
	void Update () {
        projectileTimer += Time.deltaTime;
        if (Input.GetButton("Fire1") && projectileTimer >= projectileInterval)
        {
            projectileTimer = 0;

            GameObject projectile = objectPooler.GetNextInstance();

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
            }
        }
	}
}
