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

    
    // Start is called before the first frame update


    public static EnvironmentManager Instance;
    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        this.setTablePlacerVisibility(this.arReseting); 
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
        if (this.arReseting)
        {
            // turn on ambient light
            RenderSettings.ambientIntensity = 1.0f;
            // hide env
            Environment.transform.Find("Room").gameObject.SetActive(true);
            Environment.transform.Find("TableAndChairs").gameObject.SetActive(true);
            //find table placer cursor object
            GameObject[] tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacer");
            if (tablePlacers.Length != 0)
            {
                Transform placerTransform = tablePlacers[0].transform;
                Vector3 playerDealSpot = this.GetDealingSpotPositionForPlayer(MultiplayerNetworkingManager.Instance.GetMyPlayerIndex());
                Quaternion playerDealRotation = PlayerManager.Instance.GetDealingRotation();
                
                Environment.transform.SetPositionAndRotation(placerTransform.position+(Environment.transform.position-playerDealSpot),
                                                             playerDealRotation);
            }
        }
        else
        {
            // turn off ambient light
            RenderSettings.ambientIntensity = 0.0f;
            // show env
            Environment.transform.Find("Room").gameObject.SetActive(true);
            Environment.transform.Find("TableAndChairs").gameObject.SetActive(true);
        }
        //if (this.floorLocked == false)
        //{
        //    GameObject[] tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacer");
        //    if (tablePlacers.Length == 1)
        //    {
        //        Vector3 placerPos = tablePlacers[0].transform.position;
        //        Vector3 envPos = Environment.transform.position;
        //        envPos.y = placerPos.y;
        //        Environment.transform.position = envPos;
        //    }
            
        //}
        //if (this.tableLocked == false)
        //{
        //    lockedCameraPos = camera.transform.position;
        //    lockedCameraRot = camera.transform.eulerAngles;
        //    lockedCameraFwd = camera.transform.forward;

        //    if (Physics.Raycast(lockedCameraPos, lockedCameraFwd, out HitInfo, 100.0f, mask))
        //    {

        //        Vector3 centerGround = new Vector3(lockedCameraPos.x,
        //                                           HitInfo.point.y,
        //                                            lockedCameraPos.z);
        //        Vector3 hit_to_center = HitInfo.point - centerGround;


        //        Environment.transform.position = centerGround + (Vector3.Normalize(hit_to_center)*PlayerManager.Instance.GetDistanceToTable());
        //        //Environment.transform.eulerAngles = new Vector3(Environment.transform.eulerAngles.x,
        //        //                                                lockedCameraRot.y+PlayerManager.Instance.GetTableRotation(),
        //        //                                                Environment.transform.eulerAngles.z);

        //        //Debug.DrawRay(lockedCameraPos, lockedCameraFwd * 100.0f, Color.red);

        //    }

        //}
        //else
        //{
        //    Environment.transform.eulerAngles = new Vector3(Environment.transform.eulerAngles.x,
        //                                                    lockedCameraRot.y + PlayerManager.Instance.GetTableRotation(),
        //                                                    Environment.transform.eulerAngles.z);


        //}
        //Debug.DrawRay(lockedCameraPos, Environment.transform.position, Color.yellow);

        //Quaternion rot = new Quaternion();
        //rot.eulerAngles = new Vector3(0, 90, 0);
        //Vector3 b = (rot * (camera.transform.position - Environment.transform.position)) + Environment.transform.position;
        //Debug.DrawLine(Environment.transform.position, camera.transform.position, Color.yellow);
        //Debug.DrawLine(Environment.transform.position, b, Color.yellow);
        //Debug.DrawLine(camera.transform.position, b, Color.yellow);
        ////Debug.DrawRay(Environment.transform.position, b, Color.red);
        //Debug.LogFormat("lockedCameraPos={0}  Environment={1} b={2}", lockedCameraPos.ToString(), Environment.transform.position.ToString(), b.ToString());

    }

    public Vector3 GetDealingSpotPositionForPlayer(int playerNumber)
    {
        Vector3 tableCenter = this.GetTableCenter();
        float tableWidth = 0.5f;
        Vector3 dealingSpotOffest0 = new Vector3(tableWidth, 0, 0);
        int playerCount = MultiplayerNetworkingManager.Instance.GetCurrentPlayerCount();
        float angleRotation = 360.0f * ((float)(playerNumber + 1) / (float)(playerCount));
        Vector3 dealingSpot = tableCenter + dealingSpotOffest0;
        dealingSpot = this.RotatePointAround(tableCenter, angleRotation, dealingSpot);

        return dealingSpot;
    }
    public Quaternion GetDealingSpotRotationForPlayer(int playerNumber, Vector3 forward)
    {
        Vector3 dealingSpot = this.GetDealingSpotPositionForPlayer(playerNumber);
        
        Vector3 tableCenter = this.GetTableCenter();
        Vector3 lookTowardsMiddle = tableCenter- dealingSpot;
        Quaternion rotation = Quaternion.FromToRotation(new Vector3(0.0f,0.0f,1.0f), lookTowardsMiddle);
    
        return rotation;
    }

    protected Vector3 RotatePointAround(Vector3 c, float angle, Vector3 p)
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

    private Vector3 GetTableCenter()
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
