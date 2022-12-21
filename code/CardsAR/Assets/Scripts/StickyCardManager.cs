using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StickyCardManager : MonoBehaviour
{
    public string cardID = "";

    public GameObject cursor;
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
 
    }




    private StickyCardManager GetStickyCardManager(GameObject card)
    {
        return card.GetComponent<StickyCardManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

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

            if (Spread)
            {
                cardCollider.transform.localScale = new Vector3(1.0f, 1.0f, 2.0f);
            }
            else
            {
                cardCollider.transform.localScale = new Vector3(2.0f, 1.0f, 2.0f);
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

  

    private void UpdateCardFace() {
        Vector3 CardScale = Vector3.one;
        if (!FaceUp)
        {
            CardScale.y = -1;
        }
        playingCard.transform.localScale = CardScale;

    }
}
