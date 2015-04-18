using UnityEngine;
using System.Collections;

public class Aspi : Robot
{
    public BoxCollider2D StaticDetection;
    
	private Vector3 ballOrigin;
	private GameObject ball;
	
	// Use this for initialization
	void Start () {
        if (ball == null) return;

		ball = GameObject.FindGameObjectWithTag("Ball");
		ballOrigin = ball.transform.position;
	}

    override protected void Update()
    {
        base.Update();

        if (ball == null) return;

		if(Input.GetKeyDown (KeyCode.Backspace)) {
			ball.transform.position = ballOrigin;
			ball.rigidbody2D.velocity = new Vector2(0,0);
		}
    }
}
