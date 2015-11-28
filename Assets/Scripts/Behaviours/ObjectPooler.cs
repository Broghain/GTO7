using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {

    private GameObject pooledObjectParent;

    [SerializeField]
    private GameObject pooledObjectPrefab;

    [SerializeField]
    private int instanceCount = 1;

    private List<GameObject> instances = new List<GameObject>();
    private int instanceIndex = 0;

	// Use this for initialization
	void Start () {
        GameObject parent = GameObject.Find(pooledObjectPrefab.name + "Container");
        if (parent == null)
        {
            pooledObjectParent = new GameObject();
            pooledObjectParent.name = pooledObjectPrefab.name + "Container";
        }
        else
        {
            pooledObjectParent = parent;
        }

        for (int i = 0; i < instanceCount; i++)
        {
            instances.Add((GameObject)Instantiate(pooledObjectPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity));
            instances[i].SetActive(false);
            instances[i].name = "Unused" + pooledObjectPrefab.name;
            instances[i].GetComponent<PooledObject>().SetParent(pooledObjectParent.transform);
        }
	}

    public GameObject GetNextInstance()
    {
        if (instances[instanceIndex].activeSelf)
        {
            instances.Insert(instanceIndex, (GameObject)Instantiate(pooledObjectPrefab, new Vector3(1000, 1000, 1000), Quaternion.identity));
            instances[instanceIndex].GetComponent<PooledObject>().SetParent(pooledObjectParent.transform);
            instanceCount++;
        }
        GameObject instance = instances[instanceIndex];

        instanceIndex++;
        if (instanceIndex >= instanceCount)
        {
            instanceIndex = 0;
        }

        return instance;
    }

    public GameObject GetPooledPrefab()
    {
        return pooledObjectPrefab;
    }
}
