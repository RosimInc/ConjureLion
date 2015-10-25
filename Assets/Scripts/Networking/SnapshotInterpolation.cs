using UnityEngine;
using System.Collections;
using System.Reflection;

// Implemententation based on the following material: http://gafferongames.com/networked-physics/snapshots-and-interpolation/
// Gamasutra article (not used yet): http://www.gamasutra.com/blogs/DarrelCusey/20130221/187128/Unity_Networking_Sample_Using_One_NetworkView.php

/*
 * This class uses a variation "Template Method" design pattern to manage all the interpolation and snapshots
 * buffering needs between the client and the server. This uses the Snapshots and Interpolation technique with a buffer to reduce the visual lag.
 * 
 * To use this class, you must first inherit it and implement the 3 methods UpdateData, SendData and ParseReceivedData. You must also inherit the
 * Snapshot class and add the data you want to send and receive over the network.
 * 
 * The main template method InterpolateBufferSnapshots does the heavy lifting with the operation UpdateData,
 * while the method OnSerializeNetworkView uses SendData and ParseReceivedData to serialize and parse data over the network.
 */
public abstract class SnapshotInterpolation : MonoBehaviour
{
    public Player Player;

    /// <summary>
    /// Class to be completed in the implementations
    /// </summary>
    public class Snapshot
    {
        public double timestamp;
    }

    protected Snapshot[] _snapshotBuffer = new Snapshot[99];

    /*
    // Needs to be unique per child in the scene

    protected abstract string RPCCallName { get; }*/

    // Operations
    protected abstract void UpdateData(Snapshot olderSnapshot, Snapshot recentSnapshot, float interpolationRatio);
    protected abstract void SendData(BitStream stream);
    protected abstract Snapshot ParseReceivedData(BitStream stream);

    protected virtual void Awake()
    {
        if (GetComponent<NetworkView>() == null)
        {
            gameObject.AddComponent<NetworkView>();
        }

        // We send the data 60 times per second
        Network.sendRate = 60;

        GetComponent<NetworkView>().observed = this;
        GetComponent<NetworkView>().stateSynchronization = NetworkStateSynchronization.Unreliable;

        if (Network.isServer)
        {
            if (Player.Number == 1)
            {
                //networkView.viewID = Network.AllocateViewID();
                Player.ComputerID = 0;
                //NetworkManager.Instance.ChangeOwnership(Player.ComputerID, networkView.viewID);
            }
            else
            {
                Player.Number = -1;
                Player.ComputerID = 1;
            }
        }
        else if (Network.isClient)
        {
            if (Player.Number == 2)
            {
                GetComponent<NetworkView>().viewID = Network.AllocateViewID();
                Player.Number = 1;
                Player.ComputerID = 1;
                NetworkManager.Instance.ChangeOwnership(Player.ComputerID, GetComponent<NetworkView>().viewID);
            }
            else
            {
                Player.Number = -1;
                Player.ComputerID = 0;
            }
        }
    }

    // Called by remote clients
    protected void Update()
    {
        /*
        // TODO: Only send when a player connects
        if (Network.isServer)
        {
            networkView.RPC("SpawnObject", RPCMode.AllBuffered, networkView.viewID);
        }
        else
        {
            InterpolateBufferSnapshots();
        }*/

        if (!GetComponent<NetworkView>().isMine)
        {
            InterpolateBufferSnapshots();
        }
    }

    /*
    [RPC]
    void SpawnObject(NetworkViewID viewID)
    {
        networkView.viewID = viewID;
    }*/

    /// <summary>
    /// Main template method
    /// </summary>
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

                    UpdateData(_snapshotBuffer[i - 1], _snapshotBuffer[i - 1], 1);

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
                    UpdateData(olderSnapshot, recentSnapshot, t);
                    return;
                }
            }
        }
        else
        {
            // We extrapolate by using the data we have from the last snapshot we received, but we don't interpolate
            UpdateData(_snapshotBuffer[0], _snapshotBuffer[0], 1);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            // Outgoing data
            SendData(stream);
        }
        else
        {
            // Incoming data
            Snapshot snapshot = ParseReceivedData(stream);

            // We delay the buffer by 50% of the current ping average
            float pingInSeconds = Network.GetAveragePing(Network.connections[0]) * 0.001f;
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
