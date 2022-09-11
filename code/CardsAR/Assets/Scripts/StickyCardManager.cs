using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCardManager : MonoBehaviour
{



    public bool fan = false;
    public bool selected = false;
    public bool stickySelected = false;
    public bool highlighted = false;
    public bool stickyHighlighted = false;
    public bool hiddenToMe = false;
    public bool inHiddenZone = false;
    public bool FaceUp = true;
    private float thickness = 0.002f;
    private float nonSelectedHeight = 0.001f;
    private float selectedHeight = 0.03f;
    public Color outlineColor = Color.cyan;


    public GameObject[] cardColliders;
    public GameObject hiddenCard;
    public GameObject playingCard;
    public GameObject highlight;


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


    private StickyCardManager GetStickyCardManager(GameObject card)
    {
        return card.GetComponent<StickyCardManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_below != null)
        {
            GetStickyCardManager(_below).StickyCard_Above = gameObject;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.UpdateTransform();
        //this.UpdateSelection();
        //this.UpdateStickyHighlighted();
        this.UpdateHighlightColliders();
        this.UpdateOutline();
        this.UpdateHidden();
        //this.UpdateRenderQueue();
        this.UpdateCardFace();

    }

    private void UpdateHighlightColliders()
    {
        bool underCursor = false;
        foreach (GameObject cardCollider in cardColliders)
        {
            underCursor = underCursor || cardCollider.transform.GetComponent<CursorTracker>().UnderCursor;
        }
        if (underCursor)
        {
            PlayerManager.Instance.AddToHighlightCandidates(gameObject);
        }
        else
        {
            PlayerManager.Instance.RemoveFromToHighlightCandidates(gameObject);

        }
    }

    public GameObject GetBottomCard()
    {
        if (_below)
        {
            return GetStickyCardManager(_below).GetBottomCard();
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
            return 1 + GetStickyCardManager(_above).CountAbove();
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
            return GetStickyCardManager(_above).GetTopCard();
        }
        else
        {
            return gameObject;
        }
    }

    private void UpdateHidden()
    {
        hiddenCard.SetActive(this.hiddenToMe); 
    }


    private void UpdateOutline()
    {
        Material highlightMaterial = highlight.GetComponent<Renderer>().material;
        highlightMaterial.SetColor("_Color", outlineColor);
        highlightMaterial.SetColor("_EmissionColor", outlineColor);
        if (this.selected || this.stickySelected)
        {
            highlight.transform.localScale = new Vector3(1.25f, 1.0f, 1.25f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            playingCard.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else if (this.highlighted || this.stickyHighlighted)
        {
            highlight.transform.localScale = new Vector3(1.1f, 1.0f, 1.1f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            playingCard.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else
        {
            highlight.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            gameObject.layer = LayerMask.NameToLayer("StaticCards");
            playingCard.layer = LayerMask.NameToLayer("StaticCards");
            hiddenCard.layer = LayerMask.NameToLayer("StaticCards");
        }

    }

    private void UpdateStickyHighlighted()
    {
        this.stickyHighlighted = this.IsStickyHightlighted();
    }

    private bool IsStickyHightlighted()
    {
        bool highlightedBelow = ((StickyCard_Below) && GetStickyCardManager(StickyCard_Below).IsStickyHightlighted());
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
        bool selectedBelow = ((StickyCard_Below) && GetStickyCardManager(StickyCard_Below).IsStickySelected());
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
            GetStickyCardManager(StickyCard_Below).UpdateTransform(); 
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
        gameObject.transform.position = new Vector3(StickyCard_Below.transform.position.x,
                                                    StickyCard_Below.transform.position.y + thickness,
                                                    StickyCard_Below.transform.position.z);

    }
    private void UpdateCardFace() {
        Vector3 CardScale = Vector3.one;
        if (!FaceUp)
        {
            CardScale.y = -1;
        }
        playingCard.transform.localScale = CardScale;

    }
}
