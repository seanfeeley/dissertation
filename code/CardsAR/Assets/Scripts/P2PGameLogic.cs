// Copyright 2022 Niantic, Inc. All Rights Reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.ARDK.AR;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.HitTest;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.BinarySerialization;
using Niantic.ARDK.Utilities.Input.Legacy;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System;

public class P2PGameLogic : MonoBehaviour
{
    // A reference to the Camera component, set in the Editor.
    public Camera _camera;

    // A reference to the car prefab, set in the Editor.
    public GameObject _avatarPrefab;

    // A reference to the local car, created on the first tap after connecting to the session.
    private GameObject _localAvatar;

    // A constant representing a position event tag.
    private const uint AVATAR_POS_UPDATE_EVENT = 1;
    private const uint AVATAR_ROT_UPDATE_EVENT= 2;
    private const uint CARD_EVENT = 3;

    // The world tracking configuration, created during OnEnable().
    private IARWorldTrackingConfiguration _arWorldTrackingConfiguration;

    // The multipeer networking, created during OnEnable().
    private IMultipeerNetworking _multipeerNetworking;

    // The AR session, created during OnEnable().
    private IARSession _arSession;

    // The AR networking session, created during OnEnable().
    private IARNetworking _arNetworkingSession;

    // The session identifier.
    private readonly byte[] _sessionID = Encoding.UTF8.GetBytes("SampleSessionID");

    // A dictionary list of the players and their car GameObjects.
    private Dictionary<System.Guid, GameObject> _players = new Dictionary<System.Guid, GameObject>();

    
    private System.Guid self;


    private void OnEnable()
    {
        // First, create and configure the world tracking configuration...
        _arWorldTrackingConfiguration = ARWorldTrackingConfigurationFactory.Create();
        _arWorldTrackingConfiguration.WorldAlignment = WorldAlignment.Gravity;
        _arWorldTrackingConfiguration.IsLightEstimationEnabled = true;
        _arWorldTrackingConfiguration.PlaneDetection = PlaneDetection.Horizontal;
        _arWorldTrackingConfiguration.IsAutoFocusEnabled = true;
        _arWorldTrackingConfiguration.IsDepthEnabled = false;
        _arWorldTrackingConfiguration.IsSharedExperienceEnabled = false;

        // ...next, create the multipeer networking...
        _multipeerNetworking = MultipeerNetworkingFactory.Create();

        // ...then, create the AR session, passing in the stage identifier...
        _arSession = ARSessionFactory.Create(_multipeerNetworking.StageIdentifier);

        // ...next, create the AR networking session, passing in the AR session and the multipeer networking...
        _arNetworkingSession = ARNetworkingFactory.Create(_arSession, _multipeerNetworking);

        // ...finally, subscribe callback methods
        _multipeerNetworking.Connected += OnConnected;
        _multipeerNetworking.ConnectionFailed += OnConnectionFailed;
        _multipeerNetworking.Disconnected += OnDisconnected;
        _multipeerNetworking.PeerAdded += OnPeerAdded;
        _multipeerNetworking.PeerRemoved += OnPeerRemoved;
        _multipeerNetworking.PeerDataReceived += OnPeerDataReceived;
    }

    private void OnDisable()
    {
        // Leave the networking session...
        LeaveNetworkingSession();

        // ...unsubscribe callback methods...
        _multipeerNetworking.Connected -= OnConnected;
        _multipeerNetworking.ConnectionFailed -= OnConnectionFailed;
        _multipeerNetworking.Disconnected -= OnDisconnected;
        _multipeerNetworking.PeerAdded -= OnPeerAdded;
        _multipeerNetworking.PeerRemoved -= OnPeerRemoved;
        _multipeerNetworking.PeerDataReceived -= OnPeerDataReceived;

        // ...and dispose of the objects.
        _arWorldTrackingConfiguration.Dispose();
        _multipeerNetworking.Dispose();
        _arSession.Dispose();
        _arNetworkingSession.Dispose();
    }

    private void OnApplicationQuit()
    {
        // Leave the networking session.
        LeaveNetworkingSession();
    }

    void Start()
    {
        // Run the AR networking session and join with the session ID...
        _arNetworkingSession.ARSession.Run(_arWorldTrackingConfiguration);
        _arNetworkingSession.Networking.Join(_sessionID);
    }

    void Update()
    {
        BroadcastCameraPos();
        //BroadcastCameraRot();
    }

    // Creates a car at a given position.
    private GameObject CreateAvatar(Vector3 position)
    {
        GameObject createdAvatar = Instantiate(_avatarPrefab, _avatarPrefab.transform);
        createdAvatar.transform.position = position;
        return createdAvatar;
    }

    // Broadcasts this car's position at a given position.
    private void BroadcastCameraPos()
    {
        // First, create a new memory stream object...
        MemoryStream memoryStream = new MemoryStream();

        // ...then, serialize the position into the memory stream...
        GlobalSerializer.Serialize(memoryStream, _camera.transform.position);

        // ...next, store the memory stream as a byte array...
        byte[] data = memoryStream.ToArray();

        // ...and finally, send the byte array to all peers on the network.
        _arNetworkingSession.Networking.SendDataToPeers(tag: AVATAR_POS_UPDATE_EVENT,
                                                        data: data,
                                                        peers: _arNetworkingSession.Networking.OtherPeers,
                                                        TransportType.ReliableOrdered);
    }

