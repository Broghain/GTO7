using UnityEngine;
using System.Collections;

public class StatManager : MonoBehaviour {

    public static StatManager instance;

    private int score = 0;
    private int killCount = 0;
    private int waveNumber = 0;

    private int highScore = 0;
    private int mostKills = 0;
    private int bestWave = 0;

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
	
	}

    public void IncreaseWaveNumber()
    {
        waveNumber++;
        CheckWaveAchievement();
    }

    private void CheckWaveAchievement()
    {
        PlayerController player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
        switch (waveNumber)
        {
            case 1:
                if (!player.PlayerWasHit())
                {
                    APIManager.instance.UnlockTrophy(1); //Survive wave 1 without getting hit
                }
                break;
            case 5:
                APIManager.instance.UnlockTrophy(4); //Reached wave 5
                break;
            case 6:
                if (!player.PlayerWasHit())
                {
                    APIManager.instance.UnlockTrophy(2); //Survive wave 5 without getting hit
                }
                break;
            case 10:
                APIManager.instance.UnlockTrophy(5); //Reached wave 10
                break;
            case 11:
                if (!player.PlayerWasHit())
                {
                    APIManager.instance.UnlockTrophy(3); //Survive wave 10 without getting hit
                }
                break;
            case 15:
                APIManager.instance.UnlockTrophy(6); //Reached wave 15
                break;
            case 20:
                APIManager.instance.UnlockTrophy(7); //Reached wave 20
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

    private void CheckKillAchievement()
    {
        PlayerController player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
        switch (killCount)
        {
            case 1:
                if (!player.PlayerWasHit())
                {
                    APIManager.instance.UnlockTrophy(0); //Kill first enemy without getting hit
                }
                break;
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
        switch (score)
        {
            case 1000:
                APIManager.instance.UnlockTrophy(8); //Get 1000 score
                break;
            case 5000:
                APIManager.instance.UnlockTrophy(9); //Get 5000 score
                break;
            case 10000:
                APIManager.instance.UnlockTrophy(10); //Get 10000 score
                break;
            case 50000:
                APIManager.instance.UnlockTrophy(11); //Get 50000 score
                break;
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
}
