using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCardManager : MonoBehaviour
{
    public GameObject StickyCard_Below;
    public bool highlighted;
    public bool FaceUp = true;
    private float thickness = 0.002f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StickyCard_Below)
        {
            Vector3 before = gameObject.transform.position;
            gameObject.transform.position = new Vector3(StickyCard_Below.gameObject.transform.position.x,
                                                        StickyCard_Below.gameObject.transform.position.y + thickness,
                                                         StickyCard_Below.gameObject.transform.position.z);
            Vector3 EulerRotation = StickyCard_Below.gameObject.transform.rotation.eulerAngles;

            if (FaceUp)
            {
                EulerRotation.z = 0;
            }
            else
            {
                EulerRotation.z = 180;
            }
            gameObject.transform.rotation = Quaternion.Euler(EulerRotation);

        }

    }
}
