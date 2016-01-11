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
    
    [SerializeField]
    private Text signInText;
    [SerializeField]
    private Button showTrophiesButton;

    void Start()
    {
        
    }

    void Update()
    {
        SetSignedInButtons(APIManager.instance.GetSignedIn());
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

    private void SetSignedInButtons(bool value)
    {
        showTrophiesButton.gameObject.SetActive(value);
        if (value)
        {
            signInText.GetComponent<TextTranslation>().SetKey("api.btn.logout");
        }
        else
        {
            signInText.GetComponent<TextTranslation>().SetKey("api.btn.login");
        }
    }
}
