using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float FullRotationTime;
    public bool ClockWise = true;
    public bool TimeScaleDependant;

    private Transform _cachedTransform;
    private int _clockWiseSign;

    void Awake()
    {
        _cachedTransform = transform;
        _clockWiseSign = ClockWise ? -1 : 1;
    }

    void Update()
    {
        float rotationValue = 360 / FullRotationTime * _clockWiseSign; 

        rotationValue *= TimeScaleDependant ? Time.deltaTime : Time.unscaledDeltaTime;

        _cachedTransform.Rotate(new Vector3(0f, 0f, rotationValue));
    }
}
