using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCardManager : MonoBehaviour
{
    public GameObject StickyCard_Below;
    public bool selected = false;
    public bool stickySelected = false;
    public bool highlighted = false;
    public bool stickyHighlighted = false;
    public bool hiddenToMe = false;
    public bool inHiddenZone = false;
    public bool FaceUp = true;
    private float thickness = 0.002f;
    private float nonSelectedHeight = 0.01f;
    private float selectedHeight = 0.03f;
    public Color outlineColor = Color.cyan;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.UpdateTransform();
        this.UpdateSelection();
        this.UpdateHighlighted();
        this.UpdateOutline();
        //this.UpdateRenderQueue();


    }

    private void UpdateRenderQueue()
    {
        Transform cardTransform = gameObject.transform.GetChild(0);
        Material cardMaterial = cardTransform.GetComponent<Renderer>().material;
        if (this.selected || this.stickySelected)
        {
            cardMaterial.renderQueue = 2449;

        }
        else if (this.highlighted || this.stickyHighlighted)
        {
            cardMaterial.renderQueue = 2449;

        }
        else
        {
            cardMaterial.renderQueue = 2451;

        }
    }

    private void UpdateOutline()
    {
        Transform highlightTransform = gameObject.transform.Find("CardHighlight");
        Material highlightMaterial = highlightTransform.GetComponent<Renderer>().material;
        highlightMaterial.SetColor("_OutlineColor", outlineColor);
        if (this.selected || this.stickySelected)
        {
            highlightMaterial.SetFloat("_OutlineWidth", 0.006f);
            highlightMaterial.SetFloat("_OutlineFactor", 0.001f);

        }
        else if (this.highlighted || this.stickyHighlighted)
        {
            highlightMaterial.SetFloat("_OutlineWidth", 0.002f);
            highlightMaterial.SetFloat("_OutlineFactor", 0.001f);

        }
        else
        {
            highlightMaterial.SetFloat("_OutlineWidth", 0.0f);
            highlightMaterial.SetFloat("_OutlineFactor", 0.0f);

        }

    }

    private void UpdateHighlighted()
    {
        this.stickyHighlighted = this.IsStickyHightlighted();
    }

    private bool IsStickyHightlighted()
    {
        bool highlightedBelow = ((StickyCard_Below) && StickyCard_Below.GetComponent<StickyCardManager>().IsStickyHightlighted());
        if (highlightedBelow)
        {
            this.highlighted = false;
            return true;
        }
        else
        {
            return this.highlighted;
        }
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
            return this.selected;
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
                                                    gameObject.transform.parent.gameObject.transform.position.y + selectedHeight,
                                                    gameObject.transform.position.z);
    }

    private void UpdateSingleCardPosition()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                    gameObject.transform.parent.gameObject.transform.position.y + nonSelectedHeight, 
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
