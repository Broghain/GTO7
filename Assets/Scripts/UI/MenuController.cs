using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private OptionsMenuController optionsMenu;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text killsText;
    [SerializeField]
    private Text waveText;

    void Start()
    {
        
    }

    void Update()
    {
        scoreText.text = StatManager.instance.GetHighScore().ToString();
        killsText.text = StatManager.instance.GetMostKills().ToString();
        waveText.text = StatManager.instance.GetBestWave().ToString();
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
