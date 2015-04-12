using UnityEngine;
using System.Collections;

public class FloatingArrow : MonoBehaviour
{
    public enum AxisEnum { X, Y };

    public Vector3 Offset;
    public float Speed = 1f;

    private Vector3 _initialPosition;
    private Vector3 _finalPosition;

    private float _ratio = 0f;
    private float _x = 0f;

    void Awake()
    {
        _initialPosition = transform.localPosition;
        _finalPosition = _initialPosition + Offset;
    }

	void Update ()
    {
        _x += Time.deltaTime * Speed;

        if (_x >= Mathf.PI * 2)
        {
            _x = 0f;
        }

        _ratio = (Mathf.Sin(_x) + 1) / 2;

        transform.localPosition = Vector3.Lerp(_initialPosition, _finalPosition, _ratio);
	}

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            Vector3 initialPosition = transform.position;

            Vector3 size = new Vector3(renderer.bounds.extents.x * 2, renderer.bounds.extents.y * 2, 0f);

            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(initialPosition + Vector3.Scale(Offset, transform.lossyScale), size);
        }
    }

#endif

}
