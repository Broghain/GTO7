using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour {

    public static DataManager instance;

    private bool loadedGame = false;
    private GameData data;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SaveGame()
    {
        PlayerController player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
        UpgradeController upgrader = player.GetComponent<UpgradeController>();
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        GameData data = new GameData(player.Health, player.Shield, player.Experience, player.GetLevel(), upgrader.AvailablePoints,
            upgrader.GetRank(UpgradeType.Hull, false), upgrader.GetRank(UpgradeType.Shield, false), upgrader.GetRank(UpgradeType.Bullet, false), upgrader.GetRank(UpgradeType.Laser, false), upgrader.GetRank(UpgradeType.Rocket, false),
            player.transform.position, StatManager.instance.GetScore(), StatManager.instance.GetWaveNumber(), StatManager.instance.GetKillCount());
        
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            data = (GameData)bf.Deserialize(file);
            file.Close();

            loadedGame = true;
            Application.LoadLevel(1);
        }
    }

    public bool LoadAvailable()
    {
        return File.Exists(Application.persistentDataPath + "/playerInfo.dat");
    }

    public bool IsLoaded
    {
        get { return loadedGame; }
        set { loadedGame = value; }
    }

    public GameData GetData()
    {
        return data;
    }
}

[System.Serializable]
public class GameData
{
    private float health;
    private float shield;
    private float experience;
    private int level;
    private int upgradePts;
    private int hullRank;
    private int shieldRank;
    private int bulletRank;
    private int laserRank;
    private int rocketRank;
    private float[] position;
    private int score;
    private int wave;
    private int kills;

    public GameData(float health, float shield, float experience, int level, int upgradePts, int hullRank, int shieldRank, int bulletRank, int laserRank, int rocketRank, Vector3 position, int score, int wave, int kills)
    {
        this.health = health;
        this.shield = shield;
        this.experience = experience;
        
        this.level = level;
        this.upgradePts = upgradePts;
        
        this.hullRank = hullRank;
        this.shieldRank = shieldRank;
        this.bulletRank = bulletRank;
        this.laserRank = laserRank;
        this.rocketRank = rocketRank;
        
        this.position = new float[2];
        this.position[0] = position.x;       
        this.position[1] = position.y;
        
        this.score = score;
        this.wave = wave;
        this.kills = kills;
    }

    public float GetHealth()
    {
        return health;
    }
    public float GetShield()
    {
        return shield;
    }
    public float GetExp()
    {
        return experience;
    }
    public int GetLevel()
    {
        return level;
    }
    public int GetUpgradePts()
    {
        return upgradePts;
    }
    public int GetHullRank()
    {
        return hullRank;
    }
    public int GetShieldRank()
    {
        return shieldRank;
    }
    public int GetBulletRank()
    {
        return bulletRank;
    }
    public int GetLaserRank()
    {
        return laserRank;
    }
    public int GetRocketRank()
    {
        return rocketRank;
    }
    public float[] GetPosition()
    {
        return position;
    }
    public int GetScore()
    {
        return score;
    }
    public int GetWave()
    {
        return wave;
    }
    public int GetKills()
    {
        return kills;
    }
}