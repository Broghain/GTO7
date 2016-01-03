using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    [SerializeField]
    private UpgradePanelController upgradePanel;
    [SerializeField]
    private PausePanelController pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private StatusBarController statusBars;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        UpdateStatusBars();
	}

    private void UpdateStatusBars()
    {
        statusBars.UpdateHealthBar();
        statusBars.UpdateShieldBar();
        statusBars.UpdateExperienceBar();
    }

    public void ToggleUpgradePanel()
    {
        if (upgradePanel.gameObject.activeSelf)
        {
            upgradePanel.CloseUpgradePanel();
        }
        else
        {
            upgradePanel.OpenUpgradePanel();
        }
    }

    public void TogglePausePanel()
    {
        if (pausePanel.gameObject.activeSelf)
        {
            pausePanel.ClosePausePanel();
        }
        else
        {
            pausePanel.OpenPausePanel();
        }
    }

    public void EnableGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        GameManager.instance.TogglePause();
    }

    public bool UpgradePanelOpen()
    {
        return upgradePanel.gameObject.activeSelf;
    }

    public void PlayButtonSound(AudioClip buttonSound)
    {
        AudioManager.instance.PlaySound(buttonSound);
    }
}
