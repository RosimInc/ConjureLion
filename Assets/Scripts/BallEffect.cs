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
    public int PlayerNumber;

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
            ball.rigidbody2D.gravityScale = 0f;
        }
        else
        {
            ball.rigidbody2D.gravityScale = 0.5f;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {

 		if (_stayStatic) return;
        if (!activated) return;

		checkEffects(ball);
		checkPivotEffects();
	}

	private void checkPivotEffects()
	{
		//Raycast in front

		RaycastHit hit;
		if (Physics.Raycast(gameObject.transform.position, aim.transform.forward, out hit, radius, (1 << 15)))
		{
			Debug.Log("HINGE!");
			//Detected hinge
			GameObject platform = hit.collider.gameObject;

			Vector3 centrePos = platform.transform.position;
			Vector3 hitPos = hit.point;
			Vector3 dirCentre = centrePos - aim.transform.position;
			Vector3 dirHit = hitPos - aim.transform.position;

			float angle = Vector3.Angle(dirCentre, dirHit);

			/*Vector3 local =
				aim.transform.InverseTransformPoint(obj.transform.position);

			if (local.x < 0)
				angle = -angle;*/
			//Angle between 0 (right) and 180 (left) and 90 (up) and -90 (down)

			if (angle > 180)
				angle = -(360 - angle);
			//Angle between 0 (up) and 180 (down) and -90 (left) and 90 (right)

			int forceSide = (angle > 0) ? 1 : -1;

			platform.rigidbody2D.AddTorque(forceSide * force *
					(radius - dirHit.magnitude) / radius * Time.fixedDeltaTime *
                    Mathf.Max( Input.GetAxisRaw("TriggersL_" + PlayerNumber), Input.GetAxisRaw("TriggersR_" + PlayerNumber)), ForceMode2D.Force);
		}
	}

	private void checkEffects(GameObject obj)
	{
		if (obj == null) return;

		Vector3 toBall = obj.transform.position - transform.position;
		float distance = toBall.magnitude;

		//Out of ranges
		if (distance > radius) return;

		float angle = Vector3.Angle(aim.transform.forward,
			obj.transform.position - aim.transform.position);

		Vector3 local =
			aim.transform.InverseTransformPoint(obj.transform.position);

		if (local.x < 0)
			angle = -angle;
		//Angle between 0 (right) and 180 (left) and 90 (up) and -90 (down)

		if (angle > 180)
			angle = -(360 - angle);
		//Angle between 0 (up) and 180 (down) and -90 (left) and 90 (right)

		if (angle * angle > vision * vision) return;

		
		int mod = attract ? -1 : 1;

		//Determine if there's no shadowing obstacle in the way
		RaycastHit hit;

		Debug.DrawRay(transform.position, (obj.transform.position - transform.position));
		//1 << 8 - 1 << 10

		if (Physics.Raycast(transform.position, (obj.transform.position - transform.position), out hit,
			radius, (1 << 8) | (1 << 10) | (1 << 15)))
		{
			if (hit.collider.gameObject.layer == 8) return;

			ball.rigidbody2D.AddForce(force * mod * toBall.normalized *
					(radius - distance) / radius * Time.fixedDeltaTime *
                    Mathf.Max( Input.GetAxisRaw("TriggersL_" + PlayerNumber), Input.GetAxisRaw("TriggersR_" + PlayerNumber)), ForceMode2D.Force);
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
