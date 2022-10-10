using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Utilities.BinarySerialization;
using Niantic.ARDK.Utilities.Input.Legacy;
using UnityEngine;

public class ARGameLogic : MonoBehaviour
{
    public Camera camera;
    public ARNetworkingManager manager;
    public GameObject objectPrefab;

    private const uint POSITION_EVT = 1;
    private const uint UPDATE_EVT = 2;

    private Dictionary<System.Guid, GameObject> players = new Dictionary<System.Guid, GameObject>();

    private GameObject me;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        manager.NetworkSessionManager.Networking.Connected += OnNetworkInitialised;
    }

    private void OnNetworkInitialised(ConnectedArgs args)
    {
        manager.NetworkSessionManager.Networking.PeerDataReceived += OnPeerDataReceived;
    }

    private void OnPeerDataReceived(PeerDataReceivedArgs args)
    {
        var stream = new MemoryStream(args.CopyData());
        switch (args.Tag)
        {
            case POSITION_EVT:
                Vector3 position = (Vector3)GlobalSerializer.Deserialize(stream);
                UpdatePlayerPosition(args.Peer.Identifier, position);
                SendUpdateToOthers(args.Peer, position);
                break;
            case UPDATE_EVT:
                System.Guid senderId;
                Vector3 pos;

                using (var deserializer = new BinaryDeserializer(stream))
                {
                    senderId = (System.Guid)deserializer.Deserialize();
                    pos = (Vector3)deserializer.Deserialize();
                }
                UpdatePlayerPosition(senderId, pos);
                break;
        }

    }

    private void UpdatePlayerPosition(Guid identifier, Vector3 position)
    {
        if (players.ContainsKey(identifier))
        {
            players[identifier].GetComponent<CarDriver>().SetDestination(position);
        }
        else
        {
            players.Add(identifier, CreateCar(position));
        }
    }

    private void SendUpdateToOthers(IPeer peer, Vector3 position)
    {
        var stream = new MemoryStream();
        using (var serializer = new BinarySerializer(stream))
        {
            serializer.Serialize(peer.Identifier);
            serializer.Serialize(position);
        }
        byte[] data = stream.ToArray();
        var toPeers = manager.NetworkSessionManager.Networking.OtherPeers.ToList();

        toPeers.Remove(manager.ARNetworking.Networking.Host);
        toPeers.Remove(peer);

        manager.NetworkSessionManager.Networking.SendDataToPeers(
            tag:UPDATE_EVT,
            data: data,
            peers: toPeers,
            transportType: TransportType.ReliableOrdered
        );

    }

    // Update is called once per frame
    void Update()
    {
        if (PlatformAgnosticInput.touchCount <= 0) return;
        var touch = PlatformAgnosticInput.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            OnTapScreen(touch);
        }

    }
    private void OnTapScreen(Touch touch)
    {
        var currentFrame = manager.ARSessionManager.ARSession.CurrentFrame;

        if (currentFrame == null) return;

        var hitTestResults = currentFrame.HitTest(
            camera.pixelWidth,
            camera.pixelHeight,
            touch.position,
            Niantic.ARDK.AR.HitTest.ARHitTestResultType.EstimatedHorizontalPlane
        );

        if (hitTestResults.Count <= 0) return;

        var position = hitTestResults[0].WorldTransform.ToPosition();

        if(me == null)
        {
            me = CreateCar(position);
        }
        else
        {
            me.GetComponent<CarDriver>().SetDestination(position);
        }

        if (!AMIHost())
        {
            SendToHost(position);
        }
    }

    public void SendToHost(Vector3 position)
    {
        var stream = new MemoryStream();
        GlobalSerializer.Serialize(stream, position);
        byte[] data = stream.ToArray();

        manager.NetworkSessionManager.Networking.SendDataToPeer(tag: POSITION_EVT,
            data: data,
            peer: manager.ARNetworking.Networking.Host,
            transportType: TransportType.ReliableOrdered
            );
  
    }

    private GameObject CreateCar(Vector3 posistion)
    {
        GameObject car = Instantiate(objectPrefab, this.transform);
        car.transform.position = posistion;
        return car;
    }

    bool AMIHost()
    {
        IMultipeerNetworking networking = manager.NetworkSessionManager.Networking;
        return (networking.Host.Identifier == networking.Self.Identifier);
    }


}
