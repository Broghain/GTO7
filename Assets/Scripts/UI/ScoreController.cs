using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text killsText;
    [SerializeField]
    private Text waveText;

    private int score; //actual score
    private int tempScore; //score displayed (delayed increment)

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        scoreText.text = GameManager.instance.GetScore().ToString();
        killsText.text = GameManager.instance.GetKillCount().ToString();
        waveText.text = GameManager.instance.GetWaveNumber().ToString();
	}


}
