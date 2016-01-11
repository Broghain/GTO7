using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpgradePanelController : MonoBehaviour {

    [SerializeField]
    private UpgradeController upgrader;

    [SerializeField]
    private Button btnHullPlus;
    [SerializeField]
    private Button btnShieldPlus;
    [SerializeField]
    private Button btnBulletPlus;
    [SerializeField]
    private Button btnLaserPlus;
    [SerializeField]
    private Button btnRocketPlus;

    [SerializeField]
    private Button btnHullMinus;
    [SerializeField]
    private Button btnShieldMinus;
    [SerializeField]
    private Button btnBulletMinus;
    [SerializeField]
    private Button btnLaserMinus;
    [SerializeField]
    private Button btnRocketMinus;

    [SerializeField]
    private Button btnApply;
    [SerializeField]
    private Button btnClose;
    [SerializeField]
    private Button btnOpen;

    [SerializeField]
    private Text txtHullRank;
    [SerializeField]
    private Text txtShieldRank;
    [SerializeField]
    private Text txtBulletRank;
    [SerializeField]
    private Text txtLaserRank;
    [SerializeField]
    private Text txtRocketRank;

    [SerializeField]
    private Text txtPtsAvailable;

    [SerializeField]
    private Text txtInfo;

	// Use this for initialization
	void Start () 
    {
        if (upgrader == null)
        {
            upgrader = GameObject.FindObjectOfType<UpgradeController>();
            Debug.LogError("UpgradeController is null! Assign an UpgradeController in the inspector panel.");
        }
	}

	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void UpdatePanel()
    {
        txtPtsAvailable.text = upgrader.AvailablePoints.ToString();
        txtHullRank.text = upgrader.GetRank(UpgradeType.Hull, true).ToString();
        txtShieldRank.text = upgrader.GetRank(UpgradeType.Shield, true).ToString();
        txtBulletRank.text = upgrader.GetRank(UpgradeType.Bullet, true).ToString();
        txtLaserRank.text = upgrader.GetRank(UpgradeType.Laser, true).ToString();
        txtRocketRank.text = upgrader.GetRank(UpgradeType.Rocket, true).ToString();

        if (upgrader.AvailablePoints > 0)
        {
            btnHullPlus.interactable = true;
            btnShieldPlus.interactable = true;
            btnBulletPlus.interactable = true;
            btnLaserPlus.interactable = true;
            btnRocketPlus.interactable = true;
        }
        else
        {
            btnHullPlus.interactable = false;
            btnShieldPlus.interactable = false;
            btnBulletPlus.interactable = false;
            btnLaserPlus.interactable = false;
            btnRocketPlus.interactable = false;
        }

        if (upgrader.HasWeapon(false))
        {
            btnClose.interactable = true;
        }
        
        if (upgrader.HasWeapon(true))
        {
            btnApply.interactable = upgrader.StatsChanged(UpgradeType.All);
            txtInfo.text = "";
        }
        else
        {
            btnClose.interactable = false;
            btnApply.interactable = false;
            txtInfo.text = "Upgrade at least one weapon to Rank 1 or higher!";
        }

        btnHullMinus.interactable = upgrader.StatsChanged(UpgradeType.Hull);
        btnShieldMinus.interactable = upgrader.StatsChanged(UpgradeType.Shield);
        btnBulletMinus.interactable = upgrader.StatsChanged(UpgradeType.Bullet);
        btnLaserMinus.interactable = upgrader.StatsChanged(UpgradeType.Laser);
        btnRocketMinus.interactable = upgrader.StatsChanged(UpgradeType.Rocket);
    }

    public void ChangeHullPts(int amount)
    {
        ChangePts(UpgradeType.Hull, amount);
    }
    public void ChangeShieldPts(int amount)
    {
        ChangePts(UpgradeType.Shield, amount);
    }
    public void ChangeBulletPts(int amount)
    {
        ChangePts(UpgradeType.Bullet, amount);
    }
    public void ChangeLaserPts(int amount)
    {
        ChangePts(UpgradeType.Laser, amount);
    }
    public void ChangeRocketPts(int amount)
    {
        ChangePts(UpgradeType.Rocket, amount);
    }

    private void ChangePts(UpgradeType upgrade, int amount)
    {
        if (upgrader.AvailablePoints >= amount)
        {
            upgrader.ChangePts(upgrade, amount);
            upgrader.AvailablePoints -= amount;
        }
        UpdatePanel();
    }

    public void OpenUpgradePanel()
    {
        if (!GameManager.instance.GetPaused())
        {
            btnOpen.interactable = false;
            gameObject.SetActive(true);
            UpdatePanel();
            GameManager.instance.TogglePause();
        }
    }

    public void CloseUpgradePanel()
    {
        btnOpen.interactable = true;
        gameObject.SetActive(false);
        UpdatePanel();
        GameManager.instance.TogglePause();
    }

    public void Apply()
    {
        upgrader.ApplyUpgrade();
        UpdatePanel();
        StatManager.instance.CheckUpgradeAchievement();
    }
}
