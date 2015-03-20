using UnityEngine;
using System.Collections;

public abstract class Robot : MonoBehaviour
{
    public int PlayerNumber;

    public ParticleSystem WindParticles;

    public float MinimumParticlesVelocity;
    public float MaximumParticlesVelocity;

    protected bool _isActivated;

    private float _initialParticlesVelocity = 10f;
    private float _initialParticlesLifetime = 1.35f;

    private float _previousTriggerValue = 0f;
    private float _previousParticlesVelocity = 0f;
	private BallEffect _ballEffect;

    void Awake()
    {
        _previousParticlesVelocity = WindParticles.startSpeed;
    }

	void Start()
	{
		_ballEffect = GetComponent<BallEffect>();
	}

    protected void Update()
    {
        float maxTriggerValue = 0f;

        Debug.Log(Input.GetAxisRaw("TriggersL_1"));

        if (PlayerNumber == 1)
        {
            maxTriggerValue = Mathf.Max(Input.GetAxisRaw("TriggersL_1"), Input.GetAxisRaw("TriggersR_1"));

            Debug.Log(Input.GetAxisRaw("TriggersL_1"));

            float angle = -Mathf.Atan2(Input.GetAxisRaw("R_YAxis_1"), Input.GetAxisRaw("R_XAxis_1")) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            maxTriggerValue = Mathf.Max(Input.GetAxisRaw("TriggersL_2"), Input.GetAxisRaw("TriggersR_2"));

            float angle = -Mathf.Atan2(Input.GetAxisRaw("R_YAxis_2"), Input.GetAxisRaw("R_XAxis_2")) * Mathf.Rad2Deg + 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (maxTriggerValue > 0f)
        {
            WindParticles.startSpeed = MinimumParticlesVelocity + maxTriggerValue * MaximumParticlesVelocity;

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
		_ballEffect.activated = state;

        if (state)
        {
            WindParticles.Play();
        }
        else
        {
            WindParticles.Stop();
        }
    }
}
