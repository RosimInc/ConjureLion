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
        _ratio = Time.deltaTime / 1f;

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

        float maxTriggerValue = Mathf.Max(Input.GetAxisRaw("TriggersL_" + PlayerNumber), Input.GetAxisRaw("TriggersR_" + PlayerNumber));

        Debug.Log("TriggersL_" + PlayerNumber);

        if (Mathf.Abs(Input.GetAxisRaw("R_YAxis_" + PlayerNumber)) > 0f || Mathf.Abs(Input.GetAxisRaw("R_XAxis_" + PlayerNumber)) > 0f)
        {
            _targetAngle = -(Mathf.Atan2(Input.GetAxisRaw("R_YAxis_" + PlayerNumber), Input.GetAxisRaw("R_XAxis_" + PlayerNumber)) * Mathf.Rad2Deg) - 90f;
        }

        if (maxTriggerValue > 0f)
        {
            WindParticles.startSpeed = MinimumParticlesVelocity + maxTriggerValue * (MaximumParticlesVelocity - MinimumParticlesVelocity);

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
