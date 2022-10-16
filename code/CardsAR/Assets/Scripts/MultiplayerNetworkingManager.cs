using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerNetworkingManager : MonoBehaviour
{
    public static MultiplayerNetworkingManager Instance;
    public int playerCount = 6;
    public int playerIndex = 1;
    public GameObject AvatarPrefab;
    public GameObject AvatarPrefabParent;
    private Dictionary<System.Guid, GameObject> _players = new Dictionary<System.Guid, GameObject>();
    private Guid myGuid;

    private Guid[] peerGuids = { new System.Guid("00000000-0000-0000-0000-000000000002"),
                                 new System.Guid("00000000-0000-0000-0000-000000000003"),
                                 new System.Guid("00000000-0000-0000-0000-000000000004"),
                                 new System.Guid("00000000-0000-0000-0000-000000000005"),
                                 new System.Guid("00000000-0000-0000-0000-000000000006"),
    };




    private void Awake()
    {
        Instance = this;


    }
    private void OnDestroy()
    {
    }

   

    // Start is called before the first frame update
    void Start()
    {
        myGuid = new System.Guid("00000000-0000-0000-0000-000000000001");
        this.OnPeerPoseReceived(myGuid, new Vector3(0f,0f,0f), new Quaternion());
        foreach(Guid peerID in this.peerGuids)
        {
            this.OnPeerPoseReceived(peerID, new Vector3(0f, 0f, 0f), new Quaternion());

        }

        
    }

    private void ResetAvatarPositionsAroundTable()
    {
      
        foreach (var _player in _players)
        {
            _player.Value.transform.SetPositionAndRotation(EnvironmentManager.Instance.GetDealingSpotPositionForPlayer(this.GetPeerPlayerIndex(_player.Key)),
                                                            EnvironmentManager.Instance.GetDealingSpotRotationForPlayer(this.GetPeerPlayerIndex(_player.Key),
                                                                                                                        _player.Value.transform.forward));
        }

    }

    // Update is called once per frame
    void Update()
    {
        this.ResetAvatarPositionsAroundTable();
    }

    private void OnPeerPoseReceived(System.Guid playerIdentifier, Vector3 position, Quaternion rotation)
    {
        Debug.LogFormat("CARDSAR: Peer {0} at position: {1} with rotation {2}", playerIdentifier, position, rotation);


        // ...and if the dictionary already contains the player...
        if (_players.ContainsKey(playerIdentifier) == false)
        {
            // ...then create an avatar for the remote player...
            GameObject remoteAvatar = CreateAvatar();
            // ...and add it to the dictionary.
            _players.Add(playerIdentifier, remoteAvatar);
        }
        
        
        // ...then set the player's avatar postition and rotation.
        //_players[playerIdentifier].transform.SetPositionAndRotation(position, rotation);

    }



    public int GetMyPlayerIndex()
    {
        return GetPeerPlayerIndex(myGuid);
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

    // Creates a car at a given position.
    private GameObject CreateAvatar()
    {
        GameObject createdAvatar = Instantiate(AvatarPrefab, AvatarPrefabParent.transform);
        
        return createdAvatar;
    }
}
