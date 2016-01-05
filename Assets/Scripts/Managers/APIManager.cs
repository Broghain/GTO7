using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class APIManager : MonoBehaviour {

    public static APIManager instance;

    [SerializeField]
    private Text signInText;
    [SerializeField]
    private Button showTrophiesButton;

    private bool signedIn = false;

    void Awake()
    {
        if (APIManager.instance != null && APIManager.instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

	// Use this for initialization
	void Start () 
    {
        ShowSignIn();
	}

    public void ShowSignIn()
    {
        if (GameJolt.API.Manager.Instance.CurrentUser != null)
        {
            GameJolt.API.Manager.Instance.CurrentUser.SignOut();
            SetSignedInButtons(false);
        }
        else
        {
            GameJolt.UI.Manager.Instance.ShowSignIn((bool success) =>
            {
                if (success)
                {
                    SetSignedInButtons(true);
                }
            });
        }
    }

	// Update is called once per frame
	void Update () 
    {
	
	}

    public bool GetSignedIn()
    {
        return signedIn;
    }

    public void ShowTrophies()
    {
        GameJolt.UI.Manager.Instance.ShowTrophies();
    }

    public void UnlockTrophy(int trophyId)
    {
        GameJolt.API.Trophies.Unlock(trophyId);
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
