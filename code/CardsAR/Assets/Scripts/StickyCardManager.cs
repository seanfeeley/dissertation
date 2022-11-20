using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StickyCardManager : MonoBehaviour
{
    public string cardID = "";

    public GameObject cursor;
    //public bool selected = false;
    //public bool stickySelected = false;
    //public bool highlighted = false;
    public bool stickyHighlighted = false;
    
    public bool inHiddenZone = false;
    
    private float thickness = 0.001f;
    private float spreadWidth = 0.018f;
    private float nonSelectedHeight = 0.001f;
    private float selectedHeight = 0.1f;
    public Color outlineColor = Color.cyan;
    public GameObject[] cardColliders;
    public GameObject hiddenCard;
    public GameObject playingCard;
    public GameObject highlight;
    public Vector3 CursorCollisionPoint;
    private static System.Random rng = new System.Random();

    public bool Held
    {
        get
        {
            return NetworkCardManager.Instance.IsHeld(cardID);
        }   // get method
        set
        {

        }  // set method
    }
    public string HeldBy
    {
        get
        {
            return NetworkCardManager.Instance.HeldBy(cardID);
        }   // get method
    }
    public bool Highlighted
    {
        get
        {
            bool ret = NetworkCardManager.Instance.IsHighlighted(cardID);
            //Debug.Log(cardID +" "+ ret.ToString());
            return ret;
        }   // get method
        set
        {

            bool current_value = Highlighted;
            if (current_value != value)
            {
                NetworkCardManager.Instance.SetHighlighted(cardID, value);
            }
        }  // set method
    }
    public bool Available
    {
        get
        {

            if (Below)
            {
                return GetStickyCardManager(Below).Available;
            }
            return  NetworkCardManager.Instance.IsAvailableToHighlight(cardID);
        }   // get method 
    }
    public string HiddenBy
    {
        get
        {
            return NetworkCardManager.Instance.IsHidden(cardID);
        }   // get method
    }
    
    //public bool _faceUp; // field
    public bool FaceUp
    {
        get
        {
            return NetworkCardManager.Instance.GetFaceUp(cardID);
        }   // get method
        set {
            bool current_value = FaceUp;
            if (current_value != value)
            {
                NetworkCardManager.Instance.SetFaceUp(cardID, value);
            }
        }  // set method
    }

    //public bool _spread; // field
    public bool Spread   // property
    {
        get
        {
            bool ret;
            //if (Below)
            //{
            //    ret = GetStickyCardManager(Below).Spread;
            //}
            //else
            //{
            ret = NetworkCardManager.Instance.GetSpread(cardID);
            //}
            
            //Debug.Log(ret);
            return ret;
        }   // get method
        set {
            bool current_value = Spread;
            if (current_value != value)
            {
                NetworkCardManager.Instance.SetSpread(cardID, value);
            }
        }  // set method
    }



    //public GameObject _above; // field
    public GameObject Above   // property
    {
        get
        {

            return NetworkCardManager.Instance.GetAbove(cardID); ;

        }
    }

    //public GameObject _below; // field
    public GameObject Below   // property
    {
        get
        {

            return NetworkCardManager.Instance.GetBelow(cardID); ;

        }   // get method
        //set {
        //    GameObject _old_below = _below;
        //    GameObject _new_below = value;
        //    if (_old_below)
        //    {
        //        StickyCardManager _old_below_manager = GetStickyCardManager(_old_below);
        //        _old_below_manager._above = null;
        //        if (_old_below_manager.CountAbove() == 1){
        //            _old_below_manager._spread = false;
        //        }



        //    }
        //    if (_new_below)
        //    {
        //        GameObject previously_above_new_below = GetStickyCardManager(_new_below)._above;
        //        if (previously_above_new_below)
        //        {

        //            // we are inserting into the middle of a deck of cards
        //            // so change the card above

        //            GetStickyCardManager(previously_above_new_below)._below = GetTopCard();
        //            _above = previously_above_new_below;
        //        }

        //    }
        //    // and change the card below
        //    _below = _new_below;
        //    if (_below)
        //    {
        //        GetStickyCardManager(_below)._above = gameObject;
        //    }
        //}  // set method
    }




    private StickyCardManager GetStickyCardManager(GameObject card)
    {
        return card.GetComponent<StickyCardManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //if (Below != null)
        //{
        //    GetStickyCardManager(_below)._above = gameObject;
        //}
        InvokeRepeating("UpdateCardInfo", 1.0f, .50f);
    }

    // Update is called once per frame
    void UpdateCardInfo()
    {
        this.UpdateTransform();
        this.UpdateStickyHighlighted();
        this.UpdateHighlightColliders();
        this.UpdateOutline();
        this.UpdateHidden();
        this.UpdateCardFace();

    }

    private void UpdateHighlightColliders()
    {
        bool underCursor = false;

        foreach (GameObject cardCollider in cardColliders)
        {
            underCursor = underCursor || cardCollider.transform.GetComponent<CursorTracker>().UnderCursor;
            if (cardCollider.transform.GetComponent<CursorTracker>().UnderCursor)
            {
                this.CursorCollisionPoint = cardCollider.transform.GetComponent<CursorTracker>().CollisionPoint;
            }

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
        if (Below)
        {
            return GetStickyCardManager(Below).GetBottomCard();
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
            card = GetStickyCardManager(card).Above;
        }
        return card;
    }


    public int CountAbove()
    {
        if (Above)
        {
            return 1 + GetStickyCardManager(Above).CountAbove();
        }
        else
        {
            return 1;
        }
    }

    public GameObject GetTopCard()
    {
        if (Above)
        {
            return GetStickyCardManager(Above).GetTopCard();
        }
        else
        {
            return gameObject;
        }
    }

    private void UpdateHidden()
    {
        bool hiddenToMe = this.HiddenBy != "" && this.HiddenBy != PlayerManager.Instance.uid;
        bool hiddenByMe = this.HiddenBy == PlayerManager.Instance.uid;

        hiddenCard.SetActive(hiddenToMe);
        Material cardMaterial = playingCard.GetComponent<Renderer>().material;

        if (hiddenByMe)
        {
            cardMaterial.SetColor("_Color", new Color(0.5f, 1.0f, 1.0f));
            cardMaterial.SetColor("_EmissionColor", new Color(0.5f, 1.0f, 1.0f));
        }
        else
        {
            cardMaterial.SetColor("_Color", Color.white);
            cardMaterial.SetColor("_EmissionColor", Color.white);
        }
    }


    private void UpdateOutline()
    {
        Material highlightMaterial = highlight.GetComponent<Renderer>().material;
        highlightMaterial.SetColor("_Color", outlineColor);
        highlightMaterial.SetColor("_EmissionColor", outlineColor);
        if (this.Held)
        {
            highlight.transform.localScale = new Vector3(1.3f, 1.0f, 1.3f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            playingCard.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else if (this.Highlighted)
        {
            highlight.transform.localScale = new Vector3(1.2f, 1.0f, 1.2f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            playingCard.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else if (this.stickyHighlighted || this.IsStickySelected())
        {
            highlight.transform.localScale = new Vector3(1.01f, 1.0f, 1.01f);
            gameObject.layer = LayerMask.NameToLayer("HighlightedCards");
            playingCard.layer = LayerMask.NameToLayer("HighlightedCards");
            hiddenCard.layer = LayerMask.NameToLayer("HighlightedCards");
        }
        else if (this.Available == false)
        {
            highlightMaterial.SetColor("_Color", Color.red);
            highlightMaterial.SetColor("_EmissionColor", Color.red);
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
        this.stickyHighlighted = this.IsStickyHighlighted();
    }

    internal void DropOnTopOf(GameObject anotherCard)
    {
        //selected = false;
        //cursor = null;
        //GameObject _old_below = _below;
        //GameObject _new_below = anotherCard;
        //if (_old_below)
        //{
        //    StickyCardManager _old_below_manager = GetStickyCardManager(_old_below);
        //    _old_below_manager._above = null;
        //}
        //if (_new_below)
        //{
        //    GameObject previously_above_new_below = GetStickyCardManager(_new_below)._above;
        //    if (previously_above_new_below)
        //    {
        //        // we are inserting into the middle of a deck of cards
        //        // so change the card above
        //        GetStickyCardManager(previously_above_new_below)._below = GetTopCard();
        //        _above = previously_above_new_below;
        //    }
        //}
        //// and change the card below
        //_below = _new_below;
        //if (_below)
        //{
        //    GetStickyCardManager(_below)._above = gameObject;
        //}

    }

    internal void DropUnderneath(GameObject anotherCard)
    {
        //selected = false;
        //cursor = null;
        //GameObject _old_above = _above;
        //GameObject _new_above = anotherCard;
        //if (_old_above)
        //{
        //    StickyCardManager _old_above_manager = GetStickyCardManager(_old_above);
        //    _old_above_manager._above = null;
        //}
        //if (_new_above)
        //{
        //    GameObject previously_below_new_above = GetStickyCardManager(_new_above)._below;
        //    if (previously_below_new_above)
        //    {                
        //        GetStickyCardManager(previously_below_new_above)._above = gameObject;
        //        _below = previously_below_new_above;
        //    }
        //}
        //// and change the card below
        //_above = _new_above;
        //Spread = GetStickyCardManager(_above).Spread;
        //transform.eulerAngles = _above.transform.eulerAngles;
        //transform.position = _above.transform.position;
        //if (_above)
        //{
        //    GetStickyCardManager(_above)._below = gameObject;
        //}
    }

    internal void DropOntoTable()
    {
        //selected = false;
        //cursor = null;
    }

    private bool IsStickyHighlighted()
    {
        bool highlightedBelow = ((Below) && GetStickyCardManager(Below).IsStickyHighlighted());
        if (highlightedBelow)
        {
            this.Highlighted = false;
            return true;
        }
        else
        {
            return this.Highlighted;
        }
    }


 
    public bool IsStickySelected()
    {
        bool selectedBelow = ((Below) && GetStickyCardManager(Below).IsStickySelected());
        if (selectedBelow)
        {
            //this.selected = false;
            return true;
        }
        else
        {
            return this.Held;
        }
    }

    internal void SinglePickUp(GameObject playerCursor)
    {
        //cursor = playerCursor;
        //selected = true;
        //bool currentlySpread = this.Spread;
        //this._spread = false;
        //if (Above && Below){
        //    GetStickyCardManager(_below)._above = _above;
        //    GetStickyCardManager(_above)._below = _below;
        //}
        //else if (_below)
        //{
        //    GetStickyCardManager(_below)._above = null;
        //    if(GetStickyCardManager(_below).CountAbove() == 1)
        //    {
        //        GetStickyCardManager(_below)._spread = false;
        //    }
        //}
        //else if (_above)
        //{
            
        //    GetStickyCardManager(_above)._below = null;
        //    if (GetStickyCardManager(_above).CountAbove() == 1)
        //    {
        //        GetStickyCardManager(_above)._spread = false;
        //    }
        //    else
        //    {
        //        GetStickyCardManager(_above).Spread = currentlySpread;
        //    }
        //}
        //_below = null;
        //_above = null;
    }

    public void UpdateTransform()
    { 
        if (Below)
        {
            GetStickyCardManager(Below).UpdateTransform();
            if (this.Spread)
            {
                this.UpdateSpreadPosition();
            }
            else
            {
                this.UpdateStickyPosition();
            }

        }
        else if (HeldBy != "")
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
        //List<StickyCardManager> cardManagerList = new List<StickyCardManager>();
        //StickyCardManager cardManager = this;

        //// put the card managers into a list 
        //while (cardManager != null)
        //{
        //    cardManagerList.Add(cardManager);
        //    if (cardManager._above)
        //    {
        //        cardManager = GetStickyCardManager(cardManager._above);
        //    }
        //    else
        //    {
        //        cardManager = null;
        //    }
        //}
        //bool deckFaceUp = !cardManagerList[cardManagerList.Count - 1].FaceUp;
        //bool deckSpread = cardManagerList[0]._spread;

        //// re-order the card managers
        //for (int c = 0; c < cardManagerList.Count; c++)
        //{
        //    StickyCardManager currentCardManager = cardManagerList[c];
        //    currentCardManager.FaceUp = deckFaceUp;
        //    if (c == 0)
        //    {
        //        currentCardManager._above = null;
        //        currentCardManager._below = cardManagerList[c + 1].gameObject;
        //    }
        //    else if (c == cardManagerList.Count - 1)
        //    {
        //        currentCardManager._above = cardManagerList[c - 1].gameObject;
        //        currentCardManager._spread = deckSpread;
        //        currentCardManager._below = null;
        //        currentCardManager.gameObject.transform.position = cardManagerList[0].gameObject.transform.position;
        //    }
        //    else
        //    {
        //        currentCardManager._above = cardManagerList[c - 1].gameObject;
        //        currentCardManager._below = cardManagerList[c + 1].gameObject;

        //    }
        //}
    }



    internal void ShuffleDeck()
    {
        //List<StickyCardManager> cardManagerList = new List<StickyCardManager>();
        //StickyCardManager cardManager = this;

        //// put the card managers into a list 
        //while (cardManager != null)
        //{
        //    cardManagerList.Add(cardManager);
        //    if (cardManager._above)
        //    {
        //        cardManager = GetStickyCardManager(cardManager._above);
        //    }
        //    else
        //    {
        //        cardManager = null;
        //    }
        //}
        //bool deckFaceUp = cardManagerList[cardManagerList.Count - 1].FaceUp;
        //bool deckSpread = cardManagerList[0]._spread;
        //Vector3 deckPos = cardManagerList[0].gameObject.transform.position;
        ////shuffle
        //int n = cardManagerList.Count;
        //while (n > 1)
        //{
        //    n--;
        //    int k = rng.Next(n + 1);
        //    StickyCardManager value = cardManagerList[k];
        //    cardManagerList[k] = cardManagerList[n];
        //    cardManagerList[n] = value;
        //}


        //// order the card manager stacking
        //for (int c = 0; c < cardManagerList.Count; c++)
        //{
        //    StickyCardManager currentCardManager = cardManagerList[c];
        //    currentCardManager.FaceUp = deckFaceUp;
        //    if (c == 0)
        //    {
        //        currentCardManager._above = null;
        //        currentCardManager._below = cardManagerList[c + 1].gameObject;
        //    }
        //    else if (c == cardManagerList.Count - 1)
        //    {
        //        currentCardManager._above = cardManagerList[c - 1].gameObject;
        //        currentCardManager._spread = deckSpread;
        //        currentCardManager._below = null;
        //        currentCardManager.gameObject.transform.position = deckPos;
        //    }
        //    else
        //    {
        //        currentCardManager._above = cardManagerList[c - 1].gameObject;
        //        currentCardManager._below = cardManagerList[c + 1].gameObject;

        //    }
        //}
    }

    internal void DeckPickUp(GameObject playerCursor)
    {
        //cursor = playerCursor;
        //selected = true;
        //if (_below)
        //{
        //    GetStickyCardManager(_below)._above = null;

        //    if (GetStickyCardManager(_below).CountAbove() == 1)
        //    {
        //        GetStickyCardManager(_below)._spread = false;
        //    }
        //}
        //_below = null;
        //_spread = false;
    }

    private void UpdateSelectedCardPosition()
    {
        Vector3 networkPos = NetworkCardManager.Instance.GetPostion(cardID);
        Vector3 networkRot = NetworkCardManager.Instance.GetRotation(cardID);
        gameObject.transform.localPosition = new Vector3(networkPos.x,
                                                         networkPos.y + selectedHeight,
                                                         networkPos.z);
        gameObject.transform.localEulerAngles = new Vector3(networkRot.x,
                                                       networkRot.y,
                                                       Spread? -5.0f : 0.0f);


    }

    private void UpdateSingleCardPosition()
    {
        Vector3 networkPos = NetworkCardManager.Instance.GetPostion(cardID);
        Vector3 networkRot = NetworkCardManager.Instance.GetRotation(cardID);
        gameObject.transform.localPosition = new Vector3(networkPos.x,
                                                    networkPos.y + nonSelectedHeight,
                                                    networkPos.z);
        gameObject.transform.localEulerAngles = new Vector3(networkRot.x,
                                                       networkRot.y,
                                                      Spread ? -5.0f : 0.0f);
    }

    private void UpdateStickyPosition()
    {
        Vector3 before = gameObject.transform.position;
        gameObject.transform.localPosition = new Vector3(Below.transform.localPosition.x,
                                                    Below.transform.localPosition.y + thickness,
                                                    Below.transform.localPosition.z);


        gameObject.transform.localEulerAngles = new Vector3(Below.transform.localEulerAngles.x,
                                                       Below.transform.localEulerAngles.y,
                                                       Spread ? -5.0f : 0.0f);


    }

    internal void MagnetSingle(GameObject tableCard)
    {
        //GetStickyCardManager(tableCard).SinglePickUp(cursor);
        //selected = false;
        //_below = tableCard;
        //GetStickyCardManager(_below)._above = gameObject;
        //GetStickyCardManager(GetBottomCard()).Spread = false;
    }

    private void UpdateSpreadPosition()
    {

        Vector3 before = gameObject.transform.position;
        float x_shift = Mathf.Cos(Mathf.Deg2Rad * gameObject.transform.eulerAngles.y) * spreadWidth;
        float z_shift = Mathf.Sin(Mathf.Deg2Rad * gameObject.transform.eulerAngles.y) * spreadWidth;
        gameObject.transform.position = new Vector3(Below.transform.position.x + x_shift,
                                                    Below.transform.position.y,
                                                    Below.transform.position.z - z_shift);
        gameObject.transform.eulerAngles = new Vector3(Below.transform.eulerAngles.x,
                                                       Below.transform.eulerAngles.y,
                                                       Spread ? -5.0f : 0.0f);


    }

    internal void MagnetDeck(GameObject tableCard)
    {
        //GetStickyCardManager(tableCard).DeckPickUp(cursor);
        //selected = false;
        //_below = GetStickyCardManager(tableCard).GetTopCard();
        //GetStickyCardManager(_below)._above = gameObject;
        //GetStickyCardManager(GetBottomCard()).Spread = false;
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
