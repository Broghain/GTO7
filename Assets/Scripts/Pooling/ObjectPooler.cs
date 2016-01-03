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
	void Start () 
    {
        CreatePool();
	}

    private void CreatePool()
    {
        pooledObjectParent = GameObject.Find(pooledObjectPrefab.name + "Container");
        if (pooledObjectParent == null)
        {
            pooledObjectParent = new GameObject();
            pooledObjectParent.name = pooledObjectPrefab.name + "Container";
        }

        if (instances.Count > 0)
        {
            instances = new List<GameObject>();
        }

        for (int i = 0; i < instanceCount; i++)
        {
            instances.Add((GameObject)Instantiate(pooledObjectPrefab, new Vector3(1000, 1000, 1000), pooledObjectPrefab.transform.rotation));
            instances[i].SetActive(false);
            instances[i].name = "Unused" + pooledObjectPrefab.name;
            instances[i].GetComponent<PooledObjectBehaviour>().SetParent(pooledObjectParent.transform);
        }
    }

    public GameObject GetNextDynamicInstance()
    {
        if (pooledObjectParent == null)
        {
            CreatePool();
        }

        if (instances[instanceIndex].activeSelf && !FindInactiveInstance()) //is every instance already active?
        {
            //create new instance
            instances.Insert(instanceIndex, (GameObject)Instantiate(pooledObjectPrefab, new Vector3(1000, 1000, 1000), pooledObjectPrefab.transform.rotation));
            instances[instanceIndex].GetComponent<PooledObjectBehaviour>().SetParent(pooledObjectParent.transform);
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

    public GameObject GetNextInstance(bool overrideActive)
    {
        if (pooledObjectParent == null)
        {
            CreatePool();
        }


        if (instances[instanceIndex].activeSelf && !FindInactiveInstance()) //is every instance already active?
        {
            if (!overrideActive) //should active instances persist?
            {
                //return null
                return null;
            }

        }

        GameObject instance = instances[instanceIndex];

        instanceIndex++;
        if (instanceIndex >= instanceCount)
        {
            instanceIndex = 0;
        }

        return instance;
    }

    //returns true if inactive instance is found, false if every instance is active
    private bool FindInactiveInstance()
    {
        //look for inactive instance
        for (int i = instanceIndex + 1; i == instanceIndex; i++)
        {
            if (i >= instances.Count)
            {
                i = 0;
            }

            if (!instances[i].activeSelf)
            {
                instanceIndex = i;
                return true;
            }
        }
        return false;
    }

    public GameObject GetPooledPrefab()
    {
        return pooledObjectPrefab;
    }

    public List<GameObject> GetPooledObjects()
    {
        return instances;
    }
}
