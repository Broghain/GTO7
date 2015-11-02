using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [SerializeField]
    private GameObject wallNodePrefab, wallPrefab;

    [SerializeField]
    private GameObject explosionPrefab;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () 
    {
        CreateWalls();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void CreateWalls()
    {
        Instantiate(wallNodePrefab, Camera.main.ScreenToWorldPoint(new Vector3(20, 20, 10)), Quaternion.identity);
        Instantiate(wallPrefab, Camera.main.ScreenToWorldPoint(new Vector3(20, 0, 10)), Quaternion.Euler(0,0,0));
        Instantiate(wallNodePrefab, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 20, 20, 10)), Quaternion.identity);
        Instantiate(wallPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height - 20, 10)), Quaternion.Euler(0, 0, 90));
        Instantiate(wallNodePrefab, Camera.main.ScreenToWorldPoint(new Vector3(20, Screen.height - 20, 10)), Quaternion.identity);
        Instantiate(wallPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 20, 0, 10)), Quaternion.Euler(0, 0, 0));
        Instantiate(wallNodePrefab, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 20, Screen.height - 20, 10)), Quaternion.identity);
    }

    public void InstantiateExplosion(Vector3 position)
    {
        Instantiate(explosionPrefab, position, Quaternion.identity);
    }
}
