using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusBarController : MonoBehaviour {

    [SerializeField]
    private Image healthbar;
    private Rect healthbarRect;
    private float healthbarXPos = 0.0f;
    [SerializeField]
    private Image shieldbar;
    private Rect shieldbarRect;
    private float shieldbarXPos = 0.0f;
    [SerializeField]
    private GameObject healthbarLights;
    private Vector3 lastShieldBarPos;

    [SerializeField]
    private Image experiencebar;
    private Rect experiencebarRect;
    private float experiencebarXPos = 0.0f;
    [SerializeField]
    private GameObject experiencebarLights;
    private float experienceLightsTimer = 0.0f;

    private PlayerController player;
    private UpgradeController upgrader;

	// Use this for initialization
	void Start () 
    {
        healthbarRect = healthbar.rectTransform.rect;
        shieldbarRect = shieldbar.rectTransform.rect;
        lastShieldBarPos = shieldbar.rectTransform.localPosition;
        experiencebarRect = experiencebar.rectTransform.rect;
        experiencebarXPos = -experiencebarRect.width;
	}
	
	// Update is called once per frame
	void Update () 
    {
        healthbar.rectTransform.localPosition = Vector3.Lerp(healthbar.rectTransform.localPosition, new Vector3(healthbarXPos, 0, 0), Time.deltaTime * 5);
        shieldbar.rectTransform.localPosition = Vector3.Lerp(shieldbar.rectTransform.localPosition, new Vector3(shieldbarXPos, 0, 0), Time.deltaTime * 5);
        experiencebar.rectTransform.localPosition = Vector3.Lerp(experiencebar.rectTransform.localPosition, new Vector3(experiencebarXPos, 0, 0), Time.deltaTime * 5);

        if (shieldbar.rectTransform.localPosition.x > lastShieldBarPos.x)
        {
            healthbarLights.SetActive(true);
        }
        else
        {
            healthbarLights.SetActive(false);
        }
        lastShieldBarPos = shieldbar.rectTransform.localPosition;

        if (upgrader != null && upgrader.AvailablePoints > 0)
        {
            experienceLightsTimer += Time.deltaTime;
            if (experienceLightsTimer >= 1.0f)
            {
                experiencebarLights.SetActive(!experiencebarLights.activeSelf);
                experienceLightsTimer = 0.0f;
            }
        }
        else if (experiencebarLights.activeSelf)
        {
            experiencebarLights.SetActive(false);
        }
	}

    public void UpdateHealthBar()
    {
        if (player == null)
        {
            player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
            upgrader = player.GetComponent<UpgradeController>();
        }

        float healthPct = (player.Health / player.MaxHealth) * 100;
        healthbarXPos = -healthbarRect.height + ((healthPct / 100) * healthbarRect.height);
    }

    public void UpdateShieldBar()
    {
        if (player == null)
        {
            player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
            upgrader = player.GetComponent<UpgradeController>();
        }

        float shieldPct = (player.Shield / player.MaxShield) * 100;
        shieldbarXPos = -shieldbarRect.height + ((shieldPct / 100) * shieldbarRect.height);
    }

    public void UpdateExperienceBar()
    {
        if (player == null)
        {
            player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
            upgrader = player.GetComponent<UpgradeController>();
        }

        float expPct = (player.Experience / player.MaxExperience) * 100;
        experiencebarXPos = -experiencebarRect.width + ((expPct / 100) * experiencebarRect.width);
    }
}
