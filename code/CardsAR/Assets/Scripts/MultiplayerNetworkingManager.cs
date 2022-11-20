using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



public class MultiplayerNetworkingManager : MonoBehaviour
{
    public static MultiplayerNetworkingManager Instance;
    public int playerCount = 6;
    public int playerIndex = 1;
    public GameObject AvatarPrefab;
    public GameObject AvatarPrefabParent;
    public Dictionary<String, GameObject> players = new Dictionary<String, GameObject>();


    private void Awake()
    {
        Instance = this;


    }
   


    internal bool AmIHost()
    {
        bool ret = this.GetMyPlayerIndex() == 0;
       
        return ret;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //this.DebugMoveAllAvatars();
    }

    internal bool PeerDataExists()
    {
        return this.GetCurrentPlayerCount() != 0;
    }

    //private void DebugMoveAllAvatars()
    //{
    //    Vector3 myDealPos = EnvironmentManager.Instance.lockedTablePlacerPos;
    //    Vector3 tableCenter = EnvironmentManager.Instance.GetTableCenter();
    //    Vector3 cameraReativeToDealPos = camera.transform.position - myDealPos;
    //    Vector3 dealPosReativeTableCenter = myDealPos - tableCenter;
    //    float a = (float)EnvironmentManager.Instance.AngleBetween(new Vector3(0,0,-1), dealPosReativeTableCenter);
    //    Debug.DrawLine(myDealPos, camera.transform.position, Color.
    //                        red);
    //    Vector3 peerPosChange = EnvironmentManager.Instance.RotatePointAround(Vector3.zero,
    //                                                                          -a,
    //                                                                          cameraReativeToDealPos);
    //    Vector3 cameraLook = camera.transform.eulerAngles;
    //    cameraLook.y += a;
    //    foreach (var playerAvatar in players)
    //    {
    //        Quaternion playerRot = EnvironmentManager.Instance.GetDealingSpotRotationForPlayer(this.GetPeerPlayerIndex(playerAvatar.Key));
    //        GameObject playerAvatarScreen = playerAvatar.Value.transform.GetChild(0).gameObject;
    //        playerAvatarScreen.transform.localPosition = peerPosChange;

    //        playerAvatarScreen.transform.localEulerAngles = cameraLook;
    //        Debug.DrawLine(playerAvatar.Value.transform.GetChild(0).transform.position,
    //                       playerAvatar.Value.transform.GetChild(1).transform.position, Color.green);


    //    }
    //}

    //public void AddRandomPeer()
    //{
    //    //System.Guid("00000000-0000-0000-0000-000000000020")
    //    System.Random r = new System.Random();
    //    int rInt = r.Next(0, 10000);
    //    string guid_string = "00000000-0000-0000-0000-" + rInt.ToString("D12");
    //    Guid peerID = new System.Guid(guid_string);
    //    this.OnPeerPoseReceived(peerID, new Vector3(0f, 0f, 0f), new Quaternion());

    //}

    public void OnPeerPoseReceived(String playerIdentifier, Vector3 pos, Vector3 rot, string name)
    {
        //Debug.LogFormat("CARDSAR: Peer {0} at position: {1} with rotation {2}", playerIdentifier, pos, rot);

        // ...and if the dictionary already contains the player...
        if (players.ContainsKey(playerIdentifier) == false)
        {
            // ...then create an avatar for the remote player...
            GameObject remoteAvatar = CreateAvatar();
            // ...then set the player's avatar postition and rotation.
            GameObject playerAvatarScreen = remoteAvatar.transform.GetChild(0).gameObject;
            playerAvatarScreen.transform.localPosition = pos;
            playerAvatarScreen.transform.localEulerAngles = rot;
            // ...and add it to the dictionary.
            players.Add(playerIdentifier, remoteAvatar);

            TextMeshPro namePlate = playerAvatarScreen.transform.GetChild(2).gameObject.GetComponent<TextMeshPro>();
            namePlate.text = name;
        }
        else
        {
            // ...then create an avatar for the remote player...
            GameObject remoteAvatar = players[playerIdentifier];
            // ...then set the player's avatar postition and rotation.
            GameObject playerAvatarScreen = remoteAvatar.transform.GetChild(0).gameObject;
            playerAvatarScreen.transform.localPosition = pos;
            playerAvatarScreen.transform.localEulerAngles = rot;

            Debug.DrawLine(remoteAvatar.transform.GetChild(1).position,
                          remoteAvatar.transform.GetChild(1).position+pos, Color.yellow);
            TextMeshPro namePlate = playerAvatarScreen.transform.GetChild(2).gameObject.GetComponent<TextMeshPro>();

            namePlate.text = name;

        }

        if (playerIdentifier == PlayerManager.Instance.uid)
        {
            GameObject remoteAvatar = players[playerIdentifier];
            // ...then set the player's avatar postition and rotation.
            GameObject playerAvatarScreen = remoteAvatar.transform.GetChild(0).gameObject;
            playerAvatarScreen.SetActive(false);
        }
        else
        {
            GameObject remoteAvatar = players[playerIdentifier];
            // ...then set the player's avatar postition and rotation.
            GameObject playerAvatarScreen = remoteAvatar.transform.GetChild(0).gameObject;
            playerAvatarScreen.SetActive(true);
        }





    }



    public int GetMyPlayerIndex()
    {
        return GetPeerPlayerIndex(PlayerManager.Instance.uid);
    }

    public int GetPeerPlayerIndex(String peer)
    {
        List<String> peer_list = new List<String>();
        foreach (String ip in this.players.Keys)
        {
            peer_list.Add(ip);
        }
        peer_list.Sort();
        int ret = peer_list.IndexOf(peer);
       
        return ret;
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
