using UnityEngine;
using System.Collections;

public class LaserController : PooledObject {

    [SerializeField]
    private string targetTag;

    private GameObject startObject;

    private bool rayCast = false;

    private LineRenderer line;
    private TimeToLive ttl;

	// Use this for initialization
	void Start () {
        line = GetComponent<LineRenderer>();
        ttl = GetComponent<TimeToLive>();

        line.SetPosition(0, Vector3.one * 1000);
        line.SetPosition(1, Vector3.one * 1000);
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 startPosition = startObject.transform.position;
        startPosition.z = 0;
        line.SetPosition(0, startPosition);

        Debug.Log(transform.up);

        RaycastHit hit;
        if (Physics.Raycast(startPosition, transform.up, out hit, 100))
        {
            Vector3 endPosition = hit.point;
            endPosition.z = 0;
            line.SetPosition(1, endPosition);
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject != null && hitObject.tag == targetTag)
            {
                Destroy(hitObject);
            }
        }
        else
        {
            Vector3 endPosition = startPosition + (transform.up * 100);
            endPosition.z = 0;
            line.SetPosition(1, endPosition);
        }
        rayCast = true;

        if (ttl.GetTimeToLive() <= 0)
        {
            gameObject.SetActive(false);
            gameObject.name = "Unused" + gameObject.name;
            transform.position = new Vector3(1000, 1000, 1000);
            ttl.Reset();

            line.SetPosition(0, Vector3.one * 1000);
            line.SetPosition(1, Vector3.one * 1000);
        }
	}

    public void SetStartObject(GameObject obj)
    {
        startObject = obj;
    }
}
