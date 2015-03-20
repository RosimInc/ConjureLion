using UnityEngine;
using System.Collections;

public class BallEffect : MonoBehaviour {

	public float radius;
	public float force;
	public float vision;
	public bool attract;
	public GameObject aim;
	public GameObject ball;
	public bool activated;
    public GameObject staticDetection;

    private bool _stayStatic = false;

	// Use this for initialization
	void Start () {
		
	}

    void Update()
    {
        if (!activated)
        {
            _stayStatic = false;
        }

        if (_stayStatic)
        {
            ball.transform.position = staticDetection.transform.position;
            rigidbody2D.velocity = Vector3.zero;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
		if (ball == null) return;

        if (_stayStatic)
        {
            return;
        }

        if (!activated)
        {
            return;
        }

		float angle = Vector3.Angle(aim.transform.forward,
			ball.transform.position - aim.transform.position);

		Vector3 local =
			aim.transform.InverseTransformPoint(ball.transform.position);

		if (local.x < 0)
			angle = -angle;
		//Angle between 0 (right) and 180 (left) and 90 (up) and -90 (down)

		if (angle > 180)
			angle = -(360 - angle);
		//Angle between 0 (up) and 180 (down) and -90 (left) and 90 (right)

		if (angle * angle > vision * vision) return;

		Vector3 toBall = ball.transform.position - transform.position;
		float distance = toBall.magnitude;
		int mod = attract ? -1 : 1;

		if (distance < radius)
		{
			//Determine if there's no shadowing obstacle in the way
			RaycastHit hit;

            Debug.DrawRay(transform.position, (ball.transform.position - transform.position));
            //1 << 8 - 1 << 10

            if (Physics.Raycast(transform.position, (ball.transform.position - transform.position), out hit,
				radius))
			{
				if (hit.collider.gameObject.layer == 8) return;

				ball.rigidbody2D.AddForce(force * mod * toBall.normalized *
					(radius - distance) / radius * Time.fixedDeltaTime, ForceMode2D.Force);
			}
		}
	}

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == ball && attract && activated)
        {
            _stayStatic = true;
        }
    }
}
