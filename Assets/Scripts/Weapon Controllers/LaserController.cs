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
        Vector3 startPosition = startObject.transform.position; //startposition of line is at weapon
        startPosition.z = 0;
        line.SetPosition(0, startPosition);

        RaycastHit hit;
        if (Physics.Raycast(startPosition, transform.up, out hit, 100)) //raycast to check for enemies in laser's path
        {
            Vector3 endPosition; 
            GameObject hitObject = hit.collider.gameObject; //get hit object
            if (hitObject != null && hitObject.tag == targetTag) //if an object was hit and object's tag is target tag ("Enemy")
            {
                endPosition = hit.point; //end point of line is where hit took place
                if (lastHitObject == null || lastHitObject != hitObject) //if laser didn't already hit something, or object that was hit last is not this object (prevent damaging enemy every update call)
                {
                    EnemyBehaviour enemy = hitObject.GetComponent<EnemyBehaviour>();
                    if (enemy != null && !enemy.IsOffScreen()) //if enemy isn't null and enemy is not off-screen
                    {
                        enemy.TakeDamage(damage, hit.point); //apply damage
                        DifficultyManager.instance.SetHitPct(true, WeaponType.Laser); //hit!
                    }
                    else
                    {
                        endPosition = startPosition + (transform.up * 100); //set end position of line some point far away in up direction
                        DifficultyManager.instance.SetHitPct(false, WeaponType.Laser); //miss!
                    }
                }
                lastHitObject = hitObject;
            }
            else //object that was hit does not have correct tag
            {
                endPosition = startPosition + (transform.up * 100); //set end position of line some point far away in up direction
                DifficultyManager.instance.SetHitPct(false, WeaponType.Laser); //miss!
            }
            endPosition.z = 0;
            line.SetPosition(1, endPosition);
        }
        else //nothing hit by raycast
        {
            Vector3 endPosition = startPosition + (transform.up * 100); //set end position of line some point far away in up direction
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
