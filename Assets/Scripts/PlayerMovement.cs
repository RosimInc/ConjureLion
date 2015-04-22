using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    public float Progress
    {
        get { return progress; }
        set { progress = value; }
    }

	public BezierSpline spline;
	public float speed = 1f;
	public float defaultProgress = 0f;
	public GameObject fix;
	private float progress;
    private float _previousProgress;

    private bool _playWhenPossible;

    private float PreviousMovement;

    private Player _player;

    void Awake()
    {
        _player = GetComponent<Player>();
        progress = defaultProgress;
    }

	void Start ()
    {
		Vector3 position = spline.GetPoint(progress);
		transform.localPosition = position;
	}
	
	private void Update () {
        //Debug.Log(progress);

        if (_playWhenPossible && !MusicManager.Instance.RailLoop.isPlaying)
        {
            MusicManager.Instance.PlayRailLoop();
            _playWhenPossible = false;
        }
		Vector3 dir = spline.GetDirection(progress);
        float x = InputManager.Instance.GetInputMovement(_player.Number).x;
        float y = InputManager.Instance.GetInputMovement(_player.Number).y;
		Vector3 joystickDir = new Vector3(x, y, 0);

		float angle = Vector3.Angle(dir, joystickDir);
		int mod = 1;
		if (angle > 90)
		{
			mod = -1;
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

        if (PreviousMovement == 0f && movement != 0f) // We start moving
        {
            if (MusicManager.Instance.RailLoop.isPlaying)
            {
                _playWhenPossible = true;
            }
            else
            {
                MusicManager.Instance.PlayRailLoop();
            }
        }
        else if (PreviousMovement != 0f && movement == 0f) // We stop moving
        {
            MusicManager.Instance.StopRailLoop();
            _playWhenPossible = false;
        }

        _previousProgress = progress;
        PreviousMovement = movement;
	}
}
