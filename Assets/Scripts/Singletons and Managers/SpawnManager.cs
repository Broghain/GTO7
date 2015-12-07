using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private List<EnemyBehaviour> enemyTypes = new List<EnemyBehaviour>();

    public List<GameObject> spawnList;

    [SerializeField]
    private float startSpawnBudget = 20; //Points available for the first wave
    private float spawnBudget = 20; //Points available for spending on next wave

    //private float waveNumber = 0;
    public int enemyCount = 0; //Enemies in wave
    //private int enemiesOnScreen = 0; //Enemies currently on screen

    [SerializeField]
    private float timeBetweenWaves = 2.0f;
    private float waveSpawnTimer = 0;

	// Use this for initialization
	void Start () 
    {
        spawnBudget = startSpawnBudget;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (enemyCount == 0)
        {
            Debug.Log("spawn");
            waveSpawnTimer += Time.deltaTime;
            if (waveSpawnTimer >= timeBetweenWaves)
            {
                waveSpawnTimer = 0;
                SpawnWave();
            }
        }
	}

    private void SpawnWave()
    {
        spawnList = new List<GameObject>();
        while (spawnBudget > 0)
        {
            int randomIndex = Random.Range(0, enemyTypes.Count);
            Debug.Log(randomIndex);
            EnemyBehaviour enemy = enemyTypes[randomIndex];
            if (CheckCost(enemy))
            {
                GameObject enemyObj = enemy.gameObject;
                spawnList.Add(enemyObj);
                enemyCount++;
                spawnBudget -= enemy.GetSpawnValue();
            }
        }
        foreach (GameObject obj in spawnList)
        {
            Instantiate(obj, new Vector3(0, 10, 0), Quaternion.identity);
        }
    }

    private bool CheckCost(EnemyBehaviour enemy)
    {
        if (enemy.GetSpawnValue() <= spawnBudget)
        {
            return true;
        }
        return false;
    }
}
