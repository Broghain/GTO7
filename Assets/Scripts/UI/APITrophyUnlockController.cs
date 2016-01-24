using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class APITrophyUnlockController : MonoBehaviour {

    private class Trophy
    {
        public string name;
        public string descr;
        public Sprite img;

        public Trophy(string name, string descr, Sprite img)
        {
            this.name = name;
            this.descr = descr;
            this.img = img;
        }
    }

    [SerializeField]
    private TrophyController trophyController;
    private List<Trophy> waitingTrophies;
    private Animation unlockAnimation;

	// Use this for initialization
	void Start () 
    {
        waitingTrophies = new List<Trophy>();
        unlockAnimation = GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!unlockAnimation.isPlaying && waitingTrophies.Count > 0)
        {
            trophyController.SetTrophyValues(waitingTrophies[0].name, waitingTrophies[0].descr, waitingTrophies[0].img, false);
            unlockAnimation.Play();
            Debug.Log(waitingTrophies[0].name);
            waitingTrophies.RemoveAt(0);
        }
	}

    public void AddUnlockedTrophy(string name, string descr, Sprite img)
    {
        waitingTrophies.Add(new Trophy(name, descr, img));
    }
}
