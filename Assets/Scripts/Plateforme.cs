using UnityEngine;

public class Plateforme : MonoBehaviour {
	
	public BezierSpline spline;
	
	public float duration = 10;
	
	private float progress;
	private bool goingForward = true;
	
	private GameObject ball;
	
	private void Update () {
		if (goingForward) {
			progress += Time.deltaTime / duration;
			if (progress > 1f) {
				progress = 2f - progress;
				goingForward = false;
			}
		}
		else {
			progress -= Time.deltaTime / duration;
			if (progress < 0f) {
				progress = -progress;
				goingForward = true;
			}
		}
		
		Vector3 position = spline.GetPoint(progress);
		Vector3 shifting = position - transform.localPosition;
		
		transform.localPosition = position;
		
		if(ball != null) {
			ball.transform.position = ball.transform.position + shifting;
		}
	}
	
	void OnCollisionEnter2D(Collision2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		
		ball = coll.gameObject;
		Debug.Log ("Ball enter");
	}
	
	void OnCollisionExit2D(Collision2D coll)
	{
		if(coll.gameObject.tag != "Ball")
			return;
		
		ball = null;
		Debug.Log ("Ball leave");
	}
}