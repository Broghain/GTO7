using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BezierSpline))]
public class Swarmspawner : MonoBehaviour {

    [SerializeField]
    private float swarmInterval = 5.0f;
    private float timer = 0.0f;

    [SerializeField]
    private int swarmSize = 25;
    private int swarmIndex = 0;

    [SerializeField]
    private GameObject spawnedObjectPrefab = null;

    [SerializeField]
    private ObjectPooler objectPool = null;

    private BezierSpline spline;

	// Use this for initialization
	void Start () 
    {
        spline = GetComponent<BezierSpline>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        timer += Time.deltaTime;
        if (timer >= swarmInterval)
        {
            if (timer - swarmInterval > Random.Range(0.1f, 0.5f))
            {
                if (spawnedObjectPrefab != null && swarmIndex <= swarmSize)
                {
                    Instantiate(spawnedObjectPrefab);
                    swarmIndex++;
                }
                else if (objectPool != null && swarmIndex <= swarmSize)
                {
                    GameObject obj = objectPool.GetNextDynamicInstance();
                    obj.name = objectPool.GetPooledPrefab().name;
                    obj.GetComponent<EnemyBehaviour>().Reset();
                    obj.SetActive(true);
                    obj.GetComponent<SplineTracer>().SetTrack(spline, 0, false);
                    spline.AddTracer(obj.GetComponent<SplineTracer>());
                    swarmIndex++;
                }
                else
                {
                    timer = 0.0f;
                    swarmIndex = 0;
                    Randomize(0.5f, 2.0f);
                }
            }
        }
	}

    public void SetInterval(float interval)
    {
        swarmInterval = interval;
    }

    public void SetPrefab(GameObject prefab)
    {
        spawnedObjectPrefab = prefab;
    }

    public void SetPool(ObjectPooler pool)
    {
        objectPool = pool;
    }

    public void SetSwarmSize(int size)
    {
        swarmSize = size;
    }

    public void Randomize(float minMultiplier, float maxMultiplier)
    {
        swarmInterval = Random.Range(swarmInterval * minMultiplier, swarmInterval * maxMultiplier);
        swarmSize = (int)Random.Range(swarmSize * minMultiplier, swarmSize * maxMultiplier);
    }
}
