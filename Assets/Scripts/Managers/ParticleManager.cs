using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {

    public static ParticleManager instance;

    [SerializeField]
    private ObjectPooler hardHitParticlePool;
    [SerializeField]
    private ObjectPooler hitParticlePool;
    [SerializeField]
    private ObjectPooler enemyExplodeParticlePool;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SpawnHardHitParticle(Vector3 position)
    {
        GameObject particle = hardHitParticlePool.GetNextDynamicInstance();
        particle.name = hardHitParticlePool.GetPooledPrefab().name;
        particle.SetActive(true);
        particle.transform.position = position;
    }

    public void SpawnHitParticle(Vector3 position)
    {
        GameObject particle = hitParticlePool.GetNextDynamicInstance();
        particle.name = hitParticlePool.GetPooledPrefab().name;
        particle.SetActive(true);
        particle.transform.position = position;
    }
    
    public void SpawnEnemyExplodeParticle(Vector3 position)
    {
        GameObject particle = enemyExplodeParticlePool.GetNextDynamicInstance();
        particle.name = enemyExplodeParticlePool.GetPooledPrefab().name;
        particle.SetActive(true);
        particle.transform.position = position;
    }
}
