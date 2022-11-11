using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCardData
{
    public bool faceUp { get; private set; }
    public bool spread { get; private set; }
    public Vector3 position { get; private set; }
    public Vector3 rotation { get; private set; }
    public string cardBelowID { get; private set; }
    public string cardAboveID { get; private set; }
    public string hiddenBy { get; private set; }

    public NetworkedCardData(bool faceup, bool spread, Vector3 pos, Vector3 rot, string below, string above)
    {
        this.faceUp = faceup;
        this.spread = spread;
        this.position = pos;
        this.rotation = rot;
        this.cardBelowID = below;
        this.cardAboveID = above;
        this.hiddenBy = "";
    }

    public void fromNetworkString(string networkString)
    {


        string[] parts = networkString.Split("/");
        if (parts[0] != "")
        {
           faceUp = bool.Parse(parts[0]);
        }

        spread = bool.Parse(parts[1]);
        position = new Vector3(float.Parse(parts[2]),
                                    float.Parse(parts[3]),
                                    float.Parse(parts[4]));
        rotation = new Vector3(float.Parse(parts[5]),
                               float.Parse(parts[6]),
                               float.Parse(parts[7]));
        cardBelowID = parts[8];
        cardAboveID = parts[9];
        hiddenBy = parts[10];

        if (cardBelowID == "" && cardAboveID == "")
        {
            // 1 card cannpt be spread
            spread = false;
        }
        else if (cardAboveID != "")
        {
            //

            NetworkCardManager.Instance.networkedCards[cardAboveID].UpdateSpreadUpwards(this.spread);
        }
        if (cardBelowID != "")
        {
            spread = NetworkCardManager.Instance.networkedCards[cardBelowID].spread;
        }
    }

    private void UpdateSpreadUpwards(bool below_spread)
    {
        this.spread = below_spread;
        if (cardAboveID != "")
        {
            NetworkCardManager.Instance.networkedCards[cardAboveID].UpdateSpreadUpwards(below_spread);
        }
    }

    public string ToNetworkString()
    {
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
                              this.faceUp,
                              this.spread,
                              this.position.x,
                              this.position.y,
                              this.position.z,
                              this.rotation.x,
                              this.rotation.y,
                              this.rotation.z,
                              this.cardBelowID,
                              this.cardAboveID,
                              this.hiddenBy
                              );

    }
    public string FaceUpChangeString(bool value)
    {
        return ChangeString(value,
                            this.spread,
                            this.position,
                            this.rotation,
                            this.cardBelowID,
                            this.cardAboveID,
                            this.hiddenBy
                            );

    }
    public string SpreadChangeString(bool value)
    {
        return ChangeString(this.faceUp,
                            value,
                            this.position,
                            this.rotation,
                            this.cardBelowID,
                            this.cardAboveID,
                            this.hiddenBy
                            );

    }
    public string PositionChangeString(Vector3 value)
    {
        return ChangeString(this.faceUp,
                            this.spread,
                            value,
                            this.rotation,
                            this.cardBelowID,
                            this.cardAboveID,
                            this.hiddenBy
                            );

    }
    public string RotationChangeString(Vector3 value)
    {
        return ChangeString(this.faceUp,
                              this.spread,
                              this.position,
                              value,
                              this.cardBelowID,
                              this.cardAboveID,
                              this.hiddenBy
                              );

    }
    public string BelowChangeString(string value)
    {
        return ChangeString(this.faceUp,
                              this.spread,
                              this.position,
                              this.rotation,
                              value,
                              this.cardAboveID,
                              this.hiddenBy
                              );

    }
    public string AboveChangeString(string value)
    {
        return ChangeString(  this.faceUp,
                              this.spread,
                              this.position,
                              this.rotation,
                              this.cardBelowID,
                              value,
                              this.hiddenBy
                              );

    }
    public string HiddenChangeString(string value)
    {
        return ChangeString(this.faceUp,
                            this.spread,
                            this.position,
                            this.rotation,
                            this.cardBelowID,
                            this.cardAboveID,
                            value);

    }
    internal string PositionRotationChangeString(Vector3 position, Vector3 rotation)
    {
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
                              "",
                              spread,
                              position.x,
                              position.y,
                              position.z,
                              rotation.x,
                              rotation.y,
                              rotation.z,
                              cardBelowID,
                              cardAboveID,
                              hiddenBy
                              );
    }
    internal string AboveBelowChangeString(string above, string below)
    {
        return ChangeString(this.faceUp,
                    this.spread,
                    this.position,
                    this.rotation,
                    below,
                    above,
                    this.hiddenBy);
    }
    public string ChangeString(bool newFaceUp,
                               bool newSpread,
                               Vector3 newPos,
                               Vector3 newRot,
                               string newBelow,
                               string newAbove,
                               string newHidden)
    {
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
                              newFaceUp,
                              newSpread,
                              newPos.x,
                              newPos.y,
                              newPos.z,
                              newRot.x,
                              newRot.y,
                              newRot.z,
                              newBelow,
                              newAbove,
                              newHidden
                              );

    }


}

