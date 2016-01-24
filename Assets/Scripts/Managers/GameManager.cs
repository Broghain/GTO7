using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField]
    private bool cheatsEnabled = false;
    private bool godMode = false;

    [SerializeField]
    private GameObject experiencePickup;
    [SerializeField]
    private GameObject healthPickup;

    private Transform playerTransform;

    private bool paused = false;
    private bool gameOver = false;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        StatManager.instance.Reset();
        if (DataManager.instance.IsLoaded)
        {
            GameData data = DataManager.instance.GetData();
            StatManager.instance.IncreaseScore(data.GetScore());
            StatManager.instance.IncreaseWaveNumber(data.GetWave()-1);
            StatManager.instance.IncreaseKillCount(data.GetKills());
        }
        else
        {
            UIManager.instance.ToggleUpgradePanel();
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.F11))
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                godMode = !godMode;
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                playerTransform.GetComponent<UpgradeController>().AvailablePoints++;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StatManager.instance.IncreaseScore(1000);
            }
        }

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

    public void SaveGame()
    {
        DataManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        if (paused)
        {
            TogglePause();
        }
        DataManager.instance.LoadGame();
    }

    public void RestartGame()
    {
        DataManager.instance.IsLoaded = false;
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

    public GameObject DropExperiencePickup(Vector3 position)
    {
        return (GameObject)Instantiate(experiencePickup, position, Quaternion.identity);
    }

    public GameObject DropHealthPickup(Vector3 position)
    {
        return (GameObject)Instantiate(healthPickup, position, Quaternion.identity);
    }

    void OnDestroy()
    {
        StatManager.instance.CheckStats();
    }
}
