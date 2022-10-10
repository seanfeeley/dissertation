using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public bool floorLocked;
    public bool tableLocked;
    public GameObject camera;
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
        this.setTablePlacerVisibility(! this.floorLocked); 
    }

    void setTablePlacerVisibility(bool visibility)
    {
        GameObject[] tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacer");
        if (tablePlacers.Length == 1)
        {
            tablePlacers[0].transform.GetChild(0).gameObject.SetActive(visibility);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.floorLocked == false)
        {
            GameObject[] tablePlacers = GameObject.FindGameObjectsWithTag("TablePlacer");
            if (tablePlacers.Length == 1)
            {
                Vector3 placerPos = tablePlacers[0].transform.position;
                Vector3 envPos = Environment.transform.position;
                envPos.y = placerPos.y;
                Environment.transform.position = envPos;
            }
            
        }
        if (this.tableLocked == false)
        {
            lockedCameraPos = camera.transform.position;
            lockedCameraRot = camera.transform.eulerAngles;
            lockedCameraFwd = camera.transform.forward;

            if (Physics.Raycast(lockedCameraPos, lockedCameraFwd, out HitInfo, 100.0f, mask))
            {

                Vector3 centerGround = new Vector3(lockedCameraPos.x,
                                                   HitInfo.point.y,
                                                    lockedCameraPos.z);
                Vector3 hit_to_center = HitInfo.point - centerGround;


                Environment.transform.position = centerGround + (Vector3.Normalize(hit_to_center)*PlayerManager.Instance.GetDistanceToTable());
                Environment.transform.eulerAngles = new Vector3(Environment.transform.eulerAngles.x,
                                                                lockedCameraRot.y+PlayerManager.Instance.GetTableRotation(),
                                                                Environment.transform.eulerAngles.z);

                //Debug.DrawRay(lockedCameraPos, lockedCameraFwd * 100.0f, Color.red);

            }

        }
        else
        {
            Environment.transform.eulerAngles = new Vector3(Environment.transform.eulerAngles.x,
                                                            lockedCameraRot.y + PlayerManager.Instance.GetTableRotation(),
                                                            Environment.transform.eulerAngles.z);


        }
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

    public void UnlockFloor()
    {
        this.floorLocked = false;
        RenderSettings.ambientIntensity = 1.0f;
        Environment.transform.Find("Room").gameObject.SetActive(false);
        Environment.transform.Find("TableAndChairs").gameObject.SetActive(false);
         
       
    }

    public void LockFloor()
    {
        this.floorLocked = true;
        this.tableLocked = false;
        RenderSettings.ambientIntensity = 1.0f;
        Environment.transform.Find("Room").gameObject.SetActive(false);
        Environment.transform.Find("TableAndChairs").gameObject.SetActive(true);


    }

    public void LockTable()
    {
        this.tableLocked = true;

        RenderSettings.ambientIntensity = 0.0f;
        Environment.transform.Find("Room").gameObject.SetActive(true);
        Environment.transform.Find("TableAndChairs").gameObject.SetActive(true);
    }
}
