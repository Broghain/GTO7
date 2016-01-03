using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private OptionsMenuController optionsMenu;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Text mostKillsText;
    [SerializeField]
    private Text bestWaveText;

    void Start()
    {
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        mostKillsText.text = PlayerPrefs.GetInt("KillCount", 0).ToString();
        bestWaveText.text = PlayerPrefs.GetInt("WaveNum", 0).ToString();
    }

    public void StartGame()
    {
        Application.LoadLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
