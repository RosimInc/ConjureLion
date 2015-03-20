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
		Vector3 position = spline.GetPoint(progress);
		transform.localPosition = position;
	}
	
	private void Update () {
        if (Input.GetAxisRaw("L_XAxis_" + PlayerNumber) > 0f)
        {
            progress += Time.deltaTime * speed * Input.GetAxisRaw("L_XAxis_" + PlayerNumber);
			if (progress > 1f) {
					progress = 1f;
			}
		}
        else if (Input.GetAxisRaw("L_XAxis_" + PlayerNumber) < 0f)
        {
            progress += Time.deltaTime * speed * Input.GetAxisRaw("L_XAxis_" + PlayerNumber);
			if (progress < 0f) {
				progress = 0f;
			}
		}
		
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
