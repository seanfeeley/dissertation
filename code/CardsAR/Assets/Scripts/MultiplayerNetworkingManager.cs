using System;
using System.Collections;
using System.Collections.Generic;
using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using UnityEngine;

public class MultiplayerNetworkingManager : MonoBehaviour
{
    public static MultiplayerNetworkingManager Instance;
    public int playerCount = 6;
    public int playerIndex = 1;
    private IARNetworking _arNetworking;
    private IARSession _session;
    private IMultipeerNetworking _networking;
    public GameObject AvatarPrefab;
    private Dictionary<IPeer, GameObject> _avatarDict = new Dictionary<IPeer, GameObject>();
    private IPeer self;

    private void Awake()
    {
        Instance = this;
        ARNetworkingFactory.ARNetworkingInitialized += OnInitialized;


    }
    private void OnDestroy()
    {
        ARNetworkingFactory.ARNetworkingInitialized -= OnInitialized;
    }

    private void OnInitialized(AnyARNetworkingInitializedArgs args)
    {
        _arNetworking = args.ARNetworking;
        _session = _arNetworking.ARSession;
        _networking = _arNetworking.Networking;

        _session.Ran += OnSessionRan;
        _networking.Connected += OnNetworkConnected;
        _arNetworking.PeerStateReceived += OnPeerStateReceived;
        _arNetworking.PeerPoseReceived += OnPeerPoseReceived;





    }

    private void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        Debug.LogFormat("CARDSAR: Peer {0} joined us with state: {1}", args.Peer.Identifier, args.State);
    }

    private void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        Debug.LogFormat("CARDSAR: Peer {0} at position: {1}", args.Peer.Identifier, args.Pose.GetPosition());
        if (!_avatarDict.ContainsKey(args.Peer))
        {
            _avatarDict.Add(args.Peer, Instantiate(AvatarPrefab));
        }
        GameObject playerAvatar;
        if(_avatarDict.TryGetValue(args.Peer, out playerAvatar))
        {
            GameObject screen = playerAvatar.transform.GetChild(0).gameObject;
            screen.transform.localPosition = args.Pose.GetPosition() + new Vector3(0, 0, 0);
            screen.transform.localEulerAngles = args.Pose.rotation.eulerAngles;
        }
    }

    private void OnNetworkConnected(ConnectedArgs args)
    {
        Debug.Log("CARDSAR: Network Connected");
        this.self = args.Self;
    }

    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("CARDSAR: AR Session Ran");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetMyPlayerIndex()
    {
        return GetPeerPlayerIndex(self);
    }
    public int GetPeerPlayerIndex(IPeer peer)
    {
        List<Guid> peer_list = new List<Guid>();
        foreach (IPeer ip in this._avatarDict.Keys)
        {
            peer_list.Add(ip.Identifier);
        }
        peer_list.Sort();
        return peer_list.IndexOf(peer.Identifier);
    }

    public int GetCurrentPlayerCount()
    {
        return this._avatarDict.Count;
    }
}
