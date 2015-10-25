using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

[RequireComponent(typeof(PlayerMovement))]
public class CharacterSelectorInterpolation : SnapshotInterpolation
{
    public class CharacterSelectorSnapshot : SnapshotInterpolation.Snapshot
    {
        public float progress;
    }

    private PlayerMovement _playerMovement;
    private CharacterSelectorSnapshot _snapshot;

    protected override void Awake()
    {
        base.Awake();

        _playerMovement = GetComponent<PlayerMovement>();
        _snapshot = new CharacterSelectorSnapshot();
    }

    protected override void UpdateData(Snapshot olderSnapshot, Snapshot recentSnapshot, float interpolationRatio)
    {
        CharacterSelectorSnapshot olderCharacterSnapshot = olderSnapshot as CharacterSelectorSnapshot;
        CharacterSelectorSnapshot recentCharacterSnapshot = recentSnapshot as CharacterSelectorSnapshot;

        CharacterSelectorSnapshot interpolatedSnapshot = new CharacterSelectorSnapshot();

        interpolatedSnapshot.progress = Mathf.Lerp(olderCharacterSnapshot.progress, recentCharacterSnapshot.progress, interpolationRatio);

        _playerMovement.Progress = interpolatedSnapshot.progress;
    }

    protected override void SendData(BitStream stream)
    {
        float progress = _playerMovement.Progress;

        stream.Serialize(ref progress);
    }

    protected override SnapshotInterpolation.Snapshot ParseReceivedData(BitStream stream)
    {
        stream.Serialize(ref _snapshot.progress);

        return _snapshot;
    }
}
