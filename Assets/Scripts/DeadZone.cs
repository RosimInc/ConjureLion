using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {
	private Vector3 ballOrigin;
	private GameObject ball;

	// Use this for initialization
	void Start () {
		ball = GameObject.FindGameObjectWithTag("Ball");
		ballOrigin = ball.transform.position;
	}
	
	
	private void OnTriggerEnter2D (Collider2D coll) {
		if(coll.tag != "Ball")
			return;
		
		coll.transform.position = ballOrigin;
		coll.rigidbody2D.velocity = new Vector2(0,0);
		coll.rigidbody2D.inertia = 0f;
	}
	
	
}
