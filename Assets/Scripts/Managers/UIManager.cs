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
    private APITrophyUnlockController trophyUnlockPanel;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private Animation hitFeedback;

    [SerializeField]
    private StatusBarController statusBars;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
#if UNITY_ANDROID
        pauseButton.SetActive(true);
#endif
	}
	
	// Update is called once per frame
	void Update () {
        UpdateStatusBars();
	}

    public void ShowHitFeedback()
    {
        statusBars.ShowHitFeedback();
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
        else if(!GameManager.instance.GetGameOver())
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

    public void ShowTrophyUnlock(string name, string descr, Sprite img)
    {
        trophyUnlockPanel.AddUnlockedTrophy(name, descr, img);
    }
}
