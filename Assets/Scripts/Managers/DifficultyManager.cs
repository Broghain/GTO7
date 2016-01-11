using UnityEngine;
using System.Collections;

public class DifficultyManager : MonoBehaviour {

    public static DifficultyManager instance;

    //multiplies npc stats for increased/decreased difficulty
    private float statMultiplier = 1; 

    //average lifetime of enemy ships
    private float avgLifeTime;

    //player
    private PlayerController player;

    //player projectile hit percentages
    private float bulletHitPct;
    private float laserHitPct;
    private float rocketHitPct;

    //player health variation per minute
    private float playerHealthPerMinute;
    private float timer;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    public float GetStatMultiplier()
    {
        return statMultiplier;
    }
}
