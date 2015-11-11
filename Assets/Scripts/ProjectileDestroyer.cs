using UnityEngine;
using System.Collections;

public class ProjectileDestroyer : MonoBehaviour {

    [SerializeField]
    private float timeToLive = 1.0f;
    private float timer;

    [SerializeField]
    private bool useTTL = true;

    private Vector3 screenUpperLeft;
    private Vector3 screenLowerRight;

    private Transform parent;

    void Start()
    {
        timer = timeToLive;
        screenUpperLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        screenLowerRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

	// Update is called once per frame
	void Update () {
        if (useTTL)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                DisableBullet();
            }
        }

        if (transform.position.x < screenUpperLeft.x || transform.position.x > screenLowerRight.x || transform.position.y > screenUpperLeft.y || transform.position.y < screenLowerRight.y)
        {
            //DisableBullet();
        }
	}

    private void DisableBullet()
    {
        gameObject.SetActive(false);
        gameObject.name = "Unused" + gameObject.name;
        transform.position = new Vector3(1000, 1000, 1000);
        timer = timeToLive;
        transform.parent = parent;
    }

    public void SetParent(Transform parent)
    {
        this.parent = parent;
        transform.parent = parent;
    }
}
