using UnityEngine;
using System.Collections;

public class Pulse : MonoBehaviour {

    public AnimationCurve PulsingCurve;

    private float _pulsingTime = 0f;

	void Update ()
    {
        _pulsingTime += Time.deltaTime;

        if (_pulsingTime > PulsingCurve.keys[PulsingCurve.length - 1].time)
        {
            _pulsingTime = 0f;
        }

        float newScale = PulsingCurve.Evaluate(_pulsingTime);

        transform.localScale = new Vector3(newScale, newScale, newScale);
	}
}
