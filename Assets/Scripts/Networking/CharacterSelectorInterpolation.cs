using UnityEngine;
using System.Collections;

public class CharacterSelectorInterpolation : SnapshotInterpolation
{
    public PlayerMovement PlayerMovement;

    public class CharacterSelectorSnapshot : SnapshotInterpolation.Snapshot
    {
        public float progress;
    }

    protected override void UpdateData(Snapshot olderSnapshot, Snapshot recentSnapshot, float interpolationRatio)
    {
        CharacterSelectorSnapshot olderCharacterSnapshot = olderSnapshot as CharacterSelectorSnapshot;
        CharacterSelectorSnapshot recentCharacterSnapshot = recentSnapshot as CharacterSelectorSnapshot;

        CharacterSelectorSnapshot interpolatedSnapshot = new CharacterSelectorSnapshot();

        interpolatedSnapshot.progress = Mathf.Lerp(olderCharacterSnapshot.progress, recentCharacterSnapshot.progress, interpolationRatio);

        PlayerMovement.Progress = interpolatedSnapshot.progress;
    }

    protected override void SendData(BitStream stream)
    {
        float progress = PlayerMovement.Progress;

        stream.Serialize(ref progress);
    }

    protected override SnapshotInterpolation.Snapshot ParseReceivedData(BitStream stream)
    {
        float progress = 0f;

        stream.Serialize(ref progress);

        CharacterSelectorSnapshot snapshot = new CharacterSelectorSnapshot();
        snapshot.progress = progress;

        return snapshot;
    }
}
