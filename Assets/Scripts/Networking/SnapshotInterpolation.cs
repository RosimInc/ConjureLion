using UnityEngine;
using System.Collections;
using System.Reflection;

// Implemententation based on the following material: http://gafferongames.com/networked-physics/snapshots-and-interpolation/

// Gamasutra article: http://www.gamasutra.com/blogs/DarrelCusey/20130221/187128/Unity_Networking_Sample_Using_One_NetworkView.php

// Snapshot and interpolation technique

public class SnapshotInterpolation : MonoBehaviour
{
    private Snapshot[] _snapshotBuffer = new Snapshot[99];

    private class Snapshot
    {
        public double timestamp;
        public Vector2 position;
        public bool facingRight;
        public float progress;
    }

    void Awake()
    {
        /*
         * Size of a character over the network: 4*2 + 1 = 9 bits
         * Data sent per second, per character: 9*60 = 540 bits/sec
         * Data sent per second, for 4 characters: 540*4 = 2160 bits/sec or 2.16 kbits/sec
         */

        // Since the sent data size is 9 bit (2 floats + 1 bool), we are sending 540 bits/sec per character
        Network.sendRate = 60;

        if (Network.isServer)
        {
            Debug.Log("IS SERVER!!!");
            NetworkViewID viewID = Network.AllocateViewID();
        }
    }

    [RPC]
    void SpawnObject(NetworkViewID viewID)
    {
        //Debug.Log("Spawning game object");
        this.GetComponent<NetworkView>().viewID = viewID;
    }

    // Called by remote clients
    void Update()
    {
        if (Network.isServer)
        {
            networkView.RPC("SpawnObject", RPCMode.AllBuffered, networkView.viewID);

            return;
        }

        InterpolateBufferSnapshots();
    }

    private void InterpolateBufferSnapshots()
    {
        if (_snapshotBuffer[0] == null)
        {
            return;
        }

        float pingInSeconds = Network.GetAveragePing(Network.connections[0]) * 0.001f;

        // We max out the delay at 100ms, but we can adjust it if it's too low and we see some jitter
        float interpolationDelay = Mathf.Max(0.1f, pingInSeconds);

        double interpolationTime = Network.time - interpolationDelay;

        if (_snapshotBuffer[0].timestamp > interpolationTime)
        {
            for (int i = 1; i <= _snapshotBuffer.Length; i++)
            {
                // If this was the latest snapshot, we don't go further (it means we lag a lot and we might see some jittering)
                if (i == _snapshotBuffer.Length || _snapshotBuffer[i] == null)
                {
                    Snapshot interpolatedSnapshot = new Snapshot();

                    UpdateTransform(_snapshotBuffer[i - 1]);

                    return;
                }

                // If the snapshot is too recent, we go to the next one since we want a reasonable buffer to avoid visual jitter
                if (_snapshotBuffer[i].timestamp <= interpolationTime)
                {
                    //With really low ping, the index value will never be above 2 or 3

                    Snapshot olderSnapshot = _snapshotBuffer[i];
                    Snapshot recentSnapshot = _snapshotBuffer[i - 1];

                    // We interpolate between the 2 snapshots until we catch up with the delay
                    float difference = (float)(recentSnapshot.timestamp - olderSnapshot.timestamp);

                    float t = 0.0f;

                    if (difference > 0.001f)
                    {
                        t = (float)((interpolationTime - olderSnapshot.timestamp) / difference);
                    }

                    Snapshot interpolatedSnapshot = new Snapshot();

                    
                    interpolatedSnapshot.position = Vector3.Lerp(olderSnapshot.position, recentSnapshot.position, t);
                    interpolatedSnapshot.facingRight = recentSnapshot.facingRight;
                    interpolatedSnapshot.progress = Mathf.Lerp(olderSnapshot.progress, recentSnapshot.progress, t);

                    UpdateTransform(interpolatedSnapshot);

                    // Possible Cubic Hermite Spline implementation, but not needed for now

                    /*
                    Vector3 p0 = olderSnapshot.position;
                    Vector3 p1 = recentSnapshot.position;
                    Vector3 v0 = olderSnapshot.velocity;
                    Vector3 v1 = recentSnapshot.velocity;

                    float h00 = (t * t) * (2 * t - 3) + 1;
                    float h10 = (t * t) * (t - 2) + t;
                    float h01 = (t * t) * (-2 * t + 3);
                    float h11 = (t * t) * (t - 1);


                    p0 *= 1000;
                    p1 *= 1000;

                    Vector3 position = h00 * p0 + h10 * v0 + h01 * p1 + h11 * v1;

                    position /= 1000;

                    interpolatedSnapshot.position = position;
                    interpolatedSnapshot.facingRight = recentSnapshot.facingRight;

                    UpdateTransform(interpolatedSnapshot);*/

                    return;
                }
            }
        }
        else
        {
            // We extrapolate by using the data we have from the last snapshot we received
            UpdateTransform(_snapshotBuffer[0]);
        }
    }

    private void UpdateTransform(Snapshot snapshot)
    {
        /*
        float xScale = snapshot.facingRight ? Mathf.Abs(transform.localScale.x) : -Mathf.Abs(transform.localScale.x);

        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        transform.localPosition = snapshot.position;*/

        transform.gameObject.GetComponent<PlayerMovement>().Progress = snapshot.progress;
    }

    // Called by the server
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        float positionX = 0f; // 4 bits
        float positionY = 0f; // 4 bits
        bool facingRight = false; // 1 bit
        float progress = 0f;

        if (stream.isWriting)
        {
            // Outgoing data

            positionX = transform.localPosition.x;
            positionY = transform.localPosition.y;
            facingRight = transform.localScale.x > 0;
            progress = transform.gameObject.GetComponent<PlayerMovement>().Progress;

            stream.Serialize(ref positionX); // 4 bits
            stream.Serialize(ref positionY); // 4 bits
            stream.Serialize(ref facingRight); // 1 bit
            stream.Serialize(ref progress);

            Debug.Log("Sending progress: " + progress);
        }
        else
        {
            // Incoming data

            stream.Serialize(ref positionX); // 4 bits
            stream.Serialize(ref positionY);  // 4 bits
            stream.Serialize(ref facingRight); // 1 bit
            stream.Serialize(ref progress);

            Snapshot snapshot = new Snapshot();
            snapshot.position = new Vector2(positionX, positionY);
            snapshot.facingRight = facingRight;
            snapshot.progress = progress;

            float pingInSeconds = Network.GetAveragePing(Network.connections[0]) * 0.001f;

            // We delay the buffer by 50% of the current ping average
            snapshot.timestamp = info.timestamp - pingInSeconds / 2;

            InsertSnapshot(snapshot);
        }
    }

    private void InsertSnapshot(Snapshot snapshot)
    {
        int index = 0;
        
        // We order the snapshots from the most recent ones (left) to the oldest ones (right)
        while (index < _snapshotBuffer.Length - 1 && _snapshotBuffer[index] != null && snapshot.timestamp < _snapshotBuffer[index].timestamp)
        {
            index++;
        }

        _snapshotBuffer[index] = snapshot;

        // We insert the snapshot and the oldest ones get automatically pushed out
        for (int i = _snapshotBuffer.Length - 1; i > index; i--)
        {
            _snapshotBuffer[i] = _snapshotBuffer[i - 1];
        }
    }
}
