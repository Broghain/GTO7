using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

    [SerializeField]
    private GameObject subMenuPanel, coopMenu, versusMenu, optionsMenu;

    public enum MenuType
    {
        Coop,
        Versus,
        Options
    };

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ToggleSubMenu(string menuType)
    {
        subMenuPanel.SetActive(true);
        coopMenu.SetActive(false);
        versusMenu.SetActive(false);
        optionsMenu.SetActive(false);
        switch (menuType)
        {
            case "Coop":
                coopMenu.SetActive(true);
                break;
            case "Versus":
                versusMenu.SetActive(true);
                break;
            case "Options":
                optionsMenu.SetActive(true);
                break;
        }
    }

    public void OpenOfflineGameScene()
    {
        Application.LoadLevel(1);
    }
}
