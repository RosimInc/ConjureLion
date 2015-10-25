using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

public class DeadBodyInterpolation : SnapshotInterpolation
{
    public class DeadBodySnapshot : SnapshotInterpolation.Snapshot
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    private Transform _transform;
    private DeadBodySnapshot _snapshot;

    protected override void Awake()
    {
        base.Awake();

        _transform = GetComponent<Transform>();
        _snapshot = new DeadBodySnapshot();
    }

    protected override void UpdateData(Snapshot olderSnapshot, Snapshot recentSnapshot, float interpolationRatio)
    {
        DeadBodySnapshot olderCharacterSnapshot = olderSnapshot as DeadBodySnapshot;
        DeadBodySnapshot recentCharacterSnapshot = recentSnapshot as DeadBodySnapshot;

        DeadBodySnapshot interpolatedSnapshot = new DeadBodySnapshot();

        interpolatedSnapshot.position = Vector3.Lerp(olderCharacterSnapshot.position, recentCharacterSnapshot.position, interpolationRatio);
        interpolatedSnapshot.rotation = Vector3.Lerp(olderCharacterSnapshot.rotation, recentCharacterSnapshot.rotation, interpolationRatio);

        _transform.position = interpolatedSnapshot.position;
        _transform.eulerAngles = interpolatedSnapshot.rotation;
    }

    protected override void SendData(BitStream stream)
    {
        Vector3 position = _transform.position;
        Vector3 rotation = _transform.eulerAngles;

        stream.Serialize(ref position);
        stream.Serialize(ref rotation);
    }

    protected override SnapshotInterpolation.Snapshot ParseReceivedData(BitStream stream)
    {
        stream.Serialize(ref _snapshot.position);
        stream.Serialize(ref _snapshot.rotation);

        return _snapshot;
    }
}
