using UnityEngine;
using System.Collections;

public class LaserController : PooledObjectBehaviour {

    //attributes
    [SerializeField]
    private float damage = 10.0f;
    [SerializeField]
    private float maxDamage = 100.0f;

    //sound effects
    [SerializeField]
    private AudioClip[] shotSounds;
    [SerializeField]
    private AudioClip[] hitSounds;

    //tag to check for when hit
    [SerializeField]
    private string targetTag;

    private GameObject startObject;

    private LineRenderer line;
    private TimeToLive ttl;

    private GameObject lastHitObject;


    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

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

        RaycastHit hit;
        if (Physics.Raycast(startPosition, transform.up, out hit, 100))
        {
            Vector3 endPosition = startPosition + (transform.up * 100);
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject != null && hitObject.tag == targetTag)
            {
                endPosition = hit.point;
                if (lastHitObject == null || lastHitObject != hitObject)
                {
                    EnemyBehaviour enemy = hitObject.GetComponent<EnemyBehaviour>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage, hit.point);
                    }
                }
                lastHitObject = hitObject;
            }
            endPosition.z = 0;
            line.SetPosition(1, endPosition);
        }
        else
        {
            Vector3 endPosition = startPosition + (transform.up * 100);
            endPosition.z = 0;
            line.SetPosition(1, endPosition);
        }

        if (ttl.GetTimer() <= 0)
        {
            DisableInPool();

            ttl.Reset();

            line.SetPosition(0, Vector3.one * 1000);
            line.SetPosition(1, Vector3.one * 1000);
            lastHitObject = null;
        }
	}

    public void SetStartObject(GameObject obj)
    {
        startObject = obj;
    }
    public float GetMaxDmg()
    {
        return maxDamage;
    }

    public AudioClip GetShotClip()
    {
        return shotSounds[Random.Range(0, shotSounds.Length)];
    }
    public AudioClip GetHitClip()
    {
        return hitSounds[Random.Range(0, hitSounds.Length)];
    }
}