public class NetworkCardManager : MonoBehaviour
{


    public Dictionary<String, NetworkedCardData> networkedCards;

    public Dictionary<String, GameObject> cardGameObjects;

    public Dictionary<String, String> NetworkedCardHighlightedBy;
    public Dictionary<String, String> NetworkedCardHeldBy;

    public Queue<Dictionary<String, String>> networkedCardDataChanges = new Queue<Dictionary<String, String>>();
    public Queue<Dictionary<String, String>> networkedCardHightlightChanges = new Queue<Dictionary<String, String>>();




    public GameObject HighlightedCard
    {
        get
        {
            GameObject ret = null;
            if (NetworkedCardHighlightedBy.ContainsKey(PlayerManager.Instance.uid)){
                string cardID = NetworkedCardHighlightedBy[PlayerManager.Instance.uid];
                if (cardGameObjects.ContainsKey(cardID))
                {
                    ret = cardGameObjects[cardID];
                }
            }
            return ret;

        }
    }
    public GameObject HeldCard
    {
        get
        {

            GameObject ret = null;
            if (NetworkedCardHeldBy.ContainsKey(PlayerManager.Instance.uid))
            {
                string cardID = NetworkedCardHeldBy[PlayerManager.Instance.uid];
                if (cardGameObjects.ContainsKey(cardID))
                {
                    ret = cardGameObjects[cardID];
                }

            }
            return ret;

        }
    }

    public GameObject j1;
    public GameObject j2;
    public GameObject d1;
    public GameObject d2;
    public GameObject d3;
    public GameObject d4;
    public GameObject d5;
    public GameObject d6;
    public GameObject d7;
    public GameObject d8;
    public GameObject d9;
    public GameObject d10;
    public GameObject d11;
    public GameObject d12;
    public GameObject d13;
    public GameObject s1;
    public GameObject s2;
    public GameObject s3;
    public GameObject s4;
    public GameObject s5;
    public GameObject s6;
    public GameObject s7;
    public GameObject s8;
    public GameObject s9;
    public GameObject s10;
    public GameObject s11;


    public GameObject s12;
    public GameObject s13;
    public GameObject c1;
    public GameObject c2;
    public GameObject c3;




    public GameObject c4;
    public GameObject c5;
    public GameObject c6;
    public GameObject c7;
    public GameObject c8;
    public GameObject c9;
    public GameObject c10;
    public GameObject c11;
    public GameObject c12;
    public GameObject c13;
    public GameObject h1;
    public GameObject h2;
    public GameObject h3;
    public GameObject h4;
    public GameObject h5;
    public GameObject h6;
    public GameObject h7;
    public GameObject h8;
    public GameObject h9;
    public GameObject h10;
    public GameObject h11;
    public GameObject h12;
    public GameObject h13;


    public static NetworkCardManager Instance;
    private string _CurrentHeldID;

    private void Awake()
    {
        Instance = this;
    }

