using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerNetworkingManager : MonoBehaviour
{
    public static MultiplayerNetworkingManager Instance;
    public int playerCount = 6;
    public int playerIndex = 1;
    public Camera camera;
    public GameObject AvatarPrefab;
    public GameObject AvatarPrefabParent;
    public Dictionary<System.Guid, GameObject> players = new Dictionary<System.Guid, GameObject>();
    private Guid myGuid;

    private Guid[] peerGuids = {
                                 new System.Guid("00000000-0000-0000-0000-000000000020"),
                                 new System.Guid("00000000-0000-0000-0000-000000000030"),
                                 //new System.Guid("00000000-0000-0000-0000-000000000040"),
                                 //new System.Guid("00000000-0000-0000-0000-000000000050"),
                                 //new System.Guid("00000000-0000-0000-0000-000000000060"),
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
        myGuid = new System.Guid("00000000-0000-0000-0000-000000000011");
        this.OnPeerPoseReceived(myGuid, new Vector3(0f,0f,0f), new Quaternion());
        foreach (Guid peerID in this.peerGuids)
        {
            this.OnPeerPoseReceived(peerID, new Vector3(0f, 0f, 0f), new Quaternion());

        }

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        this.DebugMoveAllAvatars();
    }

    private void DebugMoveAllAvatars()
    {
        Vector3 myDealPos = EnvironmentManager.Instance.lockedTablePlacerPos;
        Vector3 tableCenter = EnvironmentManager.Instance.GetTableCenter();
        Vector3 cameraReativeToDealPos = camera.transform.position - myDealPos;
        Vector3 dealPosReativeTableCenter = myDealPos - tableCenter;
        float a = (float)EnvironmentManager.Instance.AngleBetween(new Vector3(0,0,-1), dealPosReativeTableCenter);
        Debug.DrawLine(myDealPos, camera.transform.position, Color.
                            red);
        Vector3 peerPosChange = EnvironmentManager.Instance.RotatePointAround(Vector3.zero,
                                                                              -a,
                                                                              cameraReativeToDealPos);
        Vector3 cameraLook = camera.transform.eulerAngles;
        cameraLook.y += a;
        foreach (var playerAvatar in players)
        {
            Quaternion playerRot = EnvironmentManager.Instance.GetDealingSpotRotationForPlayer(this.GetPeerPlayerIndex(playerAvatar.Key));
            GameObject playerAvatarScreen = playerAvatar.Value.transform.GetChild(0).gameObject;
            playerAvatarScreen.transform.localPosition = peerPosChange;
            
            playerAvatarScreen.transform.localEulerAngles = cameraLook;
            Debug.DrawLine(playerAvatar.Value.transform.GetChild(0).transform.position,
                           playerAvatar.Value.transform.GetChild(1).transform.position, Color.green);


        }
    }

    private void OnPeerPoseReceived(System.Guid playerIdentifier, Vector3 position, Quaternion rotation)
    {
        Debug.LogFormat("CARDSAR: Peer {0} at position: {1} with rotation {2}", playerIdentifier, position, rotation);


        // ...and if the dictionary already contains the player...
        if (players.ContainsKey(playerIdentifier) == false)
        {
            // ...then create an avatar for the remote player...
            GameObject remoteAvatar = CreateAvatar();
            // ...then set the player's avatar postition and rotation.
            remoteAvatar.transform.SetPositionAndRotation(position, rotation);
            // ...and add it to the dictionary.
            players.Add(playerIdentifier, remoteAvatar);
        }
        
        

    }



    public int GetMyPlayerIndex()
    {
        return GetPeerPlayerIndex(myGuid);
    }
    public int GetPeerPlayerIndex(System.Guid peer)
    {
        List<Guid> peer_list = new List<Guid>();
        foreach (System.Guid ip in this.players.Keys)
        {
            peer_list.Add(ip);
        }
        peer_list.Sort();
        return peer_list.IndexOf(peer);
    }

    public int GetCurrentPlayerCount()
    {
        return this.players.Count;
    }

    // Creates a car at a given position.
    private GameObject CreateAvatar()
    {
        GameObject createdAvatar = Instantiate(AvatarPrefab, AvatarPrefabParent.transform);
        
        return createdAvatar;
    }
}
