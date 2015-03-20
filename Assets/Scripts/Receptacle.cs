using UnityEngine;
using System.Collections;

public class Receptacle : MonoBehaviour
{
    public Transform Arrow;

    private Vector3 _initialPosition;
    private Vector3 _finalPosition;
    private float _ratio = 0f;
    private float _x = 0f;
    private float _sign = 1f;

    void Awake()
    {
        _initialPosition = Arrow.localPosition;
        _finalPosition = _initialPosition + new Vector3(0f, -0.5f, 0f);
    }

    void Update()
    {
        _x += Time.deltaTime * _sign;
        _ratio = Mathf.Abs(Mathf.Sin(_x));

        if (_x >= 2 * Mathf.PI)
        {
            _ratio = 0f;
            _x = 0f;
        }

        Arrow.localPosition = Vector3.Lerp(_initialPosition, _finalPosition, _ratio);
    }
}
