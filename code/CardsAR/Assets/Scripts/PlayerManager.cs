using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public Color playerColor = Color.magenta;

    public GameObject cursor;

    public static PlayerManager Instance;

    public GameObject HighlightedCard;

    public GameObject HeldCard;


    private void Awake()
    {
        Instance = this;

    }

    internal void StartHighlightingCard(GameObject card)
    {
        if (this.HighlightedCard)
        {
            this.StopHighlightingcard(this.HighlightedCard);
        }
        if (this.HighlightedCard != card){
            this.HighlightedCard = card;
            this.HighlightedCard.GetComponent<StickyCardManager>().highlighted = true;
        }

    }

    internal void StopHighlightingcard(GameObject card)
    {
        if (this.HighlightedCard == card)
        {
            this.HighlightedCard.GetComponent<StickyCardManager>().highlighted = false;
            this.HighlightedCard = null;
        }
    }

    private void UpdateGameState()
    {

        int HeldCardCount = this.GetHeldCardCount();
            
        int HighlightedCardCount = this.GetHighlightedCardCount();
        if (HeldCardCount == 0 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_NoneHeld);}
        if (HeldCardCount == 1 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_OneHeld);}
        if (HeldCardCount >= 2 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_ManyHeld);}
        if (HeldCardCount == 0 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_NoneHeld);}
        if (HeldCardCount == 1 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_OneHeld);}
        if (HeldCardCount >= 2 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_ManyHeld);}
        if (HeldCardCount == 0 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_NoneHeld);}
        if (HeldCardCount == 1 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_OneHeld);}
        if (HeldCardCount >= 2 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_ManyHeld);}
    }

    private int GetHeldCardCount()
    {
        if (HeldCard)
        {
            return HeldCard.GetComponent<StickyCardManager>().CountAbove();
        }
        return 0;
    }
    private int GetHighlightedCardCount()
    {

        if (HighlightedCard)
        {
            return HighlightedCard.GetComponent<StickyCardManager>().CountAbove();
        }
        return 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.SetCursorColor();
    }

    private void SetCursorColor()
    {
        Transform highlightTransform = cursor.transform.Find("Sphere");
        Material highlightMaterial = highlightTransform.GetComponent<Renderer>().material;
        highlightMaterial.SetColor("_Color", playerColor);
    }



    public void PickupDropCard()
    {
        if (HeldCard)
        {
            // drop
            if (HighlightedCard)
            {
                // drop onto another card
                this.HeldCard.GetComponent<StickyCardManager>().StickyCard_Below = HighlightedCard.GetComponent<StickyCardManager>().GetTopCard();
                this.HeldCard.GetComponent<StickyCardManager>().selected = false;
                this.HeldCard = null;
            }
            else
            {
                // drop on table
                this.HeldCard.GetComponent<StickyCardManager>().selected = false;
                this.HeldCard = null;
            }
        }
        else if (HighlightedCard)
        {
            // pickup
            this.HeldCard = HighlightedCard.GetComponent<StickyCardManager>().GetTopCard();
            this.HeldCard.GetComponent<StickyCardManager>().selected = true;
            this.HeldCard.GetComponent<StickyCardManager>().StickyCard_Below = null;
            
        }
        
    }
    public void PickupDropMiddleCard()
    {

    }
    public void PickupDropBottomCard()
    {

    }
    public void PickupDropDeck()
    {

    }
    public void FlipCard()
    {

    }
    public void FlipDeck()
    {

    }
    public void SplitDeck()
    {

    }
    public void DealDeck()
    {

    }
    public void ShuffleDeck()
    {

    }


    // Update is called once per frame
    void Update()
    {
        this.UpdateGameState();
        this.MoveHeldCard();
        
    }

    private void MoveHeldCard()
    {
        if (HeldCard)
        {
            HeldCard.GetComponent<StickyCardManager>().transform.position = new Vector3(cursor.transform.position.x,
                                                                                        HeldCard.GetComponent<StickyCardManager>().transform.position.y,
                                                                                        cursor.transform.position.z);
        }
    }
}
