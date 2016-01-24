using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class APILoginController : MonoBehaviour {

    [SerializeField]
    private MenuController menu;

    [SerializeField]
    private InputField usernameInput;
    [SerializeField]
    private InputField tokenInput;
    [SerializeField]
    private TextTranslation errorTextKey;

	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void SignIn()
    {
        APIManager.instance.SignIn(usernameInput.text, tokenInput.text, this);
    }

    public void SetErrorText(string key)
    {
        errorTextKey.gameObject.SetActive(true);
        errorTextKey.SetKey(key);
    }

    public void ToggleLoginButton(bool signedInChanged)
    {
        if (signedInChanged)
        {
            menu.SignedInChanged();
        }
    }

    public void OpenLoginMenu()
    {
        if (APIManager.instance.GetSignedIn())
        {
            APIManager.instance.SignOut();
            ToggleLoginButton(true);
        }
        else
        {
            transform.parent.gameObject.SetActive(true);
            gameObject.SetActive(true);
        }
    }

    public void CloseLoginMenu()
    {
        Reset();
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }

    private void Reset()
    {
        usernameInput.text = "";
        tokenInput.text = "";
        errorTextKey.SetKey("");
        errorTextKey.gameObject.SetActive(false);
    }
}
