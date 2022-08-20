using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public bool floorLocked;
    public bool tableLocked;
    public GameObject camera;

    public LayerMask mask;
    private Ray RayOrigin;
    private RaycastHit HitInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
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
                Vector3 envPos = gameObject.transform.position;
                envPos.y = placerPos.y;
                gameObject.transform.position = envPos;
            }
            
        }
        if (this.tableLocked == false)
        {
            Transform cameraTransform = camera.transform;

            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out HitInfo, 100.0f, mask))
            {
                gameObject.transform.transform.position = HitInfo.point;
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.red);

            }

        }

    }

    public void UnlockFloor()
    {
        this.floorLocked = false;
        RenderSettings.ambientIntensity = 1.0f;
    }

    public void LockFloor()
    {
        this.floorLocked = true;
        this.tableLocked = false;
    }

    public void LockTable()
    {
        this.tableLocked = true;

        RenderSettings.ambientIntensity = 0.0f;
    }
}
