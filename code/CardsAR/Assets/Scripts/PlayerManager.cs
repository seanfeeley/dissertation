using System;
using System.Collections;
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
    // Update is called once per frame
    void Update()
    {
        this.UpdateGameState();


    }

    private void UpdateGameState()
    {

        int HeldCardCount = this.GetHeldCardCount();
        int HighlightedCardCount = this.GetHighlightedCardCount();
        bool SpreadHighlighted = this.IsSpreadHighlighted();
        if (GameManager.Instance.InCardGameplayMode())
        {
            if (HeldCardCount == 0 && SpreadHighlighted) { GameManager.Instance.UpdateGameStateTo_SpreadHighlighted_NoneHeld(); }
        else if (HeldCardCount == 1 && SpreadHighlighted) { GameManager.Instance.UpdateGameStateTo_SpreadHighlighted_OneHeld(); }
        else if (HeldCardCount >= 2 && SpreadHighlighted) { GameManager.Instance.UpdateGameStateTo_SpreadHighlighted_ManyHeld(); }
        else if (HeldCardCount == 0 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameStateTo_NoneHighlighted_NoneHeld();}
        else if (HeldCardCount == 1 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameStateTo_NoneHighlighted_OneHeld();}
        else if (HeldCardCount >= 2 && HighlightedCardCount == 0) { GameManager.Instance.UpdateGameStateTo_NoneHighlighted_ManyHeld();}
        else if (HeldCardCount == 0 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameStateTo_OneHighlighted_NoneHeld();}
        else if (HeldCardCount == 1 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameStateTo_OneHighlighted_OneHeld();}
        else if (HeldCardCount >= 2 && HighlightedCardCount == 1) { GameManager.Instance.UpdateGameStateTo_OneHighlighted_ManyHeld(); }
        else if (HeldCardCount == 0 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameStateTo_ManyHighlighted_NoneHeld(); }
        else if (HeldCardCount == 1 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameStateTo_ManyHighlighted_OneHeld(); }
        else if (HeldCardCount >= 2 && HighlightedCardCount >= 2) { GameManager.Instance.UpdateGameStateTo_ManyHighlighted_ManyHeld(); }
        }
        


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
        Debug.DrawLine(myDealPos, EnvironmentManager.Instance.camera.transform.position, Color.red);
        
        Vector3 peerPosChange = EnvironmentManager.Instance.RotatePointAround(Vector3.zero,
                                                                              -a,
                                                                              cameraReativeToDealPos);
        //Debug.Log(peerPosChange);
        return peerPosChange;
    }

    private int GetHeldCardCount()
    {
        if (NetworkCardManager.Instance.HeldCard)
        {
            return GetStickyCardManager(HeldCard).CountAbove();
        }
        return 0;
    }

    internal void AddToHighlightCandidates(GameObject gameObject)
    {
        if (!this.HighlightedCardCandidates.Contains(gameObject))
        {
            this.HighlightedCardCandidates.Add(gameObject);
            
        }
        this.updateHighlight();
    }

    internal void RemoveFromToHighlightCandidates(GameObject gameObject)
    {
        if (this.HighlightedCardCandidates.Contains(gameObject))
        {
            this.HighlightedCardCandidates.Remove(gameObject);
            
        }
        this.updateHighlight();
    }

    private void updateHighlight()
    {
        float closestDistance = float.MinValue;
        GameObject closestCard = null;
        foreach (GameObject card in this.HighlightedCardCandidates)
        {
            float distance = GetStickyCardManager(card).CursorCollisionPoint.y;

            if (distance>closestDistance)
            {
                closestDistance = distance;
                closestCard = card;
            }
        }
        if (closestCard)
        {
            if (this.IsCardAvailable(closestCard))
            {
                if (GetStickyCardManager(closestCard).Spread)
                {
                    GetStickyCardManager(closestCard).Highlighted = true;
                }
                else
                {
                    GetStickyCardManager(GetStickyCardManager(closestCard).GetBottomCard()).Highlighted = true;
                }
                
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



    private StickyCardManager GetStickyCardManager(GameObject card)
    {
        return card.GetComponent<StickyCardManager>();
    }



    public void PickupCard(GameObject button)
    {
        MenuManager.Instance.TempDisableButtons();
        if (HighlightedCard)
        {

            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            if (HeldCard)
            {
                StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
                if (HighlightedManager.Spread)
                {
                    NetworkCardManager.Instance.MagnetSingle(HeldManager.cardID, HighlightedManager.cardID);
                }
                else
                {

                    NetworkCardManager.Instance.MagnetSingle(HeldManager.cardID, GetStickyCardManager(HighlightedManager.GetTopCard()).cardID);
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
        StartCoroutine(DelayMenuEnableButtons());


    }

    IEnumerator DelayMenuEnableButtons()
    {
        //MenuManager.Instance.TempDisableButtons();
        yield return new WaitForSecondsRealtime(1f);
        MenuManager.Instance.StopTempDisableButtons();
        MenuManager.Instance.ForceRefresh();
    }

    public void DropCard(GameObject button)
    {

        MenuManager.Instance.TempDisableButtons();
        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            // drop
            if (HighlightedCard)
            {
                StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
                if (HighlightedManager.Spread)
                {
                    NetworkCardManager.Instance.DropOneCardOnTopOf(HeldManager.cardID,
                                                            HighlightedManager.cardID);
                }
                else
                {
                    NetworkCardManager.Instance.DropOneCardOnTopOf(HeldManager.cardID,
                                                            GetStickyCardManager(HighlightedManager.GetTopCard()).cardID);
                }
            }
            else
            {
                NetworkCardManager.Instance.DropOneCardOntoTable(HeldManager.cardID);
            }
        }
        StartCoroutine(DelayMenuEnableButtons());

    }
    public void PickupMiddleCard(GameObject button)
    {

        MenuManager.Instance.TempDisableButtons();
        if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            if (HeldCard)
            {
                StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
                GameObject MiddleCard = HighlightedManager.GetRandomDeck();
                NetworkCardManager.Instance.MagnetSingle(HeldManager.cardID, GetStickyCardManager(MiddleCard).cardID);
            }
            else
            {
                GameObject MiddleCard = HighlightedManager.GetRandomDeck();
                NetworkCardManager.Instance.SinglePickUp(GetStickyCardManager(MiddleCard).cardID);
            }
        }
        StartCoroutine(DelayMenuEnableButtons());

    }

    public void PickupBottomCard(GameObject button)
    {

        MenuManager.Instance.TempDisableButtons();
        if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            if (HeldCard)
            {
                StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
                GameObject BottomCard = HighlightedManager.GetBottomCard();
                NetworkCardManager.Instance.MagnetSingle(HeldManager.cardID, GetStickyCardManager(BottomCard).cardID);
            }
            else
            {
                GameObject BottomCard = HighlightedManager.GetBottomCard();
                NetworkCardManager.Instance.SinglePickUp(GetStickyCardManager(BottomCard).cardID);
            }
        }
        StartCoroutine(DelayMenuEnableButtons());
    }

    public void PickupDeck(GameObject button)
    {
        MenuManager.Instance.TempDisableButtons();
        if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            GameObject BottomCard = HighlightedManager.GetBottomCard();
            GameObject TopCard = HighlightedManager.GetTopCard();
            StickyCardManager BottomCardManager = GetStickyCardManager(BottomCard);
            StickyCardManager TopCardManager = GetStickyCardManager(TopCard);
            if (HeldCard)
            {

                StickyCardManager HeldManager = GetStickyCardManager(HeldCard);

                NetworkCardManager.Instance.MagnetDeck(HeldManager.cardID,
                                                        BottomCardManager.cardID,
                                                        TopCardManager.cardID);
     


            }
            else
            {
              
                NetworkCardManager.Instance.DeckPickup(BottomCardManager.cardID);
                
            }
        }
        StartCoroutine(DelayMenuEnableButtons());
    }
    public void DropDeck(GameObject button)
    {
        MenuManager.Instance.TempDisableButtons();
        if (HeldCard)
        {
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            StickyCardManager TopCardManager = GetStickyCardManager(HeldManager.GetTopCard());
            // drop
            if (HighlightedCard)
            {
                StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);

                if (HighlightedManager.Spread)
                {
                    NetworkCardManager.Instance.DropDeckOnTopOf(HeldManager.cardID, TopCardManager.cardID, HighlightedManager.cardID);
                }
                else
                {
                    NetworkCardManager.Instance.DropDeckOnTopOf(HeldManager.cardID, TopCardManager.cardID, GetStickyCardManager(HighlightedManager.GetTopCard()).cardID);
                }
            }
            else
            {
                NetworkCardManager.Instance.DropDeckOntoTable(HeldManager.cardID);
            }

        }
        StartCoroutine(DelayMenuEnableButtons());
    }
    public void FlipCard(GameObject button)
    {
        MenuManager.Instance.TempDisableButtons();
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
        StartCoroutine(DelayMenuEnableButtons());
    }
    public void FlipDeck(GameObject button)
    {

        MenuManager.Instance.TempDisableButtons();
        GameObject bottomCard;
        if (HeldCard && GetStickyCardManager(HeldCard).CountAbove() >= 2)
        {
            bottomCard = HeldCard;
            StickyCardManager bottomCardManager = GetStickyCardManager(bottomCard);

            bool faceUp = !bottomCardManager.FaceUp;
            bool spread = bottomCardManager.Spread;

            if (spread)
            {
                NetworkCardManager.Instance.FlipDeckOneByOne(bottomCardManager.cardID);
            }
            else
            {
                NetworkCardManager.Instance.FlipWholeDeck(bottomCardManager.cardID, pickup: true);
            }
        }
        else if (HighlightedCard && GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard()).CountAbove() >= 2)
        {
            StickyCardManager bottomCardManager = GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard());
            
            bool spread = bottomCardManager.Spread;
           
            if (spread)
            {
                NetworkCardManager.Instance.FlipDeckOneByOne(bottomCardManager.cardID);
            }
            else
            {
                NetworkCardManager.Instance.FlipWholeDeck(bottomCardManager.cardID);
            }
        }
        


        StartCoroutine(DelayMenuEnableButtons());
    }
    public void SplitDeck(GameObject button)
    {


        if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            DeckSplitManager.Instance.SetBottomCard(HighlightedManager.GetBottomCard());

            GameManager.Instance.UpdateGameStateTo_SelectingNumber_Split();
        }

 
        
    }

    public void ShuffleDeck(GameObject button)
    {
        MenuManager.Instance.TempDisableButtons();
        StickyCardManager DeckManager = null;
        if (HighlightedCard && GetStickyCardManager(HighlightedCard).CountAbove() >= 2)
        {
            DeckManager = GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard());
            NetworkCardManager.Instance.ShuffleDeck(DeckManager.cardID);
        }
        else if (HeldCard && GetStickyCardManager(HeldCard).CountAbove() >= 2)
        {

            DeckManager = GetStickyCardManager(GetStickyCardManager(HeldCard).GetBottomCard());
            NetworkCardManager.Instance.ShuffleDeck(DeckManager.cardID, pickup: true);
        }
        StartCoroutine(DelayMenuEnableButtons());

    }
    public void SortDeck(GameObject button)
    {
        MenuManager.Instance.TempDisableButtons();
        StickyCardManager DeckManager = null;
        if (HighlightedCard && GetStickyCardManager(HighlightedCard).CountAbove() >= 2)
        {
            DeckManager = GetStickyCardManager(GetStickyCardManager(HighlightedCard).GetBottomCard());
            NetworkCardManager.Instance.SortDeck(DeckManager.cardID);
        }
        else if (HeldCard && GetStickyCardManager(HeldCard).CountAbove() >= 2)
        {

            DeckManager = GetStickyCardManager(GetStickyCardManager(HeldCard).GetBottomCard());
            NetworkCardManager.Instance.SortDeck(DeckManager.cardID, pickup: true);
        }

        StartCoroutine(DelayMenuEnableButtons());
    }
    public void SpreadDeck(GameObject button)
    {

        MenuManager.Instance.TempDisableButtons();
 
        if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            StickyCardManager BottomCardManager = GetStickyCardManager(HighlightedManager.GetBottomCard());
            BottomCardManager.Spread = !BottomCardManager.Spread;
 
        }

        StartCoroutine(DelayMenuEnableButtons());
    }



    public void HideCard(GameObject button)
    {
        MenuManager.Instance.TempDisableButtons();
        if (HeldCard)
        {
            string value = PlayerManager.Instance.uid;
            StickyCardManager HeldManager = GetStickyCardManager(HeldCard);
            if (HeldManager.HiddenBy == PlayerManager.Instance.uid)
            {
                value = "";
            }


            NetworkCardManager.Instance.SetHideDeckTo(HeldManager.cardID, value);

        }
        else if (HighlightedCard)
        {
            StickyCardManager HighlightedManager = GetStickyCardManager(HighlightedCard);
            string value = PlayerManager.Instance.uid;
            StickyCardManager cardManager = GetStickyCardManager(HighlightedManager.GetBottomCard());
            if (cardManager.HiddenBy == PlayerManager.Instance.uid)
            {
                value = "";
            }
            NetworkCardManager.Instance.SetHideDeckTo(cardManager.cardID, value);
        }

        StartCoroutine(DelayMenuEnableButtons());
    }


    }
