using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    protected bool arReseting = false;
    public GameObject camera;
    public GameObject tableTop;

    public GameObject AvatarPrefab;
    public GameObject AvatarGroup;
    public GameObject Avatars;

    public GameObject Environment;

    public LayerMask mask;
    private Ray RayOrigin;
    private RaycastHit HitInfo;

    private Vector3 lockedCameraPos;
    private Vector3 lockedCameraRot;
    private Vector3 lockedCameraFwd;
    public Vector3 lockedTablePlacerPos = new Vector3(0.0f, 0.0f, 0.0f);


    public float envRotation = 0f;


    // Start is called before the first frame update


    public static EnvironmentManager Instance;
    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {

    }




    void setTablePlacerVisibility(bool visibility)
    {
        GameObject[] tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacer");
        foreach (GameObject tablePlacer in tablePlacers)
        {
            for (int c = 0; c < tablePlacer.transform.childCount; c++)
            {
                GameObject tablePlacerChild = tablePlacer.transform.GetChild(c).gameObject;
                tablePlacerChild.SetActive(visibility);
            }
        }
        tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacerPlane");
        foreach (GameObject tablePlacer in tablePlacers)
        {
            for (int c = 0; c < tablePlacer.transform.childCount; c++)
            {
                GameObject tablePlacerChild = tablePlacer.transform.GetChild(c).gameObject;
                tablePlacerChild.SetActive(visibility);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.setTablePlacerVisibility(this.arReseting);
        this.setEnvVisibility();
        if (this.arReseting)
        {
            this.lockTablePlacingVars();
        }
        else if (MultiplayerNetworkingManager.Instance.PeerDataExists())
        {
            
            this.OrientateTableAndMoveAvatarsPositions();

        }



    }

    private void lockTablePlacingVars()
    {
        this.lockedCameraFwd = camera.transform.forward;
        this.lockedCameraPos = camera.transform.position;
        this.lockedCameraRot = camera.transform.eulerAngles;
        GameObject[] tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacer");
        if (tablePlacers.Length != 0)
        {
            this.lockedTablePlacerPos = tablePlacers[0].transform.position;
        }
    }

    private void MovePlayerAvatars()
    {
        MultiplayerNetworkingManager peerManager = MultiplayerNetworkingManager.Instance;
        foreach (var _player in peerManager.players)
        {
            Vector3 pos = GetDealingSpotPositionForPlayer(peerManager.GetPeerPlayerIndex(_player.Key));
            Quaternion rot = GetDealingSpotRotationForPlayer(peerManager.GetPeerPlayerIndex(_player.Key));
            _player.Value.transform.SetPositionAndRotation(pos,
                                                            rot);
        }
    }

    private void setEnvVisibility()
    {
        if (this.arReseting)
        {

            // turn off ambient light
            RenderSettings.ambientIntensity = 1.0f;
            // show env
            Environment.transform.Find("Room").gameObject.SetActive(false);
            Environment.transform.Find("TableAndChairs").gameObject.SetActive(false);

        }
        else
        {
            // turn off ambient light
            RenderSettings.ambientIntensity = 0.0f;
            // show env
            Environment.transform.Find("Room").gameObject.SetActive(true);
            Environment.transform.Find("TableAndChairs").gameObject.SetActive(true);
        }
    }

    private void OrientateTableAndMoveAvatarsPositions()
    {
        Environment.transform.eulerAngles = new Vector3(Environment.transform.eulerAngles.x,
                                                        0,
                                                        Environment.transform.eulerAngles.z);

        //find table placer cursor object
        GameObject[] tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacer");

        //current table env space
        Vector3 envCenter_absolute = Environment.transform.position;
        Vector3 tableTopPos_absolute = this.GetTableCenter();
        Vector3 tableTopPos_relativeTo_envCenter = tableTopPos_absolute - envCenter_absolute;
        Vector3 dealPos_absolute = this.GetDealingSpotPositionForPlayer(MultiplayerNetworkingManager.Instance.GetMyPlayerIndex());
        Vector3 dealPos_relativeTo_envCenter = dealPos_absolute - envCenter_absolute;
        Vector3 dealPos_relativeTo_tableTopPos = dealPos_absolute - tableTopPos_absolute;

        envRotation = (float)this.AngleBetween(this.lockedCameraFwd, dealPos_relativeTo_tableTopPos);

        //set players 
        MultiplayerNetworkingManager peerManager = MultiplayerNetworkingManager.Instance;
        foreach (var _player in peerManager.players)
        {
            string player_id = _player.Key;
            GameObject player_obj = _player.Value;
            Vector3 pos = GetDealingSpotPositionForPlayer(peerManager.GetPeerPlayerIndex(player_id));
            Quaternion rot = GetDealingSpotRotationForPlayer(peerManager.GetPeerPlayerIndex(player_id));
            player_obj.transform.SetPositionAndRotation(pos,
                                                        rot);


        }
        //camera space

        Vector3 placerPos_relativeTo_camera = lockedTablePlacerPos - this.lockedCameraPos;

        Environment.transform.position = (lockedTablePlacerPos) - dealPos_relativeTo_envCenter;


        //new table env space
        Vector3 new_envCenter_absolute = Environment.transform.position;
        Vector3 new_tableTopPos_absolute = this.GetTableCenter();
        Vector3 new_tableTopPos_relativeTo_envCenter = new_tableTopPos_absolute - new_envCenter_absolute;
        Vector3 new_dealPos_absolute = this.GetDealingSpotPositionForPlayer(MultiplayerNetworkingManager.Instance.GetMyPlayerIndex());
        Vector3 new_dealPos_relativeTo_envCenter = new_dealPos_absolute - new_envCenter_absolute;
        Vector3 new_dealPos_relativeTo_tableTopPos = new_dealPos_absolute - new_tableTopPos_absolute;

        float a = (float)this.AngleBetween(this.lockedCameraFwd, -new_dealPos_relativeTo_tableTopPos);

        //Environment.transform.position = this.RotatePointAround(placerPos, this.envRotation, Environment.transform.position);

        Environment.transform.RotateAround(lockedTablePlacerPos, Vector3.up, a);
    }

    public double AngleBetween(Vector3 vector1, Vector3 vector2)
    {
        double sin = vector1.x * vector2.z - vector2.x * vector1.z;
        double cos = vector1.x * vector2.x + vector1.z * vector2.z;

        return Math.Atan2(sin, cos) * (180 / Math.PI);
    }
    public Vector3 GetDealingSpotPositionForPlayer(int playerNumber)
    {
        Vector3 tableCenter = this.GetTableCenter();
        float tableWidth = 0.5f;
        Vector3 dealingSpotOffest0 = new Vector3(tableWidth, 0, 0);
        
        int playerCount = MultiplayerNetworkingManager.Instance.GetCurrentPlayerCount();
        float angleRotation = 360.0f * ((float)(playerNumber + 1) / (float)(playerCount));
        Vector3 dealingSpot = tableCenter + dealingSpotOffest0;
        dealingSpot = this.RotatePointAround(tableCenter, (angleRotation), dealingSpot);

        return dealingSpot;
    }
    public Quaternion GetDealingSpotRotationForPlayer(int playerNumber)
    {
        Vector3 dealingSpot = this.GetDealingSpotPositionForPlayer(playerNumber);
        
        Vector3 tableCenter = this.GetTableCenter();
        Vector3 lookTowardsMiddle = tableCenter- dealingSpot;
        Quaternion rotation = Quaternion.FromToRotation(new Vector3(0.0f,0.0f,1.0f), lookTowardsMiddle);
    
        return rotation;
    }

    public Vector3 RotatePointAround(Vector3 c, float angle, Vector3 p)
    {
        double DegToRad = Math.PI / 180;

        double radians = angle * DegToRad;
        float sinA = (float)Math.Sin(radians);
        float cosA = (float)Math.Cos(radians);

        // translate point back to origin:
        p.x -= c.x;
        p.z -= c.z;

        // rotate point
        float xnew = p.x * cosA - p.z * sinA;
        float znew = p.x * sinA + p.z * cosA;

        // translate point back:
        p.x = xnew + c.x;
        p.z = znew + c.z;
        return p;
    }

    public Vector3 GetTableCenter()
    {
        return tableTop.transform.position;
    }

    public void ARReseting()
    {
        this.arReseting = true;

         
       
    }


    public void FinishARReset()
    {
        this.arReseting = false;
        RenderSettings.ambientIntensity = 0.0f;
        Environment.transform.Find("Room").gameObject.SetActive(true);
        Environment.transform.Find("TableAndChairs").gameObject.SetActive(true);
    }
}
