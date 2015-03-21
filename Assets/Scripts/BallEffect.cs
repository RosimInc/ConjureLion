using UnityEngine;
using System.Collections;
using System;

public class BallEffect : MonoBehaviour {

	public float radius;
	public float force;
	public float vision;
	public bool attract;
	public GameObject aim;
	public Ball ball;
	public bool activated;
    public GameObject staticDetection;
    public int PlayerNumber = 1;

    private bool _stayStatic = false;
    private bool _pullBlocked = false;

    public bool PullBlocked
    {
        get { return _pullBlocked; }
    }
    

	// Use this for initialization
	void Start () {
		ball = GameObject.FindGameObjectWithTag ("Ball").GetComponent<Ball>();
		ResourceManager.Instance.AddMovingObject(ball.gameObject);
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
            ball.rigidbody2D.velocity = Vector3.zero;
        }
        else
        {
            ball.rigidbody2D.gravityScale = 0.5f;
        }
    }

	// Update is called once per frame
	void FixedUpdate () {

        if (_stayStatic || _pullBlocked) return;
        		if (!activated) return;

		foreach(GameObject obj in ResourceManager.Instance.MovingObjects)
		{
			checkEffects(obj);
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
			radius))
		{
			if (hit.collider.gameObject.layer == 8) return;
			
			float maxTriggerValue = Mathf.Max( Input.GetAxisRaw("TriggersL_" + PlayerNumber), Input.GetAxisRaw("TriggersR_" + PlayerNumber));
			maxTriggerValue = Mathf.Max ( maxTriggerValue, Math.Abs(Input.GetAxisRaw("TriggersLR_" + PlayerNumber)));

			ball.rigidbody2D.AddForce(force * mod * toBall.normalized *
					(radius - distance) / radius * Time.fixedDeltaTime *
                    maxTriggerValue, ForceMode2D.Force);
		}
	}

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == ball.gameObject && attract && activated)
        {
            if (!_pullBlocked)
            {
                _stayStatic = true;
            }
            
            ball.Owner = this;
        }
        else if (collider.gameObject.layer == 8 && _stayStatic)
        {
            Debug.Log("abcd");
            _stayStatic = false;
        }
    }

    public void DropBall()
    {
        _stayStatic = false;
        StopCoroutine("BlockPullForAWhile");
        StartCoroutine("BlockPullForAWhile");
    }

    private IEnumerator BlockPullForAWhile()
    {
        _pullBlocked = true;

        yield return new WaitForSeconds(1f);

        _pullBlocked = false;
    }
}
