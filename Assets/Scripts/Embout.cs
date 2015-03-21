using UnityEngine;
using System.Collections;

public class Embout : MonoBehaviour {

	private bool ballIsIn = false;
	private GameObject ball;
	public SplinePipe splinePipe;
	
	private void Start () {
		ball = GameObject.FindGameObjectWithTag ("Ball");
	}

	private void OnTriggerEnter2D (Collider2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		else 
			ballIsIn = true;
		
		splinePipe.setBallIsIn(ballIsIn);
		Debug.Log("ballIsIn "+ballIsIn);
	}
	
	private void OnTriggerStay2D (Collider2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		else 
			ballIsIn = true;
		
		Debug.Log("ballIsIn "+ballIsIn);
		splinePipe.setBallIsIn(ballIsIn);
	}
	
	private void OnTriggerExit2D (Collider2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		else 
			ballIsIn = false;
		
		Debug.Log("Leave ballIsIn "+ballIsIn);
		splinePipe.setBallIsIn(ballIsIn);
	}
}
