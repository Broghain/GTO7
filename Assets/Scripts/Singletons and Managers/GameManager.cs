using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private Transform playerTransform;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public Transform GetPlayer()
    {
        return playerTransform;
    }
}
