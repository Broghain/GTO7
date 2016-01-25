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
            touchPosition.z = 0;
            Vector2 movement = Vector2.zero;

            if (Vector3.Distance(touchPosition, player.transform.position) > 0.1f)
            {
                movement = new Vector2(touchPosition.x - player.transform.position.x, touchPosition.y - player.transform.position.y);
                movement.x = Mathf.Clamp(movement.x, -1.0f, 1.0f);
                movement.y = Mathf.Clamp(movement.y, -1.0f, 1.0f);
            }

            player.UpdateMovement(movement.y, movement.x);
        }
	}
#endif
}
