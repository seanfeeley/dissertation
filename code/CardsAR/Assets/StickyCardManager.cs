using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCardManager : MonoBehaviour
{
    public GameObject StickyCard_Below;
    public bool highlighted;
    public bool selected = false;
    public bool stickySelected = false;
    public bool hiddenToMe = false;
    public bool inHiddenZone = false;
    public bool FaceUp = true;
    private float thickness = 0.002f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.UpdateTransform();
        this.UpdateSelection();
        

    }

    private void UpdateSelection()
    {
        this.stickySelected = this.IsStickySelected();
        if (this.selected)
        {
            this.StickyCard_Below = null;
        }
    }

    public bool IsStickySelected()
    {
        bool selectedBelow = ((StickyCard_Below) && StickyCard_Below.GetComponent<StickyCardManager>().IsStickySelected());
        if (selectedBelow)
        {
            this.selected = false;
            return true;
        }
        else
        {
            return selected;
        }
    }

    public void UpdateTransform()
    {
        
        if (StickyCard_Below)
        {
            StickyCard_Below.GetComponent<StickyCardManager>().UpdateTransform(); 
            this.UpdateStickyPosition();

        }
        else if (selected)
        {
            this.UpdateSelectedCardPosition();

        }
        else
        {
            this.UpdateSingleCardPosition();
           

        }

    }

    private void UpdateSelectedCardPosition()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                    gameObject.transform.parent.gameObject.transform.position.y +.02f,
                                                    gameObject.transform.position.z);
    }

    private void UpdateSingleCardPosition()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                    gameObject.transform.parent.gameObject.transform.position.y, 
                                                    gameObject.transform.position.z);
    }

    private void UpdateStickyPosition()
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
