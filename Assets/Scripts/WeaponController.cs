using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {

    [SerializeField]
    private GameObject[] spawners = null;

    [SerializeField]
    private GameObject projectilePrefab = null;

    [SerializeField]
    private float projectileInterval = 1.0f;
    private float projectileTimer = 0.0f;

    [SerializeField]
    private float inaccuracy = 0.0f;

    [SerializeField]
    private int instanceCount = 1;

    private List<GameObject> instances = new List<GameObject>();
    private int instanceIndex = 0;

    private GameObject projectileParent;

	// Use this for initialization
	void Start () {
        projectileTimer = projectileInterval;

        projectileParent = new GameObject();
        projectileParent.name = projectilePrefab.name + "Container";

        for(int i = 0; i < instanceCount; i++)
        {
            instances.Add((GameObject)Instantiate(projectilePrefab, new Vector3(1000,1000,1000), Quaternion.identity));
            instances[i].SetActive(false);
            instances[i].name = "Unused" + projectilePrefab.name;
            instances[i].GetComponent<ProjectileDestroyer>().SetParent(projectileParent.transform);
        }
	}
	
	// Update is called once per frame
	void Update () {
        projectileTimer += Time.deltaTime;
        if (Input.GetButton("Fire1") && projectileTimer >= projectileInterval)
        {
            foreach (GameObject spawner in spawners)
            {
                if (instances[instanceIndex].activeSelf)
                {
                    instances.Insert(instanceIndex, (GameObject)Instantiate(projectilePrefab, new Vector3(1000, 1000, 1000), Quaternion.identity));
                    instances[instanceIndex].GetComponent<ProjectileDestroyer>().SetParent(projectileParent.transform);
                    instanceCount++;
                }
                GameObject projectile = instances[instanceIndex];
                
                float accuracy = Random.Range(-inaccuracy, inaccuracy);
                projectile.transform.up = spawner.transform.up + (spawner.transform.right * accuracy);
                projectile.name = projectilePrefab.name;
                projectile.transform.position = spawner.transform.position;
                projectile.SetActive(true);
                
                instanceIndex++;
                if (instanceIndex >= instanceCount)
                {
                    instanceIndex = 0;
                }
            }
            projectileTimer = 0;
        }
	}
}
