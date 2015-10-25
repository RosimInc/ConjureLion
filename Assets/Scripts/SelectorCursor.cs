using UnityEngine;
using System.Collections;

public class SelectorCursor : MonoBehaviour
{
    public enum CursorPosition { Left, Middle, Right };

    public GameObject LeftArrow;
    public GameObject RightArrow;
    public Vector3 CursorOffset;

    public CursorPosition Position
    {
        get { return _position; }
    }

    private const float ANIMATION_DURATION = 0.1f;

    private Vector3 _middlePosition;
    private Vector3 _leftPosition;
    private Vector3 _rightPosition;

    private CursorPosition _position = CursorPosition.Middle;

    private bool _isMoving = false;

    void Awake()
    {
        _middlePosition = transform.localPosition;
        _leftPosition = _middlePosition - CursorOffset;
        _rightPosition = _middlePosition + CursorOffset;
    }

    public void MoveRight()
    {
        if (_isMoving) return;

        switch (_position)
        {
            case CursorPosition.Left:
                _position = CursorPosition.Middle;
                LeftArrow.GetComponent<Renderer>().enabled = true;

                StartCoroutine(MoveToPosition(_middlePosition));
                break;
            case CursorPosition.Middle:
                _position = CursorPosition.Right;
                RightArrow.GetComponent<Renderer>().enabled = false;

                StartCoroutine(MoveToPosition(_rightPosition));
                break;
            case CursorPosition.Right:
                break;
        }
    }

    public void MoveLeft()
    {
        if (_isMoving) return;

        switch (_position)
        {
            case CursorPosition.Left:
                break;
            case CursorPosition.Middle:
                _position = CursorPosition.Left;
                LeftArrow.GetComponent<Renderer>().enabled = false;

                StartCoroutine(MoveToPosition(_leftPosition));
                break;
            case CursorPosition.Right:
                _position = CursorPosition.Middle;
                RightArrow.GetComponent<Renderer>().enabled = true;

                StartCoroutine(MoveToPosition(_middlePosition));
                break;
        }
    }

    private IEnumerator MoveToPosition(Vector3 position)
    {
        _isMoving = true;

        float ratio = 0f;

        Vector3 initialPosition = transform.localPosition;

        while (ratio < 1f)
        {
            ratio += Time.deltaTime / ANIMATION_DURATION;

            transform.localPosition = Vector3.Lerp(initialPosition, position, ratio);

            yield return null;
        }

        _isMoving = false;
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            float extentX = 0f;
            float extentY = 0f;

            foreach (Renderer child in transform.GetComponentsInChildren<Renderer>())
            {
                if (child.bounds.extents.x > extentX)
	            {
		            extentX = child.bounds.extents.x;
	            }

                if (child.bounds.extents.y > extentY)
                {
                    extentY = child.bounds.extents.y;
                }
            }

            Vector3 size = new Vector3(extentX * 2, extentY * 2, 0f);

            Gizmos.color = Color.green;

            Gizmos.DrawWireCube(transform.position + CursorOffset, size);
            Gizmos.DrawWireCube(transform.position - CursorOffset, size);
        }
    }

#endif
}
