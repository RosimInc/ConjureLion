using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	private void Update ()
    {
        // TODO: Possibly split the network class from the offline ones to reduce complexit and instatiate via Network.Instantiate or the normal Instantiate

        if (_playWhenPossible && !MusicManager.Instance.RailLoop.isPlaying)
        {
            MusicManager.Instance.PlayRailLoop();
            _playWhenPossible = false;
        }
		Vector3 dir = spline.GetDirection(progress);

        // If the player number is -1, this computer doesn't have the rights to send inputs to this instance
        if (_player.Number != -1)
        {
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

            progress = spline.GetProgressFromDistance(progress, movement);
        }


        Vector3 position = spline.GetPoint(progress);
        position.z = 0;
        transform.localPosition = position;

		//TODO Here fix the movement so it uses MovePosition
		//rigidbody2D.MovePosition(transform.forward - position);
		
		//Orientation du fix
		fix.transform.LookAt(position + spline.GetDirection(progress) );
		fix.transform.Rotate(new Vector3(0,90,0));

        if (_previousProgress != progress) // We start moving
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
        else // We stop moving
        {
            MusicManager.Instance.StopRailLoop();
            _playWhenPossible = false;
        }

        _previousProgress = progress;
	}
}
