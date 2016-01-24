using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private OptionsMenuController optionsMenu;
    [SerializeField]
    private Button loadButton;
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
        SignedInChanged();
        loadButton.interactable = DataManager.instance.LoadAvailable();
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
        DataManager.instance.IsLoaded = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGame()
    {
        DataManager.instance.LoadGame();
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

    public void SignedInChanged()
    {
        SetSignedInButtons(APIManager.instance.GetSignedIn());
    }
}
