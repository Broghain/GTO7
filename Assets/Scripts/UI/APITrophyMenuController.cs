using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APITrophyMenuController : MonoBehaviour {

    [SerializeField]
    private List<int> bronzeTrophies;
    [SerializeField]
    private List<int> silverTrophies;
    [SerializeField]
    private List<int> goldTrophies;
    [SerializeField]
    private List<int> platinumTrophies;

    [SerializeField]
    private GameObject bronzeContainer;
    [SerializeField]
    private GameObject silverContainer;
    [SerializeField]
    private GameObject goldContainer;
    [SerializeField]
    private GameObject platinumContainer;

    [SerializeField]
    private GameObject trophyPrefab;

	// Use this for initialization
	void Start () 
    {
        for (int i = 0; i < bronzeTrophies.Count; i++)
        {
            int trophyId = bronzeTrophies[i];
            APIManager.instance.GetTrophyUnlocked(trophyId, this, bronzeContainer, i, bronzeTrophies.Count);
        }
        for (int i = 0; i < silverTrophies.Count; i++)
        {
            int trophyId = silverTrophies[i];
            APIManager.instance.GetTrophyUnlocked(trophyId, this, silverContainer, i, silverTrophies.Count);
        }
        for (int i = 0; i < goldTrophies.Count; i++)
        {
            int trophyId = goldTrophies[i];
            APIManager.instance.GetTrophyUnlocked(trophyId, this, goldContainer, i, goldTrophies.Count);
        }
        for (int i = 0; i < platinumTrophies.Count; i++)
        {
            int trophyId = platinumTrophies[i];
            APIManager.instance.GetTrophyUnlocked(trophyId, this, platinumContainer, i, platinumTrophies.Count);
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void CreateTrophyObj(string name, string description, Sprite image, bool locked, GameObject container, int trophyNum, int trophyCount)
    {
        GameObject trophyObj = (GameObject)Instantiate(trophyPrefab);
        trophyObj.transform.SetParent(container.transform);
        trophyObj.transform.localScale = new Vector3(1, 1, 1);

        TrophyController trophyController = trophyObj.GetComponent<TrophyController>();
        trophyController.SetTrophyValues(name, description, image, locked);

        float width = trophyObj.GetComponent<RectTransform>().rect.width;
        float startOffset = ((width / 2) - ((width / 2) * trophyCount));
        trophyObj.transform.localPosition = new Vector3(startOffset + (trophyNum * width), 0, 0);
    }

    public void OpenTrophyMenu()
    {
        transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void CloseTrophyMenu()
    {
        gameObject.SetActive(false);
        transform.parent.gameObject.SetActive(false);
    }
}
