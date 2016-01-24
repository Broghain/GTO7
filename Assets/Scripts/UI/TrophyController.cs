using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrophyController : MonoBehaviour {

    [SerializeField]
    private Image trophyIcon;
    [SerializeField]
    private Text trophyName;
    [SerializeField]
    private Text trophyDescr;
    [SerializeField]
    private GameObject trophyOverlay;

    public void SetTrophyValues(string name, string descr, Sprite icon, bool locked)
    {
        trophyIcon.sprite = icon;
        trophyName.text = name;
        trophyDescr.text = descr;
        if (trophyOverlay != null)
        {
            trophyOverlay.SetActive(locked);
        }
    }
}
