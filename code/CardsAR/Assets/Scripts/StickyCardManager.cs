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

    private static System.Random rng = new System.Random();

    public bool _spread; // field
    public bool Spread   // property
    {
        get
        {
            if (_below)
            {
                return GetStickyCardManager(_below).Spread;
            }
            return _spread;
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
                StickyCardManager _old_below_manager = GetStickyCardManager(_old_below);
                _old_below_manager._above = null;
                if (_old_below_manager.CountAbove() == 1){
                    _old_below_manager._spread = false;
                }



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
        this.UpdateSelection();
        this.UpdateStickyHighlighted();
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
    public GameObject GetRandomDeck()
    {
        int countAbove = this.CountAbove();
        int randomPickInt = rng.Next(countAbove);
        GameObject card = GetBottomCard();
        while (randomPickInt>0)
        {
            randomPickInt -= 1;
            card = GetStickyCardManager(card)._above;
        }
        return card;
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
        if (this.selected)
        {
            highlight.transform.localScale = new Vector3(1.25f, 1.0f, 1.25f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            playingCard.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else if (this.highlighted)
        {
            highlight.transform.localScale = new Vector3(1.1f, 1.0f, 1.1f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            playingCard.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else if (this.stickyHighlighted || this.stickySelected)
        {
            highlight.transform.localScale = new Vector3(1.02f, 1.0f, 1.02f);
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
        this.stickyHighlighted = this.IsStickyHighlighted();
    }

    internal void DropOnTopOf(GameObject anotherCard)
    {
        selected = false;
        cursor = null;
        GameObject _old_below = _below;
        GameObject _new_below = anotherCard;
        if (_old_below)
        {
            StickyCardManager _old_below_manager = GetStickyCardManager(_old_below);
            _old_below_manager._above = null;
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

    }

    internal void DropUnderneath(GameObject anotherCard)
    {
        selected = false;
        cursor = null;
        GameObject _old_above = _above;
        GameObject _new_above = anotherCard;
        if (_old_above)
        {
            StickyCardManager _old_above_manager = GetStickyCardManager(_old_above);
            _old_above_manager._above = null;
        }
        if (_new_above)
        {
            GameObject previously_below_new_above = GetStickyCardManager(_new_above)._below;
            if (previously_below_new_above)
            {                
                GetStickyCardManager(previously_below_new_above)._above = gameObject;
                _below = previously_below_new_above;
            }
        }
        // and change the card below
        _above = _new_above;
        Spread = GetStickyCardManager(_above).Spread;
        transform.eulerAngles = _above.transform.eulerAngles;
        transform.position = _above.transform.position;
        if (_above)
        {
            GetStickyCardManager(_above)._below = gameObject;
        }
    }

    internal void DropOntoTable()
    {
        selected = false;
        cursor = null;
    }

    private bool IsStickyHighlighted()
    {
        bool highlightedBelow = ((_below) && GetStickyCardManager(_below).IsStickyHighlighted());
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
    }

    public bool IsStickySelected()
    {
        bool selectedBelow = ((_below) && GetStickyCardManager(_below).IsStickySelected());
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
        bool currentlySpread = this.Spread;
        this._spread = false;
        if (_above && _below){
            GetStickyCardManager(_below)._above = _above;
            GetStickyCardManager(_above)._below = _below;
        }
        else if (_below)
        {
            GetStickyCardManager(_below)._above = null;
            if(GetStickyCardManager(_below).CountAbove() == 1)
            {
                GetStickyCardManager(_below)._spread = false;
            }
        }
        else if (_above)
        {
            
            GetStickyCardManager(_above)._below = null;
            if (GetStickyCardManager(_above).CountAbove() == 1)
            {
                GetStickyCardManager(_above)._spread = false;
            }
            else
            {
                GetStickyCardManager(_above).Spread = currentlySpread;
            }
        }
        _below = null;
        _above = null;
    }

    public void UpdateTransform()
    {
        
        if (_below)
        {
            GetStickyCardManager(_below).UpdateTransform();
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

    internal void FlipDeck()
    {
        List<StickyCardManager> cardManagerList = new List<StickyCardManager>();
        StickyCardManager cardManager = this;

        // put the card managers into a list 
        while (cardManager != null)
        {
            cardManagerList.Add(cardManager);
            if (cardManager._above)
            {
                cardManager = GetStickyCardManager(cardManager._above);
            }
            else
            {
                cardManager = null;
            }
        }
        bool deckFaceUp = !cardManagerList[cardManagerList.Count - 1].FaceUp;
        bool deckSpread = cardManagerList[0]._spread;

        // re-order the card managers
        for (int c = 0; c < cardManagerList.Count; c++)
        {
            StickyCardManager currentCardManager = cardManagerList[c];
            currentCardManager.FaceUp = deckFaceUp;
            if (c == 0)
            {
                currentCardManager._above = null;
                currentCardManager._below = cardManagerList[c + 1].gameObject;
            }
            else if (c == cardManagerList.Count - 1)
            {
                currentCardManager._above = cardManagerList[c - 1].gameObject;
                currentCardManager._spread = deckSpread;
                currentCardManager._below = null;
                currentCardManager.gameObject.transform.position = cardManagerList[0].gameObject.transform.position;
            }
            else
            {
                currentCardManager._above = cardManagerList[c - 1].gameObject;
                currentCardManager._below = cardManagerList[c + 1].gameObject;

            }
        }
    }



    internal void ShuffleDeck()
    {
        List<StickyCardManager> cardManagerList = new List<StickyCardManager>();
        StickyCardManager cardManager = this;

        // put the card managers into a list 
        while (cardManager != null)
        {
            cardManagerList.Add(cardManager);
            if (cardManager._above)
            {
                cardManager = GetStickyCardManager(cardManager._above);
            }
            else
            {
                cardManager = null;
            }
        }
        bool deckFaceUp = cardManagerList[cardManagerList.Count - 1].FaceUp;
        bool deckSpread = cardManagerList[0]._spread;
        Vector3 deckPos = cardManagerList[0].gameObject.transform.position;
        //shuffle
        int n = cardManagerList.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            StickyCardManager value = cardManagerList[k];
            cardManagerList[k] = cardManagerList[n];
            cardManagerList[n] = value;
        }


        // order the card manager stacking
        for (int c = 0; c < cardManagerList.Count; c++)
        {
            StickyCardManager currentCardManager = cardManagerList[c];
            currentCardManager.FaceUp = deckFaceUp;
            if (c == 0)
            {
                currentCardManager._above = null;
                currentCardManager._below = cardManagerList[c + 1].gameObject;
            }
            else if (c == cardManagerList.Count - 1)
            {
                currentCardManager._above = cardManagerList[c - 1].gameObject;
                currentCardManager._spread = deckSpread;
                currentCardManager._below = null;
                currentCardManager.gameObject.transform.position = deckPos;
            }
            else
            {
                currentCardManager._above = cardManagerList[c - 1].gameObject;
                currentCardManager._below = cardManagerList[c + 1].gameObject;

            }
        }
    }

    internal void DeckPickUp(GameObject playerCursor)
    {
        cursor = playerCursor;
        selected = true;
        if (_below)
        {
            GetStickyCardManager(_below)._above = null;

            if (GetStickyCardManager(_below).CountAbove() == 1)
            {
                GetStickyCardManager(_below)._spread = false;
            }
        }
        _below = null;
        _spread = false;
    }

    private void UpdateSelectedCardPosition()
    {

        gameObject.transform.position = new Vector3(cursor.transform.position.x,
                                                    gameObject.transform.parent.gameObject.transform.position.y + selectedHeight,
                                                    cursor.transform.position.z);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,
                                                       cursor.transform.eulerAngles.y,
                                                       0.0f);

    }

    private void UpdateSingleCardPosition()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x,
                                                    gameObject.transform.parent.gameObject.transform.position.y + nonSelectedHeight, 
                                                    gameObject.transform.position.z);
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x,
                                                       gameObject.transform.eulerAngles.y,
                                                       0.0f);
    }

    private void UpdateStickyPosition()
    {
        Vector3 before = gameObject.transform.position;
        gameObject.transform.position = new Vector3(_below.transform.position.x,
                                                    _below.transform.position.y + thickness,
                                                    _below.transform.position.z);


        gameObject.transform.eulerAngles = new Vector3(_below.transform.eulerAngles.x,
                                                       _below.transform.eulerAngles.y,
                                                       0.0f);


    }

    internal void MagnetSingle(GameObject tableCard)
    {
        GetStickyCardManager(tableCard).SinglePickUp(cursor);
        selected = false;
        _below = tableCard;
        GetStickyCardManager(_below)._above = gameObject;
        GetStickyCardManager(GetBottomCard()).Spread = false;
    }

    private void UpdateSpreadPosition()
    {
        
        Vector3 before = gameObject.transform.position;
        float x_shift = Mathf.Cos(Mathf.Deg2Rad *gameObject.transform.eulerAngles.y) * spreadWidth;
        float z_shift = Mathf.Sin(Mathf.Deg2Rad * gameObject.transform.eulerAngles.y) * spreadWidth;
        gameObject.transform.position = new Vector3(_below.transform.position.x + x_shift,
                                                    _below.transform.position.y,
                                                    _below.transform.position.z - z_shift);
        gameObject.transform.eulerAngles = new Vector3(_below.transform.eulerAngles.x,
                                                       _below.transform.eulerAngles.y,
                                                       -5.0f);


    }

    internal void MagnetDeck(GameObject tableCard)
    {
        GetStickyCardManager(tableCard).DeckPickUp(cursor);
        selected = false;
        _below = GetStickyCardManager(tableCard).GetTopCard();
        GetStickyCardManager(_below)._above = gameObject;
        GetStickyCardManager(GetBottomCard()).Spread = false;
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
