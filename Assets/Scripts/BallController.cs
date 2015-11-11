using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

    [SerializeField]
    private float startForce;

    private Rigidbody2D rbody;
    private Collider2D collider;
    private Vector3 startPosition;
    private bool launched;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider2D>();
        rbody = GetComponent<Rigidbody2D>();
        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.localPosition;
        launched = false;
        collider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump") && !launched)
        {
            float randomXForce = Mathf.Sign(Random.Range(-startForce, startForce)) * startForce;
            rbody.AddForce(new Vector2(randomXForce, startForce), ForceMode2D.Impulse);
            transform.parent = null;
            launched = true;
            collider.enabled = true;
        }

        if (transform.position.y <= Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y)
        {
            Reset();
        }

        if(launched)
        {
            if (Mathf.Abs(rbody.velocity.x) < 0.25f)
            {
                rbody.AddForce(new Vector2(Mathf.Sign(rbody.velocity.x) * 0.1f, 0));
            }
            if (Mathf.Abs(rbody.velocity.y) < 0.25f)
            {
                rbody.AddForce(new Vector2(0, Mathf.Sign(rbody.velocity.y) * 0.1f));
            }
        }
        
	}

    void Reset()
    {
        rbody.velocity = Vector2.zero;
        transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        transform.localPosition = startPosition;
        launched = false;
        collider.enabled = false;
    }
}
