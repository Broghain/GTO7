using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField]
    private bool godMode = false;

    [SerializeField]
    private GameObject experiencePickup;

    private Transform playerTransform;

    private bool paused = false;
    private bool gameOver = false;

    private int highScore = 0;
    private int mostKills = 0;
    private int bestWave = 0;

    private int score = 0;
    private int killCount = 0;
    private int waveNumber = 0;

    private bool exceededHighScore = false;
    private bool exceededMostKills = false;
    private bool exceededBestWave = false;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        UIManager.instance.ToggleUpgradePanel();

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        mostKills = PlayerPrefs.GetInt("KillCount", 0);
        bestWave = PlayerPrefs.GetInt("WaveNum", 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Cancel") && (!UIManager.instance.UpgradePanelOpen() || gameOver))
        {
            UIManager.instance.TogglePausePanel();
        }
	}

    public void TogglePause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            paused = true;
        }
        
    }

    public void GameOver()
    {
        if (!godMode)
        {
            gameOver = true;
            UIManager.instance.EnableGameOverPanel();
        }
    }

    public void QuitToMenu()
    {
        if (paused)
        {
            TogglePause();
        }
        Application.LoadLevel(0);
    }

    public void RestartGame()
    {
        if (paused)
        {
            TogglePause();
        }
        Application.LoadLevel(1);
    }

    public bool GetPaused()
    {
        return paused;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public Transform GetPlayer()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
        return playerTransform;
    }

    public EnemyBehaviour[] GetEnemiesOfType(EnemyType type)
    {
        EnemyBehaviour[] enemies = null;
        switch (type)
        {
            case EnemyType.Drone: enemies = FindObjectsOfType<DroneController>();
                break;
            case EnemyType.Fighter: enemies = FindObjectsOfType<FighterController>();
                break;
            case EnemyType.Bomber: enemies = FindObjectsOfType<BomberController>();
                break;
            case EnemyType.Gunner: enemies = FindObjectsOfType<GunnerController>();
                break;
        }
        return enemies;
    }

    public void DropExperiencePickup(Vector3 position)
    {
        Instantiate(experiencePickup, position, Quaternion.identity);
    }

    public void IncreaseWaveNumber()
    {
        waveNumber++;
        if (waveNumber > bestWave)
        {
            exceededBestWave = true;
        }
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public void IncreaseKillCount()
    {
        killCount++;
        if (killCount > mostKills)
        {
            exceededMostKills = true;
        }
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        if (score > highScore)
        {
            exceededHighScore = true;
        }
    }

    public int GetScore()
    {
        return score;
    }

    void OnDestroy()
    {
        if (exceededHighScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        if (exceededMostKills)
        {
            PlayerPrefs.SetInt("KillCount", killCount);
        }
        if (exceededBestWave)
        {
            PlayerPrefs.SetInt("WaveNum", waveNumber);
        }
    }
}
