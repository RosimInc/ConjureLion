using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public BezierSpline spline;
    public int PlayerNumber;
	public float speed = 1f;
	public float defaultProgress = 0f;
	public GameObject fix;
	private float progress;
	
	// Use this for initialization
	void Start () {
		if(spline == null) {
			spline = GameObject.FindGameObjectWithTag("Rail").GetComponent<BezierSpline>();
			Debug.Log ("Rail set automatically");
		}
		
		Vector3 position = spline.GetPoint(progress);
		transform.localPosition = position;
		
		
	}
	
	private void Update () {

		Vector3 dir = spline.GetDirection(progress);
		float x = Input.GetAxisRaw("L_XAxis_" + PlayerNumber);
		float y = -Input.GetAxisRaw("L_YAxis_" + PlayerNumber);
		Vector3 joystickDir = new Vector3(x, y, 0);

		float angle = Vector3.Angle(dir, joystickDir);
		int mod = 1;
		if (angle > 90)
		{
			mod = -1;
			Debug.Log("Advance");
		}

		float stuff = Mathf.Sqrt(x * x + y * y);

		float movement = mod * Time.deltaTime * 10 * speed * stuff;

		//Debug.Log ( "Progress "+progress+" movement"+ movement);
		progress = spline.GetProgressFromDistance(progress, movement);
		//Debug.Log ( "Progress After "+progress);
		Vector3 position = spline.GetPoint(progress);
		position.z = 0;
		transform.localPosition = position;

		//TODO Here fix the movement so it uses MovePosition
		//rigidbody2D.MovePosition(transform.forward - position);
		
		//Orientation du fix
		fix.transform.LookAt(position + spline.GetDirection(progress) );
		fix.transform.Rotate(new Vector3(0,90,0));
	}
}