    // Broadcasts this car's position at a given position.
    private void BroadcastCameraRot()
    {
        // First, create a new memory stream object...
        MemoryStream memoryStream = new MemoryStream();

        // ...then, serialize the position into the memory stream...
        GlobalSerializer.Serialize(memoryStream, _camera.transform.rotation);

        // ...next, store the memory stream as a byte array...
        byte[] data = memoryStream.ToArray();

        // ...and finally, send the byte array to all peers on the network.
        _arNetworkingSession.Networking.SendDataToPeers(tag: AVATAR_ROT_UPDATE_EVENT,
                                                        data: data,
                                                        peers: _arNetworkingSession.Networking.OtherPeers,
                                                        TransportType.ReliableOrdered);
    }

    // Leaves the multiplayer session, if applicable.
    private void LeaveNetworkingSession()
    {
        if (_multipeerNetworking.IsConnected) _multipeerNetworking.Leave();
    }

    // Subscribes to the _multipeerNetworking.Connected event handler during OnEnable().
    private void OnConnected(ConnectedArgs args)
    {
        Debug.Log("Lightship: ---P2PGameLogic.OnConnected()---");
        Debug.Log("Lightship: args.IsHost = " + args.IsHost);
    }

    // Subscribes to the _multipeerNetworking.ConnectionFailed event handler during OnEnable().
    private void OnConnectionFailed(ConnectionFailedArgs args)
    {
        Debug.Log("Lightship: ---P2PGameLogic.OnConnectionFailed()---");
        Debug.Log("Lightship: args.ErrorCode = " + args.ErrorCode);
    }

    // Subscribes to the _multipeerNetworking.Disconnected event handler during OnEnable().
    private void OnDisconnected(DisconnectedArgs args)
    {
        Debug.Log("Lightship: ---P2PGameLogic.OnDisconnected()---");
    }

    // Subscribes to the _multipeerNetworking.PeerAdded event handler during OnEnable().
    private void OnPeerAdded(PeerAddedArgs args)
    {
        Debug.Log("Lightship: ---P2PGameLogic.OnPeerAdded()---");
        Debug.Log("Lightship: args.Peer = " + args.Peer);

        // Broadcast this car's position when a peer joins the session.
        if (_localAvatar != null)
        {
            BroadcastCameraPos();
        }
    }

    // Subscribes to the _multipeerNetworking.PeerRemoved event handler during OnEnable().
    private void OnPeerRemoved(PeerRemovedArgs args)
    {
        Debug.Log("Lightship: ---P2PGameLogic.OnPeerRemoved()---");
        Debug.Log("Lightship: args.Peer = " + args.Peer);

        // If the dictionary of players contains the peer that's been removed...
        if (_players.ContainsKey(args.Peer.Identifier))
        {
            // ...get a reference to that player's car...
            GameObject peerAvatar;
            if (_players.TryGetValue(args.Peer.Identifier, out peerAvatar))
            {
                // ...destroy the car...
                Destroy(peerAvatar);
            }
            // ...and remove that player from the dictionary.
            _players.Remove(args.Peer.Identifier);
        }
    }

    // Subscribes to the _multipeerNetworking.PeerDataReceived event handler during OnEnable().
    private void OnPeerDataReceived(PeerDataReceivedArgs args)
    {
        Debug.Log("Lightship: ---P2PGameLogic.OnPeerDataReceived()---");
        Debug.Log("Lightship: args.Peer = " + args.Peer);
        Debug.Log("Lightship: args.Tag = " + args.Tag);
        Debug.Log("Lightship: args.DataLength = " + args.DataLength);
        Debug.Log("Lightship: args.TransportType = " + args.TransportType);

        // First, copy the argument data into a memory stream...
        MemoryStream memoryStream = new MemoryStream(args.CopyData());
        // ...then, take note of the player identifier...
        System.Guid playerIdentifier = args.Peer.Identifier;

        switch (args.Tag)
        {
            // In the case of a position event...
            case AVATAR_POS_UPDATE_EVENT:
                ProcessPositionData(playerIdentifier, memoryStream);
                break;
            // In the case of a position event...
            case AVATAR_ROT_UPDATE_EVENT:
                ProcessRotationData(playerIdentifier, memoryStream);
                break;
        }
    }

    private void ProcessRotationData(Guid playerIdentifier, MemoryStream memoryStream)
    {
        // ...deserialize the position from the memory stream...
        Quaternion rotation = (Quaternion)GlobalSerializer.Deserialize(memoryStream);

        // ...and if the dictionary already contains the player...
        if (_players.ContainsKey(playerIdentifier))
        {
            // ...then set the player's avatar postition and rotation.
            _players[playerIdentifier].transform.rotation = rotation;
        }
    }

    private void ProcessPositionData(Guid playerIdentifier, MemoryStream memoryStream)
    {

        // ...deserialize the position from the memory stream...
        Vector3 position = (Vector3)GlobalSerializer.Deserialize(memoryStream);

        // ...and if the dictionary already contains the player...
        if (_players.ContainsKey(playerIdentifier))
        {
            // ...then set the player's avatar postition and rotation.
            _players[playerIdentifier].transform.position = position;
        }
        // ...but if the dictionary does not contain the player...
        else
        {
            // ...then create an avatar for the remote player...
            GameObject remoteAvatar = CreateAvatar(position);

            // ...and add it to the dictionary.
            _players.Add(playerIdentifier, remoteAvatar);
        }
    }

    public int GetMyPlayerIndex()
    {
        return GetPeerPlayerIndex(self);
    }
    public int GetPeerPlayerIndex(System.Guid peer)
    {
        List<Guid> peer_list = new List<Guid>();
        foreach (System.Guid ip in this._players.Keys)
        {
            peer_list.Add(ip);
        }
        peer_list.Sort();
        return peer_list.IndexOf(peer);
    }

    public int GetCurrentPlayerCount()
    {
        return this._players.Count;
    }
}