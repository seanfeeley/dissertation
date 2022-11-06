using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedCardData
{
    public bool faceUp;
    public bool spread;
    public Vector3 position;
    public Vector3 rotation;
    public string cardBelowID;
    public string cardAboveID;
    public string heldBy;
    public string highlightedBy;
    public string hiddenBy;

    public NetworkedCardData(bool faceup, bool spread, Vector3 pos, Vector3 rot, string below, string above)
    {
        this.faceUp = faceup;
        this.spread = spread;
        this.position = pos;
        this.rotation = rot;
        this.cardBelowID = below;
        this.cardAboveID = above;
        this.heldBy = null;
        this.highlightedBy = null;
        this.hiddenBy = null;
    }

    public void fromNetworkString(string networkString)
    {
        string[] parts = networkString.Split("/");
        this.faceUp = bool.Parse(parts[0]);
        this.spread = bool.Parse(parts[1]);
        this.position.x = float.Parse(parts[2]);
        this.position.y = float.Parse(parts[3]);
        this.position.z = float.Parse(parts[4]);
        this.rotation.x = float.Parse(parts[5]);
        this.rotation.y = float.Parse(parts[6]);
        this.rotation.z = float.Parse(parts[7]);
        this.cardBelowID = parts[8]!=""? parts[8]: null;
        this.cardAboveID = parts[9] != "" ? parts[9] : null;
        this.heldBy = parts[10] != "" ? parts[10] : null;
        this.highlightedBy = parts[11] != "" ? parts[11] : null;
        this.hiddenBy = parts[12] != "" ? parts[12] : null;
    }

    public string ToNetworkString()
    {
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}/{11}/{12}",
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
                              this.heldBy,
                              this.highlightedBy,
                              this.hiddenBy
                              );

    }


}

public class CardDeckManager : MonoBehaviour
{


    public Dictionary<String, NetworkedCardData> networkedCards;

    public Dictionary<String, GameObject> cardGameObjects;

    public Queue<Dictionary<String, NetworkedCardData>> networkedCardChanges = new Queue<Dictionary<String, NetworkedCardData>>();


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


    public static CardDeckManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

        networkedCards = new Dictionary<String, NetworkedCardData>();
        networkedCards.Add("j1", new NetworkedCardData(faceup: false, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "j2", above: null));
        networkedCards.Add("j2", new NetworkedCardData(faceup: false, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d1", above: "j1"));
        networkedCards.Add("d1", new NetworkedCardData(faceup: false, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h1", above: "j2"));
        networkedCards.Add("h1", new NetworkedCardData(faceup: false, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c1", above: "d1"));
        networkedCards.Add("c1", new NetworkedCardData(faceup: false, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s1", above: "h1"));
        networkedCards.Add("s1", new NetworkedCardData(faceup: false, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: null, above: "c1"));

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


    }

    internal StickyCardManager GetCardManager(string cardID)
    {
        return cardGameObjects[cardID].GetComponent<StickyCardManager>();
    }


    internal GameObject GetBelow(string cardID)
    {
        string cardBelowID = this.networkedCards[cardID].cardBelowID;
        if (cardBelowID!=null)
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
        if (cardAboveID != null)
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

    internal bool IsHeld(string cardID)
    {
        return this.networkedCards[cardID].heldBy == PlayerManager.Instance.uid;
    }

    internal bool IsHighlighted(string cardID)
    {
        return this.networkedCards[cardID].highlightedBy == PlayerManager.Instance.uid;
    }

    internal bool IsHidden(string cardID)
    {
        return this.networkedCards[cardID].hiddenBy == PlayerManager.Instance.uid;
    }

    internal void SetHighlighted(string cardID, bool value)
    {
        NetworkedCardData data = this.networkedCards[cardID];
        if (value == true)
        {
            data.highlightedBy = PlayerManager.Instance.uid;
        }
        else
        {
            data.highlightedBy = null;
        }
        Dictionary<string, NetworkedCardData> changeData = new Dictionary<string, NetworkedCardData>();
        changeData.Add(cardID, data);
        networkedCardChanges.Enqueue(changeData);
    }

    internal bool IsAvailableToHighlight(string cardID)
    {
        return this.IsHighlighted(cardID) == true ||
                this.networkedCards[cardID].highlightedBy == null ||
                this.networkedCards[cardID].highlightedBy == "";
    }

    internal void SetFaceUp(string cardID, bool value)
    {
        NetworkedCardData data = this.networkedCards[cardID];
        data.faceUp = value;
        Dictionary<string, NetworkedCardData> changeData = new Dictionary<string, NetworkedCardData>();
        changeData.Add(cardID, data);
        networkedCardChanges.Enqueue(changeData);
    }


}
