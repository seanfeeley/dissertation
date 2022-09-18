using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public Color playerColor = Color.magenta;

    public GameObject cursor;

    public static PlayerManager Instance;

    public List<GameObject> HighlightedCardCandidates;

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
            this.StopHighlightingCard(this.HighlightedCard);
        }
        if (this.HighlightedCard != card){
            this.HighlightedCard = card;
            this.HighlightedCard.GetComponent<StickyCardManager>().highlighted = true;
        }

    }

    internal void StopHighlightingCard(GameObject card)
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
        bool SpreadHighlighted = this.IsSpreadHighlighted();
        if (HeldCardCount == 0 && SpreadHighlighted) { GameManager.Instance.UpdateGameState(GameState.SpreadHighlighted_NoneHeld); }
        else if (HeldCardCount == 1 && SpreadHighlighted) { GameManager.Instance.UpdateGameState(GameState.SpreadHighlighted_OneHeld); }
        else if (HeldCardCount >= 2 && SpreadHighlighted) { GameManager.Instance.UpdateGameState(GameState.SpreadHighlighted_ManyHeld); }
        else if (HeldCardCount == 0 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_NoneHeld);}
        else if (HeldCardCount == 1 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_OneHeld);}
        else if (HeldCardCount >= 2 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameState(GameState.NoneHighlighted_ManyHeld);}
        else if (HeldCardCount == 0 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_NoneHeld);}
        else if (HeldCardCount == 1 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_OneHeld);}
        else if (HeldCardCount >= 2 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameState(GameState.OneHighlighted_ManyHeld); }
        else if (HeldCardCount == 0 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_NoneHeld); }
        else if (HeldCardCount == 1 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_OneHeld); }
        else if (HeldCardCount >= 2 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameState(GameState.ManyHighlighted_ManyHeld); }


    }

    private bool IsSpreadHighlighted()
    {
        if (HighlightedCard)
        {
            return GetStickyCardManager(HighlightedCard).Spread;
        }
        return false;
    }

    private int GetHeldCardCount()
    {
        if (HeldCard)
        {
            return HeldCard.GetComponent<StickyCardManager>().CountAbove();
        }
        return 0;
    }

    internal void AddToHighlightCandidates(GameObject gameObject)
    {
        if (!this.HighlightedCardCandidates.Contains(gameObject))
        {
            this.HighlightedCardCandidates.Add(gameObject);
            
        }
    }

    internal void RemoveFromToHighlightCandidates(GameObject gameObject)
    {
        if (this.HighlightedCardCandidates.Contains(gameObject))
        {
            this.HighlightedCardCandidates.Remove(gameObject);
            
        }
    }

    private void updateHighlight()
    {
        float closestDistance = float.MaxValue;
        GameObject closestCard = null;
        foreach (GameObject card in this.HighlightedCardCandidates)
        {
            float distance = Vector3.Distance(cursor.transform.position, card.transform.position);
            if (distance<closestDistance)
            {
                closestDistance = distance;
                closestCard = card;
            }
        }
        if (closestCard)
        {
            this.StartHighlightingCard(closestCard);
        }
        else if(this.HighlightedCard)
        {
            this.StopHighlightingCard(this.HighlightedCard);
        }



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

    private StickyCardManager GetStickyCardManager(GameObject card)
    {
        return card.GetComponent<StickyCardManager>();
    }



    public void PickupDropCard()
    {

        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            // drop
            if (HighlightedCard)
            {
                StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
                if (HighlightedManager.Spread)
                {
                    HeldManager.DropOnTopOf(HighlightedCard);
                }
                else
                {
                    HeldManager.DropOnTopOf(HighlightedManager.GetTopCard());
                }
            }
            else
            {
                HeldManager.DropOntoTable();
            }
            this.HeldCard = null;
        }
        else if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            // pickup
            if (HighlightedManager.Spread)
            {
                this.HeldCard = HighlightedCard;
            }
            else
            {
                this.HeldCard = HighlightedManager.GetTopCard();

            }
            GetStickyCardManager(HeldCard).SinglePickUp(cursor);


        }
        
    }
    public void PickupDropMiddleCard()
    {
        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            // drop
            if (HighlightedCard)
            {

                StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
                if (!HighlightedManager.Spread)
                {
                    HeldManager.DropUnderneath(HighlightedManager.GetRandomDeck());
                    this.HeldCard = null;
                }
            }
        }
        else if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            // pickup
            if (!HighlightedManager.Spread)
            {
               this.HeldCard = HighlightedManager.GetRandomDeck();
               GetStickyCardManager(HeldCard).SinglePickUp(cursor);

            }
            


        }

    }
    public void PickupDropBottomCard()
    {
        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            // drop
            if (HighlightedCard)
            {
                StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
                if (HighlightedManager.Spread)
                {
                    HeldManager.DropUnderneath(HighlightedCard);
                }
                else
                {
                    HeldManager.DropUnderneath(HighlightedManager.GetBottomCard());
                }
            }
            else
            {
                HeldManager.DropOntoTable();
            }
            this.HeldCard = null;
        }
        else if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            // pickup
            if (HighlightedManager.Spread)
            {
                this.HeldCard = HighlightedCard;
            }
            else
            {
                this.HeldCard = HighlightedManager.GetBottomCard();

            }
            GetStickyCardManager(HeldCard).SinglePickUp(cursor);


        }
    }
    public void PickupDropDeck()
    {
        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            // drop
            if (HighlightedCard)
            {
                HeldManager.DropOnTopOf(HighlightedCard);
            }
            else
            {
                HeldManager.DropOntoTable();
            }
            this.HeldCard = null;
        }
        else if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            // pickup
            if (HighlightedManager.Spread)
            {
                this.HeldCard = HighlightedCard;
            }
            else
            {
                this.HeldCard = HighlightedManager.GetBottomCard();

            }
            GetStickyCardManager(HeldCard).DeckPickUp(cursor);


        }
    }
    public void FlipCard()
    {
        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            if(HeldManager.CountAbove() == 1)
            {
                HeldManager.Flip();
            }
            else
            {
                GetStickyCardManager(HeldManager.GetTopCard()).Flip();
            }

        }
        else if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            if (HighlightedManager.Spread)
            {
                HighlightedManager.Flip();
            }
            else
            {
                StickyCardManager topCardManager = GetStickyCardManager(HighlightedManager.GetTopCard());
                topCardManager.Flip();
            }
        }
    }
    public void FlipDeck()
    {
        StickyCardManager DeckManager = null;
        // pick wether teh deck is highlighted or in hand
        if (HighlightedCard && GetStickyCardManager(HighlightedCard).CountAbove() >= 2)
        {
            DeckManager = GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard());
            DeckManager.FlipDeck();
        }
        else if (HeldCard && GetStickyCardManager(HeldCard).CountAbove() >= 2)
        {
            DeckManager = GetStickyCardManager(HeldCard);
            DeckManager.FlipDeck();
            DeckManager.DropOntoTable();
            HeldCard = DeckManager.GetBottomCard();
            GetStickyCardManager(HeldCard).DeckPickUp(cursor);
        }
    }
    public void SplitDeck()
    {
        if (HeldCard)
        {
            if (HighlightedCard)
            {

            }
            else
            {

            }
        }
        else if (HighlightedCard)
        {

        }
    }
    public void DealDeck()
    {
        if (HeldCard)
        {
            if (HighlightedCard)
            {

            }
            else
            {

            }
        }
        else if (HighlightedCard)
        {

        }
    }
    public void ShuffleDeck()
    {
        StickyCardManager DeckManager = null;
        // pick wether the deck is highlighted or in hand
        if (HighlightedCard && GetStickyCardManager(HighlightedCard).CountAbove() >= 2)
        {
            DeckManager = GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard());
            DeckManager.ShuffleDeck();
        }
        else if (HeldCard && GetStickyCardManager(HeldCard).CountAbove() >= 2)
        {
            DeckManager = GetStickyCardManager(HeldCard);
            DeckManager.ShuffleDeck();
            DeckManager.DropOntoTable();
            HeldCard = DeckManager.GetBottomCard();
            GetStickyCardManager(HeldCard).DeckPickUp(cursor);
        }
    }
    public void SpreadDeck()
    {
       if (HighlightedCard)
       {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            StickyCardManager BottomCardManager = GetStickyCardManager(HighlightedManager.GetBottomCard());
            BottomCardManager.Spread = !BottomCardManager.Spread;
       }
       
    }
    public void MagneticPickup()
    {
        if (HeldCard)
        {

            if (HighlightedCard)
            {
                GameObject OldHeldCard = HeldCard;

                StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
                StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
                // pickup
                if (HighlightedManager.Spread)
                {
                    HeldManager.MagnetSingle(HighlightedCard);
                    this.HeldCard = HighlightedCard;
                }
                else
                {
                    HeldManager.MagnetDeck(HighlightedCard);
                    this.HeldCard = HighlightedCard;
                }
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        this.UpdateGameState();
        this.updateHighlight();

    }


}
