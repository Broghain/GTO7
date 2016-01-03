using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    public static SpawnManager instance;

    [SerializeField]
    private List<EnemyBehaviour> enemyTypes = new List<EnemyBehaviour>();

    [SerializeField]
    private List<BezierSpline> droneSplines = new List<BezierSpline>();
    [SerializeField]
    private ObjectPooler dronePool;

    private ObjectPooler[] enemyPools;

    private List<GameObject> spawnList;

    [SerializeField]
    private float startSpawnBudget = 20; //Points available for the first wave
    private float spawnBudget = 20; //Points available for spending on next wave

    //private float waveNumber = 0;
    private int enemyCount = 0; //Enemies in wave
    //private int enemiesOnScreen = 0; //Enemies currently on screen

    [SerializeField]
    private float timeBetweenWaves = 2.0f;
    private float waveSpawnTimer = 0;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        spawnBudget = startSpawnBudget;
        enemyPools = GetComponents<ObjectPooler>();
        if (dronePool == null)
        {
            foreach (ObjectPooler pool in enemyPools)
            {
                if (pool.GetPooledPrefab().name == "Drone")
                {
                    dronePool = pool;
                    break;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (enemyCount <= 0)
        {
            enemyCount = 0;
            waveSpawnTimer += Time.deltaTime;
            if (waveSpawnTimer >= timeBetweenWaves)
            {
                GameManager.instance.IncreaseWaveNumber();
                spawnBudget = startSpawnBudget * GameManager.instance.GetWaveNumber();
                waveSpawnTimer = 0;
                SpawnWave();
            }
        }
	}

    private void SpawnWave()
    {
        spawnBudget *= DifficultyManager.instance.GetStatMultiplier();
        spawnList = new List<GameObject>();
        int attempts = 0; //attempts to find random enemy to spawn (prevent infinite loop)
        while (spawnBudget > 0)
        {
            int randomIndex = Random.Range(0, enemyTypes.Count);
            EnemyBehaviour enemy = enemyTypes[randomIndex];
            if (CheckCost(enemy) || attempts > enemyTypes.Count)
            {
                GameObject enemyObj = enemy.gameObject;
                spawnList.Add(enemyObj);
                enemyCount++;
                spawnBudget -= enemy.SpawnCost;
                attempts = 0;
            }
            attempts++;
        }
        
        foreach (GameObject obj in spawnList)
        {
            bool objIsPooled = false;
            foreach (ObjectPooler pool in enemyPools)
            {
                if (pool.GetPooledPrefab() == obj)
                {
                    objIsPooled = true;
                    GameObject enemy = pool.GetNextDynamicInstance();
                    enemy.name = pool.GetPooledPrefab().name;
                    enemy.transform.position = new Vector3(Random.Range(-7, 7), 10, 0);
                    enemy.SetActive(true);
                    enemy.GetComponent<EnemyBehaviour>().Reset();
                    break;
                }
                
            }

            if (!objIsPooled)
            {
                Instantiate(obj, new Vector3(0, 10, 0), Quaternion.identity);
            }
        }

        BezierSpline[] deleteSplines = GameObject.FindObjectsOfType<BezierSpline>();
        foreach (BezierSpline spline in deleteSplines)
        {
            spline.SetDestroy(true);
        }

        int splineCount = GameManager.instance.GetWaveNumber();
        splineCount = Mathf.Clamp(splineCount, 1, 5);
        for (int i = 0; i < splineCount; i++)
        {
            BezierSpline spline = droneSplines[Random.Range(0, droneSplines.Count)];

            Vector3 approxPlayerPosition = GameManager.instance.GetPlayer().position;
            approxPlayerPosition += new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f),0);

            Vector3 splineCenterPosition = spline.GetControlPoint((int)spline.GetControlPointCount() / 2);
            Vector3 splinePosition = approxPlayerPosition - splineCenterPosition;
            Instantiate(spline.gameObject, splinePosition, Quaternion.identity);

            Swarmspawner swarmSpawner = spline.GetComponent<Swarmspawner>();
            if (swarmSpawner != null)
            {
                swarmSpawner.SetPool(dronePool);
            }
        }
    }

    private bool CheckCost(EnemyBehaviour enemy)
    {
        if (enemy.SpawnCost <= spawnBudget)
        {
            return true;
        }
        return false;
    }

    public void DecreaseEnemyCount(int amount)
    {
        enemyCount -= amount;
    }
}
