using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public Transform LeftArrow;
    public Transform RightArrow;

    private Vector3 _initialPosition1;
    private Vector3 _finalPosition1;
    private Vector3 _initialPosition2;
    private Vector3 _finalPosition2;
    private float _ratio = 0f;
    private float _x = 0f;
    private float _sign = 1f;

    private int _ballNumber = 0;

	void Awake ()
    {
        _initialPosition1 = LeftArrow.localPosition;
        _finalPosition1 = _initialPosition1 + new Vector3(0f, -0.5f, 0f);

        _initialPosition2 = RightArrow.localPosition;
        _finalPosition2 = _initialPosition2 + new Vector3(0f, -0.5f, 0f);
	}
	
	void Update ()
    {
        if (_ballNumber >= 2)
        {
            Application.LoadLevel("Level0");
        }

        _x += Time.deltaTime * _sign;
        _ratio = Mathf.Abs(Mathf.Sin(_x));

        if (_x >= 2 * Mathf.PI)
        {
            _ratio = 0f;
            _x = 0f;
        }

        LeftArrow.localPosition = Vector3.Lerp(_initialPosition1, _finalPosition1, _ratio);
        RightArrow.localPosition = Vector3.Lerp(_initialPosition2, _finalPosition2, _ratio);
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.collider.enabled = false;
        coll.rigidbody.isKinematic = true;
        _ballNumber++;
    }
}
