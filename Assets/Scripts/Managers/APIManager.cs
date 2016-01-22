using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class APIManager : MonoBehaviour {

    public static APIManager instance;

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
        //ShowSignIn();
	}

    public void SignIn(string username, string token, APILoginController loginMenu)
    {
        GameJolt.API.Objects.User user = new GameJolt.API.Objects.User(username, token);
        user.SignIn((bool success) => {
            if (success)
            {
                signedIn = true;
                loginMenu.ToggleLoginButton(true);
                loginMenu.CloseLoginMenu();
            }
            else
            {
                loginMenu.SetErrorText("api.txt.noconnection");
            }
        });
    }

    public void SignOut()
    {
        if (GameJolt.API.Manager.Instance.CurrentUser != null)
        {
            signedIn = false;
            GameJolt.API.Manager.Instance.CurrentUser.SignOut();
        }
    }

    public void ShowSignIn()
    {
        if (GameJolt.API.Manager.Instance.CurrentUser != null)
        {
            signedIn = false;
            GameJolt.API.Manager.Instance.CurrentUser.SignOut();
        }
        else
        {
            GameJolt.UI.Manager.Instance.ShowSignIn((bool success) =>
            {
                if (success)
                {
                    signedIn = true;
                    StatManager.instance.LoadStats();
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
        if (signedIn)
        {
            GameJolt.API.Trophies.Get(trophyId, (GameJolt.API.Objects.Trophy trophy) =>
            {
                if (!trophy.Unlocked)
                {
                    GameJolt.API.Trophies.Unlock(trophyId);
                }
            });
        }
    }

    public void LockTrophy(int trophyId)
    {
        GameJolt.API.Trophies.Get(trophyId, (GameJolt.API.Objects.Trophy trophy) =>
        {
            if (trophy.Unlocked)
            {
                trophy.Unlocked = false;
            }
        });
    }

    public void GetStat(string statname)
    {
        GameJolt.API.DataStore.Get(statname, false, (string value) =>
        {
            if (value != null)
            {
                int stat = int.Parse(value);
                StatManager.instance.LoadOnlineStat(stat, statname);
            }
        });
    }

    public void SetStat(int stat, string statname)
    {
        GameJolt.API.DataStore.Set(statname, stat.ToString(), false, (bool success) => { });
    }
}
