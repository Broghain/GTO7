using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour {

    [SerializeField]
    private OptionsMenuController optionsMenu = null;

    public void OpenPausePanel()
    {
        gameObject.SetActive(true);
        GameManager.instance.TogglePause();
    }

    public void ClosePausePanel()
    {
        gameObject.SetActive(false);
        if (optionsMenu.gameObject.activeSelf)
        {
            optionsMenu.gameObject.SetActive(false);
        }
        GameManager.instance.TogglePause();
    }
}
