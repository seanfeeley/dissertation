using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{

    public GameObject cursor;
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

    // See Order of Execution for Event Functions for information on FixedUpdate() and Update() related to physics queries
    void FixedUpdate()
    {

        Transform cameraTransform = camera.transform;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out HitInfo, 100.0f, mask))
        {
            cursor.transform.position = HitInfo.point;
            cursor.transform.eulerAngles = new Vector3(cursor.transform.eulerAngles.x,
                                                        cameraTransform.eulerAngles.y,
                                                        cursor.transform.eulerAngles.z);
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.red);
            //Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 100.0f, Color.red);
        }


    }
}
