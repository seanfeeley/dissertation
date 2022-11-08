using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public Color playerColor = Color.magenta;

    public GameObject cursor;
    public LayerMask cursorMask;

    public static PlayerManager Instance;

    public List<GameObject> HighlightedCardCandidates;


    public string uid { get; private set; }


    public GameObject HighlightedCard
    {
        get
        {

            return NetworkCardManager.Instance.HighlightedCard;

        }
    }
    public GameObject HeldCard
    {
        get
        {

            return NetworkCardManager.Instance.HeldCard; ;

        }
    }

    private void Awake()
    {
        Instance = this;
        //take the system string identifier, extract the numbers to get a unique int ID
        string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
        string deviceUniqueIdentifier_int = new String(deviceUniqueIdentifier.Where(Char.IsDigit).ToArray());
        this.uid = deviceUniqueIdentifier_int.Substring(0, 8);
    }


    //internal void StartHighlightingCard(GameObject card)
    //{
       
    //    if (CardDeckManager.Instance.HighlightedCard)
    //    {
    //        this.StopHighlightingCard(CardDeckManager.Instance.HighlightedCard);
    //    }
    //    if (CardDeckManager.Instance.HighlightedCard != card){
    //        //this.HighlightedCard = card;
    //        CardDeckManager.Instance.HighlightedCard.GetComponent<StickyCardManager>().Highlighted = true;
    //    }

    //}

    //internal void StopHighlightingCard(GameObject card)
    //{
    //    if (this.HighlightedCard == card)
    //    {
    //        this.HighlightedCard.GetComponent<StickyCardManager>().Highlighted = false;
    //        this.HighlightedCard = null;
    //    }
    //}

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

    internal Quaternion GetDealingRotation()
    {
        return new Quaternion();
    }

 
    internal float GetTableRotation()
    {
        
        return (360/MultiplayerNetworkingManager.Instance.GetCurrentPlayerCount())* MultiplayerNetworkingManager.Instance.GetMyPlayerIndex();
    }

    internal Vector3 GetPlayerRot()
    {
        Vector3 myDealPos = EnvironmentManager.Instance.lockedTablePlacerPos;
        Vector3 tableCenter = EnvironmentManager.Instance.GetTableCenter();
        Vector3 cameraReativeToDealPos = EnvironmentManager.Instance.camera.transform.position - myDealPos;
        Vector3 dealPosReativeTableCenter = myDealPos - tableCenter;
        float a = (float)EnvironmentManager.Instance.AngleBetween(new Vector3(0, 0, -1), dealPosReativeTableCenter);
        Debug.DrawLine(myDealPos, EnvironmentManager.Instance.camera.transform.position, Color.
                            red);
        Vector3 peerPosChange = EnvironmentManager.Instance.RotatePointAround(Vector3.zero,
                                                                              -a,
                                                                              cameraReativeToDealPos);
        Vector3 cameraLook = EnvironmentManager.Instance.camera.transform.eulerAngles;
        cameraLook.y += a;
        return cameraLook;
    }

    internal float GetDistanceToTable()
    {
        return 1.0f;
    }

    private bool IsSpreadHighlighted()
    {
        if (NetworkCardManager.Instance.HighlightedCard)
        {
            return GetStickyCardManager(NetworkCardManager.Instance.HighlightedCard).Spread;
        }
        return false;
    }

    internal Vector3 GetPlayerPos()
    {
        Vector3 myDealPos = EnvironmentManager.Instance.lockedTablePlacerPos;
        Vector3 tableCenter = EnvironmentManager.Instance.GetTableCenter();
        Vector3 cameraReativeToDealPos = EnvironmentManager.Instance.camera.transform.position - myDealPos;
        Vector3 dealPosReativeTableCenter = myDealPos - tableCenter;
        float a = (float)EnvironmentManager.Instance.AngleBetween(new Vector3(0, 0, -1), dealPosReativeTableCenter);
        Debug.DrawLine(myDealPos, EnvironmentManager.Instance.camera.transform.position, Color.
                            red);
        Vector3 peerPosChange = EnvironmentManager.Instance.RotatePointAround(Vector3.zero,
                                                                              -a,
                                                                              cameraReativeToDealPos);
        return peerPosChange;
    }

    private int GetHeldCardCount()
    {
        if (NetworkCardManager.Instance.HeldCard)
        {
            return NetworkCardManager.Instance.HeldCard.GetComponent<StickyCardManager>().CountAbove();
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
            if (this.IsCardAvailable(closestCard))
            {
                GetStickyCardManager(closestCard).Highlighted = true;
            }
        }
        else if(HighlightedCard)
        {
            GetStickyCardManager(HighlightedCard).Highlighted = false;
        }




    }

    private bool IsCardAvailable(GameObject closestCard)
    {
        return GetStickyCardManager(closestCard).Available;
    }

    private int GetHighlightedCardCount()
    {

        if (NetworkCardManager.Instance.HighlightedCard)
        {
            return GetStickyCardManager(NetworkCardManager.Instance.HighlightedCard).CountAbove();
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



    public void PickupCard()
    {
        if (HighlightedCard)
        {

            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            if (HeldCard)
            {
                StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
                if (HighlightedManager.Spread)
                {
                    NetworkCardManager.Instance.MagnetSingle(HighlightedManager.cardID);
                }
                else
                {
                    NetworkCardManager.Instance.MagnetDeck(GetStickyCardManager(HighlightedManager.GetTopCard()).cardID);
                }
            }
            else
            {
                if (HighlightedManager.Spread)
                {
                    NetworkCardManager.Instance.SinglePickUp(HighlightedManager.cardID);
                }
                else
                {
                    NetworkCardManager.Instance.SinglePickUp(GetStickyCardManager(HighlightedManager.GetTopCard()).cardID);
                }
            }
        }


    }
    public void DropCard()
    {

        //if (HeldCard)
        //{
        //    StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //    // drop
        //    if (HighlightedCard)
        //    {
        //        StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
        //        if (HighlightedManager.Spread)
        //        {
        //            HeldManager.DropOnTopOf(HighlightedCard);
        //        }
        //        else
        //        {
        //            HeldManager.DropOnTopOf(HighlightedManager.GetTopCard());
        //        }
        //    }
        //    else
        //    {
        //        HeldManager.DropOntoTable();
        //    }
        //    this.HeldCard = null;
        //}
           
    }
    public void PickupMiddleCard()
    {

        //if (HighlightedCard)
        //{
        //    StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
        //    if (HeldCard)
        //    {
        //        StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //        GameObject MiddleCard = HighlightedManager.GetRandomDeck();
        //        HeldManager.MagnetSingle(MiddleCard);
        //        this.HeldCard = MiddleCard;
        //    }
        //    else
        //    {
        //        this.HeldCard = HighlightedManager.GetRandomDeck();
        //        StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //        HeldManager.SinglePickUp(cursor);
        //    }
        //}

    }
    public void DropMiddleCard()
    {
        //if (HeldCard)
        //{
        //    // drop
        //    if (HighlightedCard)
        //    {
        //        StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //        StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
        //        if (!HighlightedManager.Spread)
        //        {
        //            HeldManager.DropUnderneath(HighlightedManager.GetRandomDeck());
        //            this.HeldCard = null;
        //        }
        //    }
        //}
        

    }
    public void PickupBottomCard()
    {

    //    if (HighlightedCard)
    //    {
    //        StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
    //        if (HeldCard)
    //        {
    //            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
    //            GameObject BottomCard = HighlightedManager.GetBottomCard();
    //            HeldManager.MagnetSingle(BottomCard);
    //            this.HeldCard = BottomCard;
    //        }
    //        else
    //        {
    //            this.HeldCard = HighlightedManager.GetBottomCard();
    //            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
    //            HeldManager.SinglePickUp(cursor);
    //        }
    //    }
    //}
    //public void DropBottomCard()
    //{
    //    if (HeldCard)
    //    {
    //        // drop
    //        if (HighlightedCard)
    //        {
    //            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
    //            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
    //            if (HighlightedManager.Spread)
    //            {
    //                HeldManager.DropUnderneath(HighlightedCard);
    //            }
    //            else
    //            {
    //                HeldManager.DropUnderneath(HighlightedManager.GetBottomCard());
    //            }
    //            this.HeldCard = null;
                
    //        }
    //    }
    }
    public void PickupDeck()
    {
        //if (HighlightedCard)
        //{
        //    StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
        //    GameObject BottomCard = HighlightedManager.GetBottomCard();
        //    if (HeldCard)
        //    {

        //        StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //        if (HighlightedManager.Spread)
        //        {
        //            this.HeldCard = HighlightedCard;
        //        }
        //        else
        //        {
        //            this.HeldCard = BottomCard;
        //        }
        //        HeldManager.MagnetDeck(HighlightedCard);

        //    }
        //    else
        //    {
        //        if (HighlightedManager.Spread)
        //        {
        //            this.HeldCard = HighlightedCard;
        //        }
        //        else
        //        {
        //            this.HeldCard = BottomCard;
        //        }
        //        StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //        HeldManager.DeckPickUp(cursor);
        //    }
        //}
    }
    public void DropDeck()
    {
        //if (HeldCard)
        //{
        //    StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //    // drop
        //    if (HighlightedCard)
        //    {
        //        StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);

        //        if (HighlightedManager.Spread)
        //        {
        //            HeldManager.DropOnTopOf(HighlightedCard);
        //        }
        //        else
        //        {
        //            HeldManager.DropOnTopOf(HighlightedManager.GetTopCard());
        //        }
        //    }
        //    else
        //    {
        //        HeldManager.DropOntoTable();
        //    }
        //    this.HeldCard = null;
        //}
    }
    public void FlipCard()
    {
        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            if (HeldManager.CountAbove() == 1)
            {
                NetworkCardManager.Instance.SetFaceUp(HeldManager.cardID, !HeldManager.FaceUp);
            }
            else
            {
                StickyCardManager TopCardManager = GetStickyCardManager(HeldManager.GetTopCard());
                NetworkCardManager.Instance.SetFaceUp(TopCardManager.cardID, !TopCardManager.FaceUp);
            }

        }
        else if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            if (HighlightedManager.Spread)
            {
                NetworkCardManager.Instance.SetFaceUp(HighlightedManager.cardID, !HighlightedManager.FaceUp);
            }
            else
            {
                StickyCardManager topCardManager = GetStickyCardManager(HighlightedManager.GetTopCard());
                NetworkCardManager.Instance.SetFaceUp(topCardManager.cardID, !topCardManager.FaceUp);
            }
        }
    }
    public void FlipDeck()
    {
        //StickyCardManager DeckManager = null;
        //// pick wether teh deck is highlighted or in hand
        //if (HighlightedCard && GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard()).CountAbove() >= 2)
        //{
        //    DeckManager = GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard());
        //    DeckManager.FlipDeck();
        //}
        //else if (HeldCard && GetStickyCardManager(HeldCard).CountAbove() >= 2)
        //{
        //    DeckManager = GetStickyCardManager(HeldCard);
        //    DeckManager.FlipDeck();
        //    DeckManager.DropOntoTable();
        //    HeldCard = DeckManager.GetBottomCard();
        //    GetStickyCardManager(HeldCard).DeckPickUp(cursor);
        //}
    }
    public void SplitDeck()
    {
        //if (HeldCard)
        //{
        //    if (HighlightedCard)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}
        //else if (HighlightedCard)
        //{

        //}
    }
    public void DealDeck()
    {
        //if (HeldCard)
        //{
        //    if (HighlightedCard)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}
        //else if (HighlightedCard)
        //{

        //}
    }
    public void ShuffleDeck()
    {
        //StickyCardManager DeckManager = null;
        //// pick wether the deck is highlighted or in hand
        //if (HighlightedCard && GetStickyCardManager(HighlightedCard).CountAbove() >= 2)
        //{
        //    DeckManager = GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard());
        //    DeckManager.ShuffleDeck();
        //}
        //else if (HeldCard && GetStickyCardManager(HeldCard).CountAbove() >= 2)
        //{
        //    DeckManager = GetStickyCardManager(HeldCard);
        //    DeckManager.ShuffleDeck();
        //    DeckManager.DropOntoTable();
        //    HeldCard = DeckManager.GetBottomCard();
        //    GetStickyCardManager(HeldCard).DeckPickUp(cursor);
        //}
    }
    public void SpreadDeck()
    {
        if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            StickyCardManager BottomCardManager = GetStickyCardManager(HighlightedManager.GetBottomCard());
            NetworkCardManager.Instance.SetSpread(BottomCardManager.cardID, !BottomCardManager.Spread);
        }

    }
    public void MagneticPickup()
    {
        //if (HeldCard)
        //{

        //    if (HighlightedCard)
        //    {
        //        GameObject OldHeldCard = HeldCard;

        //        StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
        //        StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
        //        // pickup
        //        if (HighlightedManager.Spread)
        //        {
        //            HeldManager.MagnetSingle(HighlightedCard);
        //            this.HeldCard = HighlightedCard;
        //        }
        //        else
        //        {
        //            HeldManager.MagnetDeck(HighlightedCard);
        //            this.HeldCard = HighlightedCard;
        //        }
        //    }
        //}

    }
    // Update is called once per frame
    void Update()
    {
        this.UpdateGameState();
        this.updateHighlight();

    }


}
