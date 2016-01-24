using UnityEngine;
using System.Collections;

public class StatManager : MonoBehaviour {

    public static StatManager instance;

    private int score = 0;
    private int killCount = 0;
    private int waveNumber = 0;

    public int highScore = 0;
    public int mostKills = 0;
    public int bestWave = 0;

    private bool exceededHighScore = false;
    private bool exceededMostKills = false;
    private bool exceededBestWave = false;

    void Awake()
    {
        if (StatManager.instance != null && StatManager.instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () 
    {
        LoadStats();
	}

    public void LoadStats()
    {
        if (APIManager.instance.GetSignedIn())
        {
            APIManager.instance.GetStat("HighScore");
            APIManager.instance.GetStat("KillCount");
            APIManager.instance.GetStat("WaveNum");
        }
        else
        {
            int localScore = PlayerPrefs.GetInt("HighScore", 0);
            int localKills = PlayerPrefs.GetInt("KillCount", 0);
            int localWave = PlayerPrefs.GetInt("WaveNum", 0);

            highScore = localScore;
            mostKills = localKills;
            bestWave = localWave;
        }
    }

    public void LoadOnlineStat(int onlineStat, string statname)
    {
        int localStat = PlayerPrefs.GetInt(statname, 0);
        int highestStat = localStat;

        if (localStat > onlineStat) //if local stat is HIGHER than online stat
        {
            APIManager.instance.SetStat(localStat, statname); //set online value
        }
        else if (localStat <= onlineStat) //if local stat is LOWER than online stat
        {
            PlayerPrefs.SetInt(statname, onlineStat); //set local value
            highestStat = onlineStat;
        }

        switch(statname)
        {
            case "HighScore": highScore = highestStat;
                break;
            case "KillCount": mostKills = highestStat;
                break;
            case "WaveNum": bestWave = highestStat;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.F11))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetHighScores();
                ResetTrophies();
            }
        }
	}

    public void IncreaseWaveNumber()
    {
        waveNumber++;
        CheckWaveAchievement();
        if (waveNumber > bestWave)
        {
            exceededBestWave = true;
        }
    }

    public void IncreaseWaveNumber(int amount)
    {
        waveNumber += amount;
        CheckWaveAchievement();
        if (waveNumber > bestWave)
        {
            exceededBestWave = true;
        }
    }

    private void CheckWaveAchievement()
    {
        PlayerController player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
        switch (waveNumber)
        {
            case 2:
                if (!player.PlayerWasHit())
                {
                    APIManager.instance.UnlockTrophy(48202); //Survive wave 1 without getting hit
                }
                break;
            case 5:
                APIManager.instance.UnlockTrophy(48199); //Reached wave 5
                break;
            case 6:
                if (!player.PlayerWasHit())
                {
                    APIManager.instance.UnlockTrophy(48206); //Survive wave 5 without getting hit
                }
                break;
            case 10:
                APIManager.instance.UnlockTrophy(48203); //Reached wave 10
                break;
            case 11:
                if (!player.PlayerWasHit())
                {
                    APIManager.instance.UnlockTrophy(48206); //Survive wave 10 without getting hit
                }
                break;
            case 15:
                APIManager.instance.UnlockTrophy(49207); //Reached wave 15
                break;
            case 20:
                APIManager.instance.UnlockTrophy(48211); //Reached wave 20
                break;
        }
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public void IncreaseKillCount()
    {
        killCount++;
        CheckKillAchievement();
        if (killCount > mostKills)
        {
            exceededMostKills = true;
        }
    }

    public void IncreaseKillCount(int amount)
    {
        killCount += amount;
        CheckKillAchievement();
        if (killCount > mostKills)
        {
            exceededMostKills = true;
        }
    }

    private void CheckKillAchievement()
    {
        PlayerController player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
        if (killCount == 1)
        {
            if (!player.PlayerWasHit())
            {
                APIManager.instance.UnlockTrophy(48198); //Kill first enemy without getting hit
            }
        }
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        CheckScoreAchievement();
        if (score > highScore)
        {
            exceededHighScore = true;
        }
    }

    private void CheckScoreAchievement()
    {
        PlayerController player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
        if (score >= 1000)
        {
            APIManager.instance.UnlockTrophy(48197); //Get 1000 score
        }
        if (score >= 10000)
        {
            APIManager.instance.UnlockTrophy(48201); //Get 10000 score
        }
        if (score >= 100000)
        {
            APIManager.instance.UnlockTrophy(48205); //Get 100000 score
        }
        if (score >= 1000000)
        {
            APIManager.instance.UnlockTrophy(48209); //Get 1000000 score
        }
    }

    public void CheckUpgradeAchievement()
    {
        UpgradeController upgrader = GameManager.instance.GetPlayer().GetComponent<UpgradeController>();
        if (upgrader.GetRank(UpgradeType.Bullet, false) >= 5 || upgrader.GetRank(UpgradeType.Rocket, false) >= 5 || upgrader.GetRank(UpgradeType.Laser, false) >= 5)
        {
            APIManager.instance.UnlockTrophy(48200); //Upgraded weapon to rank 5
        }
        if (upgrader.GetRank(UpgradeType.Bullet, false) >= 10 || upgrader.GetRank(UpgradeType.Rocket, false) >= 10 || upgrader.GetRank(UpgradeType.Laser, false) >= 10)
        {
            APIManager.instance.UnlockTrophy(48204); //Upgraded weapon to rank 10
        }
        if (upgrader.GetRank(UpgradeType.Bullet, false) >= 10 && upgrader.GetRank(UpgradeType.Rocket, false) >= 10 && upgrader.GetRank(UpgradeType.Laser, false) >= 10)
        {
            APIManager.instance.UnlockTrophy(48208); //Upgraded all weapons to rank 10
        }
        if (upgrader.GetRank(UpgradeType.Bullet, false) >= 10 && upgrader.GetRank(UpgradeType.Rocket, false) >= 10 && upgrader.GetRank(UpgradeType.Laser, false) >= 10 && upgrader.GetRank(UpgradeType.Hull, false) >= 10 && upgrader.GetRank(UpgradeType.Shield, false) >= 10)
        {
            APIManager.instance.UnlockTrophy(48212); //Upgraded all upgrade types to rank 10
        }
    }

    public int GetScore()
    {
        return score;
    }

    public void CheckStats()
    {
        if (exceededHighScore)
        {
            highScore = score;
            SaveStat("HighScore", score);
        }
        if (exceededMostKills)
        {
            mostKills = killCount;
            SaveStat("KillCount", killCount);
        }
        if (exceededBestWave)
        {
            bestWave = waveNumber;
            SaveStat("WaveNum", waveNumber);
        }
    }

    private void SaveStat(string statname, int statValue)
    {
        PlayerPrefs.SetInt(statname, statValue);
        PlayerPrefs.Save();
        if (APIManager.instance.GetSignedIn()) //if signed in
        {
            APIManager.instance.SetStat(statValue, statname); //set online value
        }
    }

    public int GetHighScore()
    {
        return highScore;
    }
    public int GetMostKills()
    {
        return mostKills;
    }
    public int GetBestWave()
    {
        return bestWave;
    }

    public void Reset()
    {
        score = 0;
        killCount = 0;
        waveNumber = 0;
    }

    private void ResetHighScores()
    {
        highScore = 0;
        SaveStat("HighScore", 0);
        mostKills = 0;
        SaveStat("KillCount", 0);
        bestWave = 0;
        SaveStat("WaveNum", 0);
    }

    private void ResetTrophies()
    {
        APIManager.instance.LockTrophy(48197);
        APIManager.instance.LockTrophy(48198);
        APIManager.instance.LockTrophy(48199);
        APIManager.instance.LockTrophy(48200);

        APIManager.instance.LockTrophy(48201);
        APIManager.instance.LockTrophy(48202);
        APIManager.instance.LockTrophy(48203);
        APIManager.instance.LockTrophy(48204);

        APIManager.instance.LockTrophy(48205);
        APIManager.instance.LockTrophy(48206);
        APIManager.instance.LockTrophy(48207);
        APIManager.instance.LockTrophy(48208);

        APIManager.instance.LockTrophy(48209);
        APIManager.instance.LockTrophy(48210);
        APIManager.instance.LockTrophy(48211);
        APIManager.instance.LockTrophy(48212);
    }
}
