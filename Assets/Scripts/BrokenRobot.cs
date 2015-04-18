using UnityEngine;
using System.Collections;
using System;

public abstract class BrokenRobot : MonoBehaviour
{
	public ParticleSystem WindParticles;
	public float MinimumParticlesVelocity;
	public float MaximumParticlesVelocity;
	public Transform Body;

	protected bool _isActivated = false;

	private float _previousParticlesVelocity = 0f;
    private bool _isPlayingBreath = false;

	void Awake()
	{
		_previousParticlesVelocity = WindParticles.startSpeed;
	}

	void Update()
	{
        float maxTriggerValue = 0f;

        if (UnityEngine.Random.Range(0f, 1f) < 0.005f)
        {
            maxTriggerValue = 0.1f;
            Debug.Log("abc");
        }
        
		if (!_isPlayingBreath && maxTriggerValue > 0f)
		{
            Debug.Log("def");
            StartCoroutine(PlayBreath(maxTriggerValue));
		}
	}

    private IEnumerator PlayBreath(float triggerValue)
    {
        Debug.Log("ghi");

        _isPlayingBreath = true;

        float elapsedTime = 0f;

        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;

            WindParticles.startSpeed = MinimumParticlesVelocity + triggerValue * (MaximumParticlesVelocity - MinimumParticlesVelocity);

            float ratio = WindParticles.startSpeed / _previousParticlesVelocity;

            WindParticles.startLifetime /= ratio;
            WindParticles.emissionRate *= ratio;

            if (!_isActivated)
            {
                ActivateAbility(true);
            }

            _previousParticlesVelocity = WindParticles.startSpeed;

            yield return null;
        }

        ActivateAbility(false);

        _isPlayingBreath = false;
    }

	public void ActivateAbility(bool state)
	{
        _isActivated = state;

		if (state)
		{
			WindParticles.Play();

            if (this is BrokenSouffli)
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
            if (this is BrokenSouffli)
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
}
