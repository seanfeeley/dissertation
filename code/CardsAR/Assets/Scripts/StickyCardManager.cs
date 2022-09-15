using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyCardManager : MonoBehaviour
{


    public GameObject cursor;
    public bool selected = false;
    public bool stickySelected = false;
    public bool highlighted = false;
    public bool stickyHighlighted = false;
    public bool hiddenToMe = false;
    public bool inHiddenZone = false;
    public bool FaceUp = true;
    private float thickness = 0.002f;
    private float spreadWidth = 0.03f;
    private float nonSelectedHeight = 0.001f;
    private float selectedHeight = 0.03f;
    public Color outlineColor = Color.cyan;

    public bool _spread; // field
    public bool Spread   // property
    {
        get
        {
            if (_below)
            {
                return GetStickyCardManager(_below).Spread;
            }
            else if(_above){
                return _spread;

            }
            return false;
        }   // get method
        set { _spread = value; }  // set method
    }

    public GameObject[] cardColliders;
    public GameObject hiddenCard;
    public GameObject playingCard;
    public GameObject highlight;

    public GameObject _above; // field


    public GameObject _below; // field
    public GameObject StickyCard_Below   // property
    {
        get { return _below; }   // get method
        set {
            GameObject _old_below = _below;
            GameObject _new_below = value;
            if (_old_below)
            {
                GetStickyCardManager(_old_below)._above = null;

            }
            if (_new_below)
            {
                GameObject previously_above_new_below = GetStickyCardManager(_new_below)._above;
                if (previously_above_new_below)
                {

                    // we are inserting into the middle of a deck of cards
                    // so change the card above

                    GetStickyCardManager(previously_above_new_below)._below = GetTopCard();
                    _above = previously_above_new_below;
                }

            }
            // and change the card below
            _below = _new_below;
            if (_below)
            {
                GetStickyCardManager(_below)._above = gameObject;
            }
        }  // set method
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
            GetStickyCardManager(_below)._above = gameObject;
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

    internal void DropOnTopOf(GameObject anotherCard)
    {
        StickyCard_Below = anotherCard;
        selected = false;
        cursor = null;
    }

    internal void DropOntoTable()
    {
        selected = false;
        cursor = null;
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

    internal void SinglePickUp(GameObject playerCursor)
    {
        cursor = playerCursor;
        selected = true;
        if (_below)
        {
            GetStickyCardManager(_below)._above = null;
        }
        if (_above)
        {
            GetStickyCardManager(_above).Spread = Spread; 
            GetStickyCardManager(_above)._below = null;
        }
        _below = null;
        _above = null;
    }

    public void UpdateTransform()
    {
        
        if (StickyCard_Below)
        {
            GetStickyCardManager(StickyCard_Below).UpdateTransform();
            if (this.Spread)
            {
                this.UpdateSpreadPosition();
            }
            else
            {
                this.UpdateStickyPosition();
            }

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

    internal void DeckPickUp(GameObject playerCursor)
    {
        cursor = playerCursor;
        selected = true;
        if (_below)
        {
            GetStickyCardManager(_below)._above = null;
        }
        _below = null;
        _spread = false;
    }

    private void UpdateSelectedCardPosition()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                    gameObject.transform.parent.gameObject.transform.position.y + selectedHeight,
                                                    gameObject.transform.position.z);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,
                                                       cursor.transform.eulerAngles.y,
                                                       gameObject.transform.eulerAngles.z);

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


        gameObject.transform.eulerAngles = new Vector3(StickyCard_Below.transform.eulerAngles.x,
                                                       StickyCard_Below.transform.eulerAngles.y,
                                                       0.0f);


    }

    internal void MagnetSingle(GameObject tableCard)
    {
        GetStickyCardManager(tableCard).SinglePickUp(cursor);
        selected = false;
        StickyCard_Below = tableCard;
    }

    private void UpdateSpreadPosition()
    {
        
        Vector3 before = gameObject.transform.position;
        float x_shift = Mathf.Cos(Mathf.Deg2Rad *gameObject.transform.eulerAngles.y) * spreadWidth;
        float z_shift = Mathf.Sin(Mathf.Deg2Rad * gameObject.transform.eulerAngles.y) * spreadWidth;
        gameObject.transform.position = new Vector3(StickyCard_Below.transform.position.x + x_shift,
                                                    StickyCard_Below.transform.position.y,
                                                    StickyCard_Below.transform.position.z - z_shift);
        gameObject.transform.eulerAngles = new Vector3(StickyCard_Below.transform.eulerAngles.x,
                                                       StickyCard_Below.transform.eulerAngles.y,
                                                       -2.0f);


    }

    internal void MagnetDeck(GameObject tableCard)
    {
        GetStickyCardManager(tableCard).DeckPickUp(cursor);
        selected = false;
        StickyCard_Below = GetStickyCardManager(tableCard).GetTopCard();
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
