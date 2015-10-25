using UnityEngine;
using System.Collections;
using System;
using InputHandling;

[RequireComponent(typeof(Player))]
public abstract class Robot : MonoBehaviour
{
	public ParticleSystem WindParticles;
	public float MinimumParticlesVelocity;
	public float MaximumParticlesVelocity;
	public float rotateSpeed = 1f;
	public Transform Body;

	protected bool _isActivated = false;
	private BallEffect _ballEffect;

	private float _previousParticlesVelocity = 0f;

	private float _targetAngle;

    private float _breathRatio = 0f; // Normalized between 0 and 1

	protected Player _player;
	

	void Awake()
	{
		_player = GetComponent<Player>();

		_previousParticlesVelocity = WindParticles.startSpeed;
		_targetAngle = Body.localEulerAngles.z;
	}

	protected virtual void Start()
	{
		_ballEffect = GetComponent<BallEffect>();

        // If the player number is negative, it is controlled by the network player
        if (_player.Number > 0)
        {
            InputManager.Instance.AddCallback(_player.Number - 1, RobotCallback);
        }

		if(WindParticles == null)
			Debug.Log ("Bug Particules");
	}

	protected virtual void Update()
	{
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[WindParticles.particleCount];

		int num = WindParticles.GetParticles(particles);

		for (int i = 0; i < num; i++)
		{
			particles[i].lifetime = 0;
		}

		//WindParticles.SetParticles(particles, num);

		//Debug.Log(num);

		if (Body.localEulerAngles.z < 0f)
		{
			Body.localEulerAngles = new Vector3(0f, 0f, Body.localEulerAngles.z + 360f);
		}

		if (_targetAngle < 0f)
		{
			_targetAngle += 360f;
		}

		float newAngle = Mathf.LerpAngle(Body.localEulerAngles.z, _targetAngle, Time.deltaTime * 10f);

		if (_targetAngle != Body.localEulerAngles.z)
		{
			Body.localEulerAngles = new Vector3(0f, 0f, newAngle);
		}

        if (_breathRatio > 0f)
		{
            WindParticles.startSpeed = MinimumParticlesVelocity + _breathRatio * (MaximumParticlesVelocity - MinimumParticlesVelocity);

			float ratio = WindParticles.startSpeed / _previousParticlesVelocity;

			WindParticles.startLifetime /= ratio;
			WindParticles.emissionRate *= ratio;

			if (!_isActivated)
			{
				ActivateAbility(true);
			}

			_previousParticlesVelocity = WindParticles.startSpeed;
		}
		else if (_isActivated)
		{
			ActivateAbility(false);
		}
	}

	public void ActivateAbility(bool state)
	{
		_isActivated = state;
		if(_ballEffect == null)
			_ballEffect = GetComponent<BallEffect>();

		if (_ballEffect != null)
		{
			_ballEffect.activated = state;
		}

		if (state)
		{
			WindParticles.Play();

			if (this is Souffli)
			{
				MusicManager.Instance.PlaySouffliBreathIn();
				StopCoroutine("PlaySouffliBreathLoop");
				StartCoroutine("PlaySouffliBreathLoop");
			}
			else
			{
				MusicManager.Instance.PlayAspiBreathIn();
				StopCoroutine("PlayAspiBreathLoop");
				StartCoroutine("PlayAspiBreathLoop");
			}
		}
		else
		{
			if (this is Souffli)
			{
				MusicManager.Instance.StopSouffliBreathLoop();
				MusicManager.Instance.StopSouffliBreathIn();
			}
			else
			{
				MusicManager.Instance.StopAspiBreathLoop();
				MusicManager.Instance.StopAspiBreathIn();
			}

			WindParticles.Stop();
		}
	}

	private IEnumerator PlaySouffliBreathLoop()
	{
		yield return new WaitForSeconds(MusicManager.Instance.SouffliBreathIn.time);
		
		if (_isActivated)
		{
			MusicManager.Instance.PlaySouffliBreathLoop();
		}
	}

	private IEnumerator PlayAspiBreathLoop()
	{
		yield return new WaitForSeconds(MusicManager.Instance.AspiBreathIn.time);

		if (_isActivated)
		{
			MusicManager.Instance.PlayAspiBreathLoop();
		}
	}

    // TODO: REMOVE, THIS IS ONLY FOR TESTS (should be put in the player controller Player)
    private void RobotCallback(MappedInput mappedInput)
    {
        if (mappedInput.Ranges.ContainsKey(InputConstants.BREATHE))
        {
            _breathRatio = mappedInput.Ranges[InputConstants.BREATHE];
        }

        float rotationX = 0f;
        float rotationY = 0f;

        if (mappedInput.Ranges.ContainsKey(InputConstants.ROTATE_X))
        {
            rotationX = mappedInput.Ranges[InputConstants.ROTATE_X];
        }

        if (mappedInput.Ranges.ContainsKey(InputConstants.ROTATE_Y))
        {
            rotationY = mappedInput.Ranges[InputConstants.ROTATE_Y];
        }

        _targetAngle = -(Mathf.Atan2(rotationX, rotationY) * Mathf.Rad2Deg);
    }
}
