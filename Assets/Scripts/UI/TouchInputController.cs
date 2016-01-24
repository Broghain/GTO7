using UnityEngine;
using System.Collections;

public class TouchInputController : MonoBehaviour {
#if UNITY_ANDROID
    private PlayerController player;

	// Use this for initialization
	void Start () 
    {
        player = GameManager.instance.GetPlayer().GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.touchCount > 0)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

            Vector2 movement = new Vector2(touchPosition.y - player.transform.position.y, touchPosition.x - player.transform.position.x);
            movement.Normalize();

            player.UpdateMovement(movement.x, movement.y);
        }
	}
#endif
}
