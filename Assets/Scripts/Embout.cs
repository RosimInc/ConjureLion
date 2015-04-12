using UnityEngine;
using System.Collections;

public class Embout : MonoBehaviour {

	private bool ballIsIn = false;
	public SplinePipe splinePipe;
	public bool isBeginning = false;
	
	public bool GetBallIsIn () {
	return ballIsIn;
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
	
	public void FlowIntensity (float intensity) {
		splinePipe.blower.intensity = intensity;
		splinePipe.blower.isAtBeginning = isBeginning;
	}
}
