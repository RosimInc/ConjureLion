using UnityEngine;
using System.Collections;

public abstract class Robot : MonoBehaviour
{
    public int PlayerNumber;
    public ParticleSystem WindParticles;
    public float MinimumParticlesVelocity;
    public float MaximumParticlesVelocity;
    public float rotateSpeed = 1f;
    public Transform Body;

    protected bool _isActivated = false;

    private float _previousParticlesVelocity = 0f;

    private float _targetAngle;
    private float _ratio = 0f;

    void Awake()
    {
        _previousParticlesVelocity = WindParticles.startSpeed;
        _targetAngle = Body.localEulerAngles.z;
    }

    protected void Update()
    {
        /*
        _ratio = Time.deltaTime / 1f;

        float minAngle = Mathf.Min(Body.localEulerAngles.z, _targetAngle);
        float maxAngle = Mathf.Max(Body.localEulerAngles.z, _targetAngle);
        
        float newAngle = Mathf.LerpAngle(minAngle, maxAngle, maxAngle / minAngle);

        Debug.Log(minAngle);
        Debug.Log(maxAngle);*/
        /*
        if (_targetAngle != Body.localEulerAngles.z)
        {
            Body.localEulerAngles = new Vector3(0f, 0f, newAngle);
        }*/

        float maxTriggerValue = 0f;

        if (PlayerNumber == 1)
        {
            maxTriggerValue = Mathf.Max(Input.GetAxisRaw("TriggersL_1"), Input.GetAxisRaw("TriggersR_1"));

            _targetAngle = -(Mathf.Atan2(Input.GetAxisRaw("R_YAxis_1"), Input.GetAxisRaw("R_XAxis_1")) * Mathf.Rad2Deg) - 90f;
        }
        else
        {
            maxTriggerValue = Mathf.Max(Input.GetAxisRaw("TriggersL_2"), Input.GetAxisRaw("TriggersR_2"));

            _targetAngle = -Mathf.Atan2(Input.GetAxisRaw("R_YAxis_2"), Input.GetAxisRaw("R_XAxis_2")) * Mathf.Rad2Deg;
        }

        Body.localEulerAngles = new Vector3(0f, 0f, _targetAngle);

        if (maxTriggerValue > 0f)
        {
            WindParticles.startSpeed = MinimumParticlesVelocity + maxTriggerValue * MaximumParticlesVelocity;

            float ratio = WindParticles.startSpeed / _previousParticlesVelocity;

            WindParticles.startLifetime /= ratio;
            WindParticles.emissionRate *= ratio;
            Debug.Log(_isActivated);
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
        Debug.Log("abc");
        _isActivated = state;

        if (state)
        {
            Debug.Log("abcd");
            WindParticles.Play();
        }
        else
        {
            WindParticles.Stop();
            Debug.Log("def");
        }
    }
}
