using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour
{
    public Transform StartPoint;
    public Transform EndPoint;
    public LineRenderer BeamLine;
    public ParticleSystem BeamParticles;

    public Transform StartObject;
    public Transform EndObject;

    private float LIFETIME_RATIO = 0.025f;

    void Awake()
    {
        BeamLine.SetWidth(0.2f, 0.2f);
    }

    void Update()
    {
        UpdateVisualEffects();
    }

    public void Activate(bool state)
    {
        BeamLine.gameObject.SetActive(state);

        if (state)
        {
            BeamParticles.gameObject.SetActive(true);
            BeamParticles.Play();
        }
        else
        {
            BeamParticles.Stop();
            BeamParticles.gameObject.SetActive(false);
        }

        // We need to re-update after changing the state of the visuals since the transform of the targetted robot may have changed
        UpdateVisualEffects();
    }

    private void UpdateVisualEffects()
    {
        StartPoint.position = StartObject.position + new Vector3(0f, 0f, -0.5f);
        EndPoint.position = EndObject.position + new Vector3(0f, 0f, -0.5f);

        float angle = Vector3.Angle(EndPoint.position - StartPoint.position, transform.right);

        angle = EndPoint.position.y > StartPoint.position.y ? angle : -angle;

        float distance = Vector3.Magnitude(EndPoint.position - StartPoint.position);

        BeamParticles.startLifetime = distance * LIFETIME_RATIO;

        StartPoint.eulerAngles = new Vector3(0f, 0f, angle);

        BeamLine.SetPosition(0, StartPoint.position);
        BeamLine.SetPosition(1, EndPoint.position);
    }
}
