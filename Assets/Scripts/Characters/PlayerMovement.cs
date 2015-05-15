using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InputHandling;

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

    private float _movementX;
    private float _movementY;

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
        InputManager.Instance.AddCallback(RobotCallback);
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
            Vector3 joystickDir = new Vector3(_movementX, _movementY, 0);

            float angle = Vector3.Angle(dir, joystickDir);

            int mod = 1;
            if (angle > 90)
            {
                mod = -1;
            }

            float stuff = Mathf.Sqrt(Mathf.Pow(_movementX, 2) + Mathf.Pow(_movementY, 2));

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

    // TODO: REMOVE, THIS IS ONLY FOR TESTS (should be put in the player controller Player)
    private void RobotCallback(MappedInput mappedInput)
    {
        // TODO: Find a way to reduce the number of callbacks called by only triggering when it's the correct player (dunno if possible)

        if (mappedInput.PlayerIndex != _player.Number - 1) return;

        if (mappedInput.Ranges.ContainsKey(ActionsConstants.Ranges.MoveX))
        {
            _movementX = mappedInput.Ranges[ActionsConstants.Ranges.MoveX];
        }

        if (mappedInput.Ranges.ContainsKey(ActionsConstants.Ranges.MoveY))
        {
            _movementY = mappedInput.Ranges[ActionsConstants.Ranges.MoveY];
        }
    }
}
