using UnityEngine;
using System.Collections;

public class OptionsMenuController : MonoBehaviour {

    [SerializeField]
    private GameObject languagePanel = null;
    [SerializeField]
    private GameObject audioPanel = null;

    public void OpenOptionsMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        gameObject.SetActive(false);
        CloseAudioPanel();
        CloseLanguagePanel();
    }

    public void OpenAudioPanel()
    {
        audioPanel.SetActive(true);
    }

    public void CloseAudioPanel()
    {
        audioPanel.SetActive(false);
    }

    public void OpenLanguagePanel()
    {
        languagePanel.SetActive(true);
    }

    public void CloseLanguagePanel()
    {
        languagePanel.SetActive(false);
    }
}