    public void printy()
    {
        foreach (string key in this.networkedCards.Keys)
        {
            Debug.Log(key+": a="+this.networkedCards[key].cardAboveID+" b="+this.networkedCards[key].cardBelowID );
        }
        Debug.Log("**************************************************");
    }

        // Start is called before the first frame update
    void Start()
    {

        NetworkedCardHighlightedBy = new Dictionary<String, String>();
        NetworkedCardHeldBy = new Dictionary<String, String>();


        networkedCards = new Dictionary<String, NetworkedCardData>();

        networkedCards.Add("j1", new NetworkedCardData(faceup: true, spread: true, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "j2", above: ""));
        networkedCards.Add("j2", new NetworkedCardData(faceup: true, spread: true, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d1", above: "j1"));
        networkedCards.Add("d1", new NetworkedCardData(faceup: true, spread: true, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h1", above: "j2"));
        networkedCards.Add("h1", new NetworkedCardData(faceup: true, spread: true, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c1", above: "d1"));
        networkedCards.Add("c1", new NetworkedCardData(faceup: true, spread: true, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s1", above: "h1"));
        networkedCards.Add("s1", new NetworkedCardData(faceup: true, spread: true, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "", above: "c1"));

        cardGameObjects = new Dictionary<String, GameObject>();
        cardGameObjects.Add("j1", j1);
        cardGameObjects.Add("j2", j2);
        cardGameObjects.Add("d1", d1);
        cardGameObjects.Add("d2", d2);
        cardGameObjects.Add("d3", d3);
        cardGameObjects.Add("d4", d4);
        cardGameObjects.Add("d5", d5);
        cardGameObjects.Add("d6", d6);
        cardGameObjects.Add("d7", d7);
        cardGameObjects.Add("d8", d8);
        cardGameObjects.Add("d9", d9);
        cardGameObjects.Add("d10", d10);
        cardGameObjects.Add("d11", d11);
        cardGameObjects.Add("d12", d12);
        cardGameObjects.Add("d13", d13);
        cardGameObjects.Add("s1", s1);
        cardGameObjects.Add("s2", s2);
        cardGameObjects.Add("s3", s3);
        cardGameObjects.Add("s4", s4);
        cardGameObjects.Add("s5", s5);
        cardGameObjects.Add("s6", s6);
        cardGameObjects.Add("s7", s7);
        cardGameObjects.Add("s8", s8);
        cardGameObjects.Add("s9", s9);
        cardGameObjects.Add("s10", s10);
        cardGameObjects.Add("s11", s11);
        cardGameObjects.Add("s12", s12);
        cardGameObjects.Add("s13", s13);
        cardGameObjects.Add("c1", c1);
        cardGameObjects.Add("c2", c2);
        cardGameObjects.Add("c3", c3);
        cardGameObjects.Add("c4", c4);
        cardGameObjects.Add("c5", c5);
        cardGameObjects.Add("c6", c6);
        cardGameObjects.Add("c7", c7);
        cardGameObjects.Add("c8", c8);
        cardGameObjects.Add("c9", c9);
        cardGameObjects.Add("c10", c10);
        cardGameObjects.Add("c11", c11);
        cardGameObjects.Add("c12", c12);
        cardGameObjects.Add("c13", c13);
        cardGameObjects.Add("h1", h1);
        cardGameObjects.Add("h2", h2);
        cardGameObjects.Add("h3", h3);
        cardGameObjects.Add("h4", h4);
        cardGameObjects.Add("h5", h5);
        cardGameObjects.Add("h6", h6);
        cardGameObjects.Add("h7", h7);
        cardGameObjects.Add("h8", h8);
        cardGameObjects.Add("h9", h9);
        cardGameObjects.Add("h10", h10);
        cardGameObjects.Add("h11", h11);
        cardGameObjects.Add("h12", h12);
        cardGameObjects.Add("h13", h13);


        InvokeRepeating("printy", 1f, 2.0f);
    }



    internal StickyCardManager GetCardManager(string cardID)
    {
        return cardGameObjects[cardID].GetComponent<StickyCardManager>();
    }


    internal GameObject GetBelow(string cardID)
    {
        string cardBelowID = this.networkedCards[cardID].cardBelowID;
        if (cardBelowID!= "")
        {
            return this.cardGameObjects[cardBelowID];
        }
        else
        {
            return null;
        }
        
    }
    internal GameObject GetAbove(string cardID)
    {
        string cardAboveID = this.networkedCards[cardID].cardAboveID;
        if (cardAboveID != "")
        {
            return this.cardGameObjects[cardAboveID];
        }
        else
        {
            return null;
        }
    }
    internal bool GetSpread(string cardID)
    {
        return this.networkedCards[cardID].spread;
    }
    internal bool GetFaceUp(string cardID)
    {
        return this.networkedCards[cardID].faceUp;
    }
    internal Vector3 GetPostion(string cardID)
    {
        return this.networkedCards[cardID].position;
    }
    internal Vector3 GetRotation(string cardID)
    {
        return this.networkedCards[cardID].rotation;
    }

    internal bool IsHighlighted(string cardID)
    {
        return (this.NetworkedCardHighlightedBy.ContainsKey(PlayerManager.Instance.uid)
                && this.NetworkedCardHighlightedBy[PlayerManager.Instance.uid] == cardID);
    }

    internal bool IsHeld(string cardID)
    {
        return (this.NetworkedCardHeldBy.ContainsKey(PlayerManager.Instance.uid)
                && this.NetworkedCardHeldBy[PlayerManager.Instance.uid] == cardID);
    }

    internal string HeldBy(string cardID)
    {
        foreach (string player in this.NetworkedCardHeldBy.Keys)
        {
            if (this.NetworkedCardHeldBy[player] == cardID)
            {
                return player;
            }
        }
        return "";
    }

    internal bool IsHidden(string cardID)
    {
        return this.networkedCards[cardID].hiddenBy == PlayerManager.Instance.uid;
    }

    internal void SetHighlighted(string cardID, bool value)
    {
        Debug.Log("SetHighlighted TO " + value.ToString());
        NetworkedCardData data = this.networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("user", PlayerManager.Instance.uid);
        changeData.Add("card", cardID);
        if (value == true)
        {
            changeData.Add("type", PubNubManager.PLAYER_HIGHLIGHT_START);
        }
        else
        {
            changeData.Add("type", PubNubManager.PLAYER_HIGHLIGHT_STOP);
        }
        PubNubManager.Instance.StoreHighlightMessage(changeData);
    }





    internal void SetHeld(string cardID, bool value)
    {
        Debug.Log("SetHeld TO " + value.ToString());
        NetworkedCardData data = this.networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("user", PlayerManager.Instance.uid);
        changeData.Add("card", cardID);
        if (value == true)
        {
            changeData.Add("type", PubNubManager.PLAYER_HELD_START);
            this._CurrentHeldID = cardID;
            InvokeRepeating("UpdateHeldPosition", 1f, 0.2f);
        }
        else
        {
            changeData.Add("type", PubNubManager.PLAYER_HELD_STOP);
            this._CurrentHeldID = "";
            CancelInvoke();
            InvokeRepeating("printy", 1f, 2.0f);
        }
        PubNubManager.Instance.BroadcastMessage(changeData);


    }
    internal void UpdateHeldPosition()
    {
        Debug.Log("UpdateHeldPosition");
        Vector3 position = PlayerManager.Instance.cursor.transform.localPosition;
        Vector3 rotation = new Vector3(0.0f,
                                       PlayerManager.Instance.cursor.transform.localEulerAngles.y,
                                                          0.0f);


       
        NetworkedCardData data = this.networkedCards[this._CurrentHeldID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        changeData.Add(this._CurrentHeldID, data.PositionRotationChangeString(position, rotation));
        PubNubManager.Instance.BroadcastMessage(changeData);
    }


    internal bool IsAvailableToHighlight(string cardID)
    {
        return this.IsHighlighted(cardID) == true || (
                  this.NetworkedCardHighlightedBy.ContainsValue(cardID) == false &&
                  this.NetworkedCardHeldBy.ContainsValue(cardID) == false);
               
    }

    internal void SetFaceUp(string cardID, bool value)
    {
      
        Debug.Log("SetFaceUp TO " + value.ToString());
        NetworkedCardData cardData = this.networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        changeData.Add(cardID, cardData.FaceUpChangeString(value));
        PubNubManager.Instance.BroadcastMessage(changeData);
      
    }
    internal void SetSpread(string cardID, bool value)
    {
        Debug.Log("SetSpread TO " + value.ToString());
        NetworkedCardData cardData = this.networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        changeData.Add(cardID, cardData.SpreadChangeString(value));
        PubNubManager.Instance.BroadcastMessage(changeData);
    }


    internal void MagnetSingle(string cardID)
    {
        //throw new NotImplementedException();
    }
    internal void MagnetDeck(string cardID)
    {
        //throw new NotImplementedException();
    }
    internal void SinglePickUp(string cardID)
    {
        //this.SetHeld(cardID, true);
        StickyCardManager card = cardGameObjects[cardID].GetComponent<StickyCardManager>();
        NetworkedCardData cardData = networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        if (cardData.cardBelowID != "" && cardData.cardAboveID!= "")
        {
            //card is in the middle of a deck
            NetworkedCardData cardBelowData = networkedCards[cardData.cardBelowID];
            NetworkedCardData cardAboveData = networkedCards[cardData.cardAboveID];

            changeData.Add(cardData.cardBelowID, cardBelowData.AboveChangeString(cardData.cardAboveID));
            changeData.Add(cardID, cardData.AboveBelowChangeString("",""));
            changeData.Add(cardData.cardAboveID, cardAboveData.BelowChangeString(cardData.cardBelowID));
        }
        else if (cardData.cardBelowID != "")
        {
            //card is bottom of deck
            NetworkedCardData cardBelowData = networkedCards[cardData.cardBelowID];

            changeData.Add(cardData.cardBelowID, cardBelowData.AboveChangeString(""));
            changeData.Add(cardID, cardData.AboveBelowChangeString("", ""));
        }
        else if (cardData.cardAboveID != "")
        {
            //card is top of deck
            NetworkedCardData cardAboveData = networkedCards[cardData.cardAboveID];

            changeData.Add(cardID, cardData.AboveBelowChangeString("", ""));
            changeData.Add(cardData.cardAboveID, cardAboveData.BelowChangeString(""));

        }
        else
        {
            //card is loose
            changeData.Add(cardID, cardData.AboveBelowChangeString("", ""));
        }
        PubNubManager.Instance.BroadcastMessage(changeData);

        this.SetHeld(cardID, true);

    }

    internal void DropOnTopOf(string thisCardID, string thatCardID)
    {
        //this.SetHeld(cardID, true);
        //StickyCardManager thisCard = cardGameObjects[thisCardID].GetComponent<StickyCardManager>();
        NetworkedCardData thisCardData = networkedCards[thisCardID];
        //StickyCardManager thatCard = cardGameObjects[thatCardID].GetComponent<StickyCardManager>();
        NetworkedCardData thatCardData = networkedCards[thatCardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        this.SetHeld(thisCardID, false);
        if (thatCardData.cardAboveID != "" )
        {
            //card is in the middle of a deck
            string aboveThatCardID = thatCardData.cardAboveID;
            NetworkedCardData aboveThatCardData = networkedCards[aboveThatCardID];
            changeData.Add(thatCardID, thatCardData.AboveChangeString(thisCardID));
            changeData.Add(thisCardID, thisCardData.AboveBelowChangeString(aboveThatCardID, thatCardID));
            changeData.Add(aboveThatCardID, aboveThatCardData.BelowChangeString(thisCardID));
        }
        else
        {
            //drop onto the top of a deck
            changeData.Add(thatCardID, thatCardData.AboveChangeString(thisCardID));
            changeData.Add(thisCardID, thisCardData.BelowChangeString(thatCardID));
        }
        PubNubManager.Instance.BroadcastMessage(changeData);
    }

    internal void DropOntoTable(string cardID)
    {
        this.SetHeld(cardID, false);
    }


}
