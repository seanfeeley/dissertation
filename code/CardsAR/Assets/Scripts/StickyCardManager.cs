using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCardManager : MonoBehaviour
{



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

    public GameObject _below; // field
    public GameObject StickyCard_Below   // property
    {
        get { return _below; }   // get method
        set {
            if (_below)
            {
                _below.GetComponent<StickyCardManager>().StickyCard_Above = null;
            }
            if (value != null)
            {
                value.GetComponent<StickyCardManager>().StickyCard_Above = gameObject;
            }
            _below = value;
        }  // set method
    }

    public GameObject _above; // field
    public GameObject StickyCard_Above   // property
    {
        get { return _above; }   // get method
        set { _above = value; }  // set method
    }



    // Start is called before the first frame update
    void Start()
    {
        if (_below != null)
        {
            _below.GetComponent<StickyCardManager>().StickyCard_Above = gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.UpdateTransform();
        //this.UpdateSelection();
        //this.UpdateHighlighted();
        this.UpdateOutline();
        this.UpdateHidden();
        //this.UpdateRenderQueue();
        this.UpdateCardFace();

    }

    public GameObject GetBottomCard()
    {
        if (_below)
        {
            return _below.GetComponent<StickyCardManager>().GetBottomCard();
        }
        else
        {
            return gameObject;
        }
    }


    public int CountAbove()
    {
        if (_above)
        {
            return 1 + _above.GetComponent<StickyCardManager>().CountAbove();
        }
        else
        {
            return 1;
        }
    }

    public GameObject GetTopCard()
    {
        if (_above)
        {
            return _above.GetComponent<StickyCardManager>().GetTopCard();
        }
        else
        {
            return gameObject;
        }
    }

    private void UpdateHidden()
    {
        gameObject.transform.Find("Red_PlayingCards_Hidden_00").gameObject.SetActive(this.hiddenToMe); 
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
        GameObject card = gameObject.transform.GetChild(0).gameObject;
        GameObject hiddenCard = gameObject.transform.GetChild(1).gameObject;
        Material highlightMaterial = highlightTransform.GetComponent<Renderer>().material;
        highlightMaterial.SetColor("_Color", outlineColor);
        highlightMaterial.SetColor("_EmissionColor", outlineColor);
        if (this.selected || this.stickySelected)
        {
            highlightTransform.localScale = new Vector3(1.25f, 1.0f, 1.25f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            card.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else if (this.highlighted || this.stickyHighlighted)
        {
            highlightTransform.localScale = new Vector3(1.1f, 1.0f, 1.1f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            card.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else
        {
            highlightTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            gameObject.layer = LayerMask.NameToLayer("StaticCards");
            card.layer = LayerMask.NameToLayer("StaticCards");
            hiddenCard.layer = LayerMask.NameToLayer("StaticCards");
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

    internal void Flip()
    {
        this.FaceUp = !this.FaceUp;
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

    }
    private void UpdateCardFace() {
        Vector3 CardScale = Vector3.one;
        if (!FaceUp)
        {
            CardScale.y = -1;
        }
        GameObject mainCard = gameObject.transform.GetChild(0).gameObject;
        mainCard.transform.localScale = CardScale;

    }
}
