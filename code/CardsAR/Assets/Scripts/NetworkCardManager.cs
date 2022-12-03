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
        if (parts[0] != "null")
        {
           faceUp = bool.Parse(parts[0]);
        }

        
        
        bool spread_changed = false;
        if (parts[1] != "null")
        {
            bool new_spread = bool.Parse(parts[1]);
            if (new_spread != spread)
            {
                spread = new_spread;
                spread_changed = true;
            }
        }
        if (spread_changed == false)
        {

            if (cardBelowID == "" && cardAboveID == "")
            {
                // 1 card cannpt be spread
                spread = false;
            }
            else if (cardAboveID != "")
            {
                spread = NetworkCardManager.Instance.networkedCards[cardAboveID].spread;
            }
            else if (cardBelowID != "")
            {
                spread = NetworkCardManager.Instance.networkedCards[cardBelowID].spread;
            }

        }

        if (parts[2] != "null")
        {
            position = new Vector3(float.Parse(parts[2]),
                           float.Parse(parts[3]),
                           float.Parse(parts[4]));
        }
        if (parts[5] != "null")
        {
            rotation = new Vector3(float.Parse(parts[5]),
                               float.Parse(parts[6]),
                               float.Parse(parts[7]));
        }
        if (parts[8] != "null")
        {
            cardBelowID = parts[8];
        }
        if (parts[9] != "null")
        {
            cardAboveID = parts[9];
        }
        if (parts[10] != "null")
        {
            hiddenBy = parts[10];
        }

    }

    //private void UpdateSpreadUpwards(bool below_spread)
    //{
    //    this.spread = below_spread;
    //    if (cardAboveID != "")
    //    {
    //        NetworkCardManager.Instance.networkedCards[cardAboveID].UpdateSpreadUpwards(below_spread);
    //    }
    //}

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
    public string ToDynamicNetworkString(string new_faceUp = null,
                                         string new_spread = null,
                                         string new_posX = null,
                                         string new_posY = null,
                                         string new_posZ = null,
                                         string new_rotX = null,
                                         string new_rotY = null,
                                         string new_rotZ = null,
                                         string new_cardBelowID = null,
                                         string new_cardAboveID = null,
                                         string new_hiddenBy = null)
    {

  
        if (new_faceUp == null) { new_faceUp = faceUp.ToString(); }
        if (new_spread == null) { new_spread = spread.ToString(); }
        if (new_posX == null) { new_posX = position.x.ToString(); }
        if (new_posY == null) { new_posY = position.y.ToString(); }
        if (new_posZ == null) { new_posZ = position.z.ToString(); }
        if (new_rotX == null) { new_rotX = rotation.x.ToString(); }
        if (new_rotY == null) { new_rotY = rotation.y.ToString(); }
        if (new_rotZ == null) { new_rotZ = rotation.z.ToString(); }
        if (new_cardBelowID == null) { new_cardBelowID = cardBelowID.ToString(); }
        if (new_cardAboveID == null) { new_cardAboveID = cardAboveID.ToString(); }
        if (new_hiddenBy == null) { new_hiddenBy = hiddenBy.ToString(); }
        return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
                                new_faceUp,
                                new_spread,
                                new_posX,
                                new_posY,
                                new_posZ,
                                new_rotX,
                                new_rotY,
                                new_rotZ,
                                new_cardBelowID,
                                new_cardAboveID,
                                new_hiddenBy
                                );

        return "";

    }
    //public string FaceUpChangeString(bool value)
    //{
    //    return ChangeString(value,
    //                        this.spread,
    //                        this.position,
    //                        this.rotation,
    //                        this.cardBelowID,
    //                        this.cardAboveID,
    //                        this.hiddenBy
    //                        );

    //}
    //public string SpreadChangeString(bool value)
    //{
    //    return ChangeString(this.faceUp,
    //                        value,
    //                        this.position,
    //                        this.rotation,
    //                        this.cardBelowID,
    //                        this.cardAboveID,
    //                        this.hiddenBy
    //                        );

    //}
    //public string PositionChangeString(Vector3 value)
    //{
    //    return ChangeString(this.faceUp,
    //                        this.spread,
    //                        value,
    //                        this.rotation,
    //                        this.cardBelowID,
    //                        this.cardAboveID,
    //                        this.hiddenBy
    //                        );

    //}
    //public string RotationChangeString(Vector3 value)
    //{
    //    return ChangeString(this.faceUp,
    //                          this.spread,
    //                          this.position,
    //                          value,
    //                          this.cardBelowID,
    //                          this.cardAboveID,
    //                          this.hiddenBy
    //                          );

    //}
    //public string BelowChangeString(string value, Vector3 pos, Vector3 rot,string new_spread = "")
    //{
    //    return ChangeString(this.faceUp,
    //                          new_spread != ""? bool.Parse(new_spread): this.spread,
    //                          pos,
    //                          rot,
    //                          value,
    //                          this.cardAboveID,
    //                          this.hiddenBy
    //                          );

    //}
    //public string AboveChangeString(string value, string new_spread = "")
    //{
    //    return ChangeString(  this.faceUp,
    //                          new_spread != "" ? bool.Parse(new_spread) : this.spread,
    //                          this.position,
    //                          this.rotation,
    //                          this.cardBelowID,
    //                          value,
    //                          this.hiddenBy
    //                          );

    //}
    //public string HiddenChangeString(string value)
    //{
        

    //    return ChangeString(this.faceUp,
    //                        this.spread,
    //                        this.position,
    //                        this.rotation,
    //                        this.cardBelowID,
    //                        this.cardAboveID,
    //                        value);

    //}
    //internal string PositionRotationChangeOnlyString(Vector3 pos, Vector3 rot, string new_spread = "null")
    //{
    //    return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
    //                          "null",
    //                          new_spread,
    //                          pos.x,
    //                          pos.y,
    //                          pos.z,
    //                          rot.x,
    //                          rot.y,
    //                          rot.z,
    //                          "null",
    //                          "null",
    //                          "null"
    //                          );
    //}
    //internal string HeldCardPositionRotationChangeString(Vector3 position, Vector3 rotation)
    //{
    //    return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
    //                          "null",
    //                          "null",
    //                          position.x,
    //                          position.y,
    //                          position.z,
    //                          rotation.x,
    //                          rotation.y,
    //                          rotation.z,
    //                          "null",
    //                          "null",
    //                          "null"
    //                          );
    //}
    //internal string AboveBelowChangeString(string above, string below, Vector3 position, Vector3 rotation, string new_spread = "")
    //{
    //    return ChangeString(this.faceUp,
    //                new_spread != "" ? bool.Parse(new_spread) : this.spread,
    //                this.position,
    //                this.rotation,
    //                below,
    //                above,
    //                this.hiddenBy);
    //}
    //internal string AboveBelowFaceUpChangeString(string above, string below, bool deckFaceUp, Vector3 pos, Vector3 rot)
    //{
    //    return ChangeString(deckFaceUp,
    //        this.spread,
    //        pos,
    //        rot,
    //        below,
    //        above,
    //        this.hiddenBy);
    //}

    //public string ChangeString(bool newFaceUp,
    //                           bool newSpread,
    //                           Vector3 newPos,
    //                           Vector3 newRot,
    //                           string newBelow,
    //                           string newAbove,
    //                           string newHidden)
    //{
    //    return string.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/{9}/{10}",
    //                          newFaceUp,
    //                          newSpread,
    //                          newPos.x,
    //                          newPos.y,
    //                          newPos.z,
    //                          newRot.x,
    //                          newRot.y,
    //                          newRot.z,
    //                          newBelow,
    //                          newAbove,
    //                          newHidden
    //                          );

    //}

    //internal string AboveBelowPosRotChangeString(string above, string below, Vector3 card0Pos, Vector3 card0Rot)
    //{
    //    return ChangeString(this.faceUp,
    //                        this.spread,
    //                        card0Pos,
    //                        card0Rot,
    //                        below,
    //                        above,
    //                        this.hiddenBy);
    //}

    //internal string AboveBelowFaceUpPosRotChangeString(string above, string below, bool deckFaceUp, Vector3 card0Pos, Vector3 card0Rot)
    //{
    //    return ChangeString(deckFaceUp,
    //                        this.spread,
    //                        card0Pos,
    //                        card0Rot,
    //                        below,
    //                        above,
    //                        this.hiddenBy);
    //}
}

public class NetworkCardManager : MonoBehaviour
{


    public Dictionary<String, NetworkedCardData> networkedCards;

    public Dictionary<String, GameObject> cardGameObjects;

    public Dictionary<String, String> NetworkedCardHighlightedBy;
    public Dictionary<String, String> NetworkedCardHeldBy;

    public Queue<Dictionary<String, String>> networkedCardDataChanges = new Queue<Dictionary<String, String>>();
    public Queue<Dictionary<String, String>> networkedCardHightlightChanges = new Queue<Dictionary<String, String>>();

    private static System.Random rng = new System.Random();


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
        //foreach (string key in this.networkedCards.Keys)
        //{
        //    Debug.Log(key+": a="+this.networkedCards[key].cardAboveID+" b="+this.networkedCards[key].cardBelowID );
        //}
        //Debug.Log("**************************************************");
    }

        // Start is called before the first frame update
    void Start()
    {

        NetworkedCardHighlightedBy = new Dictionary<String, String>();
        NetworkedCardHeldBy = new Dictionary<String, String>();


        networkedCards = new Dictionary<String, NetworkedCardData>();

        networkedCards.Add("j01", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "j02", above: ""));
        networkedCards.Add("j02", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d01", above: "j01"));
        networkedCards.Add("d01", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d02", above: "j02"));
        networkedCards.Add("d02", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d03", above: "d01"));
        networkedCards.Add("d03", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d04", above: "d02"));
        networkedCards.Add("d04", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d05", above: "d03"));
        networkedCards.Add("d05", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d06", above: "d04"));
        networkedCards.Add("d06", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d07", above: "d05"));
        networkedCards.Add("d07", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d08", above: "d06"));
        networkedCards.Add("d08", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d09", above: "d07"));
        networkedCards.Add("d09", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d10", above: "d08"));
        networkedCards.Add("d10", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d11", above: "d09"));
        networkedCards.Add("d11", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d12", above: "d10"));
        networkedCards.Add("d12", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "d13", above: "d11"));
        networkedCards.Add("d13", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h01", above: "d12"));
        networkedCards.Add("h01", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h02", above: "d13"));
        networkedCards.Add("h02", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h03", above: "h01"));
        networkedCards.Add("h03", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h04", above: "h02"));
        networkedCards.Add("h04", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h05", above: "h03"));
        networkedCards.Add("h05", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h06", above: "h04"));
        networkedCards.Add("h06", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h07", above: "h05"));
        networkedCards.Add("h07", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h08", above: "h06"));
        networkedCards.Add("h08", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h09", above: "h07"));
        networkedCards.Add("h09", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h10", above: "h08"));
        networkedCards.Add("h10", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h11", above: "h09"));
        networkedCards.Add("h11", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h12", above: "h10"));
        networkedCards.Add("h12", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "h13", above: "h11"));
        networkedCards.Add("h13", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c01", above: "h12"));
        networkedCards.Add("c01", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c02", above: "h13"));
        networkedCards.Add("c02", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c03", above: "c01"));
        networkedCards.Add("c03", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c04", above: "c02"));
        networkedCards.Add("c04", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c05", above: "c03"));
        networkedCards.Add("c05", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c06", above: "c04"));
        networkedCards.Add("c06", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c07", above: "c05"));
        networkedCards.Add("c07", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c08", above: "c06"));
        networkedCards.Add("c08", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c09", above: "c07"));
        networkedCards.Add("c09", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c10", above: "c08"));
        networkedCards.Add("c10", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c11", above: "c09"));
        networkedCards.Add("c11", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c12", above: "c10"));
        networkedCards.Add("c12", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "c13", above: "c11"));
        networkedCards.Add("c13", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s01", above: "c12"));
        networkedCards.Add("s01", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s02", above: "c13"));
        networkedCards.Add("s02", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s03", above: "s01"));
        networkedCards.Add("s03", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s04", above: "s02"));
        networkedCards.Add("s04", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s05", above: "s03"));
        networkedCards.Add("s05", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s06", above: "s04"));
        networkedCards.Add("s06", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s07", above: "s05"));
        networkedCards.Add("s07", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s08", above: "s06"));
        networkedCards.Add("s08", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s09", above: "s07"));
        networkedCards.Add("s09", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s10", above: "s08"));
        networkedCards.Add("s10", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s11", above: "s09"));
        networkedCards.Add("s11", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s12", above: "s10"));
        networkedCards.Add("s12", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "s13", above: "s11"));
        networkedCards.Add("s13", new NetworkedCardData(faceup: true, spread: false, pos: new Vector3(0.0f, 0.0f, 0.0f), rot: new Vector3(0.0f, 0.0f, 0.0f), below: "", above: "s12"));

        cardGameObjects = new Dictionary<String, GameObject>();
        cardGameObjects.Add("j01", j1);
        cardGameObjects.Add("j02", j2);
        cardGameObjects.Add("d01", d1);
        cardGameObjects.Add("d02", d2);
        cardGameObjects.Add("d03", d3);
        cardGameObjects.Add("d04", d4);
        cardGameObjects.Add("d05", d5);
        cardGameObjects.Add("d06", d6);
        cardGameObjects.Add("d07", d7);
        cardGameObjects.Add("d08", d8);
        cardGameObjects.Add("d09", d9);
        cardGameObjects.Add("d10", d10);
        cardGameObjects.Add("d11", d11);
        cardGameObjects.Add("d12", d12);
        cardGameObjects.Add("d13", d13);
        cardGameObjects.Add("s01", s1);
        cardGameObjects.Add("s02", s2);
        cardGameObjects.Add("s03", s3);
        cardGameObjects.Add("s04", s4);
        cardGameObjects.Add("s05", s5);
        cardGameObjects.Add("s06", s6);
        cardGameObjects.Add("s07", s7);
        cardGameObjects.Add("s08", s8);
        cardGameObjects.Add("s09", s9);
        cardGameObjects.Add("s10", s10);
        cardGameObjects.Add("s11", s11);
        cardGameObjects.Add("s12", s12);
        cardGameObjects.Add("s13", s13);
        cardGameObjects.Add("c01", c1);
        cardGameObjects.Add("c02", c2);
        cardGameObjects.Add("c03", c3);
        cardGameObjects.Add("c04", c4);
        cardGameObjects.Add("c05", c5);
        cardGameObjects.Add("c06", c6);
        cardGameObjects.Add("c07", c7);
        cardGameObjects.Add("c08", c8);
        cardGameObjects.Add("c09", c9);
        cardGameObjects.Add("c10", c10);
        cardGameObjects.Add("c11", c11);
        cardGameObjects.Add("c12", c12);
        cardGameObjects.Add("c13", c13);
        cardGameObjects.Add("h01", h1);
        cardGameObjects.Add("h02", h2);
        cardGameObjects.Add("h03", h3);
        cardGameObjects.Add("h04", h4);
        cardGameObjects.Add("h05", h5);
        cardGameObjects.Add("h06", h6);
        cardGameObjects.Add("h07", h7);
        cardGameObjects.Add("h08", h8);
        cardGameObjects.Add("h09", h9);
        cardGameObjects.Add("h10", h10);
        cardGameObjects.Add("h11", h11);
        cardGameObjects.Add("h12", h12);
        cardGameObjects.Add("h13", h13);


        //InvokeRepeating("printy", 1f, 2.0f);
    }

    internal void SortDeck(string cardID, bool pickup=false)
    {
        List<String> cardNameList = new List<String>();
        string nextCardID = cardID;

        Vector3 card0Pos = NetworkCardManager.Instance.networkedCards[cardID].position;
        Vector3 card0Rot = NetworkCardManager.Instance.networkedCards[cardID].rotation;

        // put the card managers into a list 
        while (nextCardID != "")
        {
            cardNameList.Add(nextCardID);
            NetworkedCardData cardData = NetworkCardManager.Instance.networkedCards[nextCardID];
            nextCardID = cardData.cardAboveID;
        }
        bool deckFaceUp = NetworkCardManager.Instance.networkedCards[cardNameList[cardNameList.Count - 1]].faceUp;
        //bool deckSpread = cardManagerList[0]._spread;
        //Vector3 deckPos = cardManagerList[0].gameObject.transform.position;
        ////sort
        cardNameList.Sort();
  


        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        // order the card manager stacking
        for (int c = 0; c < cardNameList.Count; c++)
        {
            string currentName = cardNameList[c];
            string new_above = "";
            string new_below = "";
            if (c == 0)
            {
                new_above = cardNameList[c + 1].ToString();
            }
            else if (c == cardNameList.Count - 1)
            {
                new_below = cardNameList[c - 1].ToString();
            }
            else
            {
                new_above = cardNameList[c + 1].ToString();
                new_below = cardNameList[c - 1].ToString();

            }
            changeData.Add(currentName, networkedCards[currentName].ToDynamicNetworkString(new_cardAboveID: new_above,
                                                                               new_cardBelowID: new_below,
                                                                               new_posX: card0Pos.x.ToString(),
                                                                               new_posY: card0Pos.y.ToString(),
                                                                               new_posZ: card0Pos.z.ToString(),
                                                                               new_rotX: card0Rot.x.ToString(),
                                                                               new_rotY: card0Rot.y.ToString(),
                                                                               new_rotZ: card0Rot.z.ToString()
                                                                               ));

        }
        PubNubManager.Instance.BroadcastMessage(changeData);

        if (pickup)
        {
            this.SetHeld(cardNameList[0], true);
        }

    }

    internal StickyCardManager GetCardManager(string cardID)
    {
        return cardGameObjects[cardID].GetComponent<StickyCardManager>();
    }


    internal GameObject GetBelow(string cardID)
    {
        string cardBelowID = this.networkedCards[cardID].cardBelowID;
        if (cardBelowID!= "" && this.cardGameObjects.ContainsKey(cardBelowID))
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
        if (cardAboveID != "" && this.cardGameObjects.ContainsKey(cardAboveID))
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

    internal string IsHidden(string cardID)
    {
        return this.networkedCards[cardID].hiddenBy;
    }



    internal void SetHighlighted(string cardID, bool value)
    {
        //Debug.Log("SetHighlighted TO " + value.ToString());
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
        //Debug.Log("SetHeld TO " + value.ToString());
        NetworkedCardData data = this.networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("user", PlayerManager.Instance.uid);
        changeData.Add("card", cardID);
        if (value == true)
        {
            changeData.Add("type", PubNubManager.PLAYER_HELD_START);
            this._CurrentHeldID = cardID;
            InvokeRepeating("UpdateHeldPosition", 1f, 0.50f);
        }
        else
        {
            changeData.Add("type", PubNubManager.PLAYER_HELD_STOP);
            this._CurrentHeldID = "";
            CancelInvoke();
        }
        PubNubManager.Instance.BroadcastMessage(changeData);


    }

    internal void FlipWholeDeck(string cardID, bool pickup=false)
    {
        List<String> cardNameList = new List<String>();
        string nextCardID = cardID;

        Vector3 card0Pos = NetworkCardManager.Instance.networkedCards[cardID].position;
        Vector3 card0Rot = NetworkCardManager.Instance.networkedCards[cardID].rotation;
        bool deckFaceUp = !NetworkCardManager.Instance.networkedCards[cardID].faceUp;
        // put the card managers into a list 
        while (nextCardID != "")
        {
            cardNameList.Insert(0, nextCardID);
            NetworkedCardData cardData = NetworkCardManager.Instance.networkedCards[nextCardID];
            nextCardID = cardData.cardAboveID;
        }





        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        // order the card manager stacking
        for (int c = 0; c < cardNameList.Count; c++)
        {
            string currentName = cardNameList[c];
            string new_above = "";
            string new_below = "";
            if (c == 0)
            {
                new_above = cardNameList[c + 1].ToString();

            }
            else if (c == cardNameList.Count - 1)
            {
                new_below = cardNameList[c - 1].ToString();

            }
            else
            {
                new_above = cardNameList[c + 1].ToString();
                new_below = cardNameList[c - 1].ToString();
            }
            changeData.Add(currentName, networkedCards[currentName].ToDynamicNetworkString(new_cardAboveID: new_above,
                                                                                           new_cardBelowID: new_below,
                                                                                           new_posX: card0Pos.x.ToString(),
                                                                                           new_posY: card0Pos.y.ToString(),
                                                                                           new_posZ: card0Pos.z.ToString(),
                                                                                           new_rotX: card0Rot.x.ToString(),
                                                                                           new_rotY: card0Rot.y.ToString(),
                                                                                           new_rotZ: card0Rot.z.ToString(),
                                                                                           new_faceUp: deckFaceUp.ToString()
                                                                                           )) ;

        }
        PubNubManager.Instance.BroadcastMessage(changeData);

        if (pickup)
        {
            this.SetHeld(cardNameList[0], true);
        }

   

    }

    internal void FlipDeckOneByOne(string cardID)
    {
        //Debug.Log("FlipDeckOneByOne");
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        string nextCard = cardID;
        bool faceUp = !this.networkedCards[cardID].faceUp;
        NetworkedCardData nextCardData;
        while (nextCard != "")
        {
            nextCardData = this.networkedCards[nextCard];
            changeData.Add(nextCard, nextCardData.ToDynamicNetworkString(new_faceUp:faceUp.ToString()));
            nextCard = nextCardData.cardAboveID;
        }
        PubNubManager.Instance.BroadcastMessage(changeData);

    }

    internal void UpdateHeldPosition()
    {
        //Debug.Log("UpdateHeldPosition");
        Vector3 position = PlayerManager.Instance.cursor.transform.localPosition;
        Vector3 rotation = new Vector3(0.0f,
                                       PlayerManager.Instance.cursor.transform.localEulerAngles.y,
                                                          0.0f);


       
        NetworkedCardData data = this.networkedCards[this._CurrentHeldID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        changeData.Add(this._CurrentHeldID, data.ToDynamicNetworkString(
                                                                        new_faceUp:"null",
                                                                        new_hiddenBy:"null",
                                                                        new_spread:"null",
                                                                        new_cardAboveID:"null",
                                                                        new_cardBelowID: "null",
                                                                        
                                                                        new_posX:position.x.ToString(),
                                                                        new_posY: position.y.ToString(),
                                                                        new_posZ: position.z.ToString(),
                                                                        new_rotX: rotation.x.ToString(),
                                                                        new_rotY: rotation.y.ToString(),
                                                                        new_rotZ: rotation.z.ToString()
                                                                        
                                                                        ));
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
      
        //Debug.Log("SetFaceUp TO " + value.ToString());
        NetworkedCardData cardData = this.networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        changeData.Add(cardID, cardData.ToDynamicNetworkString(new_faceUp:value.ToString()));
        PubNubManager.Instance.BroadcastMessage(changeData);
      
    }
    internal void SetSpread(string cardID, bool value)
    {
        //Debug.Log("SetSpread TO " + value.ToString());
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        string nextCard = cardID;
        NetworkedCardData nextCardData;
        NetworkedCardData card0Data = this.networkedCards[nextCard];
        Vector3 card0Pos = card0Data.position;
        Vector3 card0Rot = card0Data.rotation;
        while (nextCard != "")
        {
            nextCardData = this.networkedCards[nextCard];
            changeData.Add(nextCard, nextCardData.ToDynamicNetworkString(new_spread:value.ToString(),
                                                                         new_posX: card0Pos.x.ToString(),
                                                                         new_posY: card0Pos.y.ToString(),
                                                                         new_posZ: card0Pos.z.ToString(),
                                                                         new_rotX: card0Rot.x.ToString(),
                                                                         new_rotY: card0Rot.y.ToString(),
                                                                         new_rotZ: card0Rot.z.ToString()));
            nextCard = nextCardData.cardAboveID;
        }
        PubNubManager.Instance.BroadcastMessage(changeData);
    }

    internal void ShuffleDeck(string cardID, bool pickup=false)
    {
        List<String> cardNameList = new List<String>();
        string nextCardID = cardID;

        Vector3 card0Pos = NetworkCardManager.Instance.networkedCards[cardID].position;
        Vector3 card0Rot = NetworkCardManager.Instance.networkedCards[cardID].rotation;

        // put the card managers into a list 
        while (nextCardID != "")
        {
            cardNameList.Add(nextCardID);
            NetworkedCardData cardData = NetworkCardManager.Instance.networkedCards[nextCardID];
            nextCardID = cardData.cardAboveID;
        }
        bool deckFaceUp = NetworkCardManager.Instance.networkedCards[cardNameList[cardNameList.Count - 1]].faceUp;
        //bool deckSpread = cardManagerList[0]._spread;
        //Vector3 deckPos = cardManagerList[0].gameObject.transform.position;
        ////shuffle
        int n = cardNameList.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string value = cardNameList[k];
            cardNameList[k] = cardNameList[n];
            cardNameList[n] = value;
        }


        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        // order the card manager stacking
        for (int c = 0; c < cardNameList.Count; c++)
        {
            string currentName = cardNameList[c];
            string new_above = "";
            string new_below = "";
            if (c == 0)
            {
                new_above = cardNameList[c + 1].ToString();

                
            }
            else if (c == cardNameList.Count - 1)
            {
                new_below = cardNameList[c - 1].ToString();
            }
            else
            {
                new_below = cardNameList[c - 1].ToString();
                new_above = cardNameList[c + 1].ToString();
            }
            changeData.Add(currentName, networkedCards[currentName].ToDynamicNetworkString(new_cardAboveID: new_above,
                                                                                   new_cardBelowID: new_below,
                                                                                   new_posX: card0Pos.x.ToString(),
                                                                                   new_posY: card0Pos.y.ToString(),
                                                                                   new_posZ: card0Pos.z.ToString(),
                                                                                   new_rotX: card0Rot.x.ToString(),
                                                                                   new_rotY: card0Rot.y.ToString(),
                                                                                   new_rotZ: card0Rot.z.ToString()
                                                                                   ));
        }
        PubNubManager.Instance.BroadcastMessage(changeData);

        if (pickup)
        {
            this.SetHeld(cardNameList[0], true);
        }

    }

    internal void MagnetSingle(string heldCardID, string highlightedCardID)
    {

        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        string belowHighlightedCardID = networkedCards[highlightedCardID].cardBelowID;
        string aboveHighlightedCardID = networkedCards[highlightedCardID].cardAboveID;

        if (aboveHighlightedCardID != "" && belowHighlightedCardID != "")
        {
            // card is middle of deck
            changeData.Add(aboveHighlightedCardID, networkedCards[aboveHighlightedCardID].ToDynamicNetworkString(new_cardBelowID:belowHighlightedCardID));
            changeData.Add(belowHighlightedCardID, networkedCards[belowHighlightedCardID].ToDynamicNetworkString(new_cardAboveID:aboveHighlightedCardID));
        }
        else if (aboveHighlightedCardID == "" && belowHighlightedCardID != "")
        {
            // card is top of deck
            changeData.Add(belowHighlightedCardID, networkedCards[belowHighlightedCardID].ToDynamicNetworkString(new_cardAboveID: ""));
        }
        else if (aboveHighlightedCardID != "" && belowHighlightedCardID == "")
        {
            // card is bottom of deck
            changeData.Add(aboveHighlightedCardID, networkedCards[aboveHighlightedCardID].ToDynamicNetworkString(new_cardBelowID: ""));
        }
        //changeData.Add(highlightedCardID, networkedCards[highlightedCardID].AboveBelowChangeString(heldCardID, "",
        //                                                                                           networkedCards[highlightedCardID].position,
        //                                                                                           networkedCards[highlightedCardID].rotation,
        //                                                                                           new_spread: "false"));
        Vector3 heldPos = networkedCards[highlightedCardID].position;
        Vector3 heldRot = networkedCards[highlightedCardID].rotation;

        changeData.Add(highlightedCardID, networkedCards[highlightedCardID].ToDynamicNetworkString(new_cardAboveID:heldCardID,
                                                                                                   new_cardBelowID: "",
                                                                                                   new_posX: heldPos.x.ToString(),
                                                                                                   new_posY: heldPos.y.ToString(),
                                                                                                   new_posZ: heldPos.z.ToString(),
                                                                                                   new_rotX: heldPos.x.ToString(),
                                                                                                   new_rotY: heldPos.y.ToString(),
                                                                                                   new_rotZ: heldPos.z.ToString(),
                                                                                                   new_spread: "false"));
        changeData.Add(heldCardID, networkedCards[heldCardID].ToDynamicNetworkString(new_cardBelowID: highlightedCardID));

        PubNubManager.Instance.BroadcastMessage(changeData);

        this.SetHeld(heldCardID, false);
        this.SetHeld(highlightedCardID, true);
    }

    internal void SinglePickUp(string cardID)
    {
        //this.SetHeld(cardID, true);
       
        NetworkedCardData cardData = networkedCards[cardID];

        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        changeData.Add(cardID, cardData.ToDynamicNetworkString(new_cardAboveID: "",
                                                       new_cardBelowID: "",
                                                       new_spread: "false"));
        string below = cardData.cardBelowID;
        string above = cardData.cardAboveID;
        if (below != "" && above!= "")
        {
            //card is in the middle of a deck
            NetworkedCardData cardBelowData = networkedCards[below];
            NetworkedCardData cardAboveData = networkedCards[above];

            changeData.Add(below, cardBelowData.ToDynamicNetworkString(new_cardAboveID:above));

            changeData.Add(above, cardAboveData.ToDynamicNetworkString(new_cardBelowID:below,
                                                                                      new_posX: cardAboveData.position.x.ToString(),
                                                                                      new_posY: cardAboveData.position.y.ToString(),
                                                                                      new_posZ: cardAboveData.position.z.ToString(),
                                                                                      new_rotX: cardAboveData.rotation.x.ToString(),
                                                                                      new_rotY: cardAboveData.rotation.z.ToString(),
                                                                                      new_rotZ: cardAboveData.rotation.y.ToString()));
        }
        else if (below != "" && above == "")
        {
            //card is top of deck
            NetworkedCardData cardBelowData = networkedCards[below];

            changeData.Add(below, cardBelowData.ToDynamicNetworkString(new_cardAboveID:""));
        }
        else if (below == "" && above != "")
        {
            //card is bottom of deck
            NetworkedCardData cardAboveData = networkedCards[above];

            changeData.Add(above, cardAboveData.ToDynamicNetworkString(new_cardBelowID:""));

        }
        PubNubManager.Instance.BroadcastMessage(changeData);

        this.SetHeld(cardID, true);

    }
    internal void MagnetDeck(string heldCardID, string bottomCardID, string topCardID)
    {
        NetworkedCardData heldCardData = networkedCards[heldCardID];
        NetworkedCardData bottomCardData = networkedCards[bottomCardID];
        

        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        
        

        if (bottomCardData.cardBelowID != "")
        {
            NetworkedCardData belowThisCardData = networkedCards[bottomCardData.cardBelowID];
            changeData.Add(bottomCardData.cardBelowID, belowThisCardData.ToDynamicNetworkString(new_cardAboveID:""));

        }


        string nextCard = bottomCardID;
        NetworkedCardData nextCardData;
        while (nextCard != "")
        {
            nextCardData = this.networkedCards[nextCard];
            string new_above = null;
            string new_below = null;
            if (nextCard == bottomCardID)
            {
                new_below = "";
            }
            else if (nextCard == topCardID)
            {
                new_above = heldCardID;
            }
            changeData.Add(nextCard, nextCardData.ToDynamicNetworkString(new_spread:"false",
                                                                         new_posX: heldCardData.position.x.ToString(),
                                                                         new_posY: heldCardData.position.y.ToString(),
                                                                         new_posZ: heldCardData.position.z.ToString(),
                                                                         new_rotX: heldCardData.rotation.x.ToString(),
                                                                         new_rotY: heldCardData.rotation.y.ToString(),
                                                                         new_rotZ: heldCardData.rotation.z.ToString(),
                                                                         new_cardAboveID: new_above,
                                                                         new_cardBelowID: new_below
                                                                          ));
            nextCard = nextCardData.cardAboveID;
        }
        changeData.Add(heldCardID, heldCardData.ToDynamicNetworkString(new_cardBelowID:topCardID,
                                                                        new_posX: heldCardData.position.x.ToString(),
                                                                        new_posY: heldCardData.position.y.ToString(),
                                                                        new_posZ: heldCardData.position.z.ToString(),
                                                                        new_rotX: heldCardData.rotation.x.ToString(),
                                                                        new_rotY: heldCardData.rotation.y.ToString(),
                                                                        new_rotZ: heldCardData.rotation.z.ToString()));

        //PubNubManager.Instance.BroadcastMessage(changeData);


        this.SetHeld(bottomCardID, true);

        PubNubManager.Instance.BroadcastMessage(changeData);
    }


    internal void DeckPickup(string cardID)
    {
        NetworkedCardData cardData = networkedCards[cardID];

        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        string nextCard = cardID;
        NetworkedCardData nextCardData;
        while (nextCard != "")
        {
            nextCardData = this.networkedCards[nextCard];
            string new_below = null;
            if (nextCard == cardID)
            {
                new_below = "";

            }
            changeData.Add(nextCard, nextCardData.ToDynamicNetworkString(new_cardBelowID: new_below,
                                                                new_posX: cardData.position.x.ToString(),
                                                                new_posY: cardData.position.y.ToString(),
                                                                new_posZ: cardData.position.z.ToString(),
                                                                new_rotX: cardData.rotation.x.ToString(),
                                                                new_rotY: cardData.rotation.y.ToString(),
                                                                new_rotZ: cardData.rotation.z.ToString(),
                                                                new_spread: "false"));
            nextCard = nextCardData.cardAboveID;
        }



        if (cardData.cardBelowID != "")
        {
            NetworkedCardData belowThisCardData = networkedCards[cardData.cardBelowID];
            changeData.Add(cardData.cardBelowID, belowThisCardData.ToDynamicNetworkString(new_cardAboveID:""));

        }

        this.SetHeld(cardID, true);

        PubNubManager.Instance.BroadcastMessage(changeData);
    }


    internal void DropOneCardOnTopOf(string thisCardID, string thatCardID)
    {
        if (thisCardID == thatCardID)
        {
            return;
        }
        //this.SetHeld(cardID, true);
        //StickyCardManager thisCard = cardGameObjects[thisCardID].GetComponent<StickyCardManager>();
        NetworkedCardData thisCardData = networkedCards[thisCardID];
        //StickyCardManager thatCard = cardGameObjects[thatCardID].GetComponent<StickyCardManager>();
        NetworkedCardData thatCardData = networkedCards[thatCardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        this.SetHeld(thisCardID, false);

        string new_spread = thatCardData.spread.ToString();

        changeData.Add(thatCardID, thatCardData.ToDynamicNetworkString(new_cardAboveID:thisCardID, new_spread: new_spread));
        changeData.Add(thisCardID, thisCardData.ToDynamicNetworkString(new_cardAboveID:thatCardData.cardAboveID,
                                                                       new_cardBelowID:thatCardID,
                                                                       new_posX: thatCardData.position.x.ToString(),
                                                                       new_posY: thatCardData.position.y.ToString(),
                                                                       new_posZ: thatCardData.position.z.ToString(),
                                                                       new_rotX: thatCardData.rotation.x.ToString(),
                                                                       new_rotY: thatCardData.rotation.y.ToString(),
                                                                       new_rotZ: thatCardData.rotation.z.ToString(),
                                                                       new_spread: new_spread));

        if (thisCardData.cardAboveID != "" && thisCardData.cardAboveID != thatCardID)
        {
            NetworkedCardData aboveThisCardData = networkedCards[thisCardData.cardAboveID];
            changeData.Add(thisCardData.cardAboveID, aboveThisCardData.ToDynamicNetworkString(new_cardBelowID:""));
            this.SetHeld(thisCardData.cardAboveID, true);
        }

        if (thatCardData.cardAboveID != "" && thatCardData.cardAboveID != thisCardID)
        {
            string aboveThatCardID = thatCardData.cardAboveID;
            NetworkedCardData aboveThatCardData = networkedCards[aboveThatCardID];
            changeData.Add(aboveThatCardID, aboveThatCardData.ToDynamicNetworkString(new_cardBelowID:thisCardID));
        }


        PubNubManager.Instance.BroadcastMessage(changeData);
    }

    internal void DropOneCardOntoTable(string cardID)
    {

        this.SetHeld(cardID, false);

        NetworkedCardData cardData = networkedCards[cardID];
        if (cardData.cardAboveID != "")
        {
            Dictionary<string, string> changeData = new Dictionary<string, string>();
            changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
            NetworkedCardData aboveThisCardData = networkedCards[cardData.cardAboveID];
            changeData.Add(cardID, cardData.ToDynamicNetworkString(new_cardAboveID:""));
            changeData.Add(cardData.cardAboveID, aboveThisCardData.ToDynamicNetworkString(new_cardBelowID:""));
            PubNubManager.Instance.BroadcastMessage(changeData);

            this.SetHeld(cardData.cardAboveID, true);
        }
    }

    //internal void DropOneCardUnderneath(string thisCardID, string thatCardID)
    //{
    //    //NetworkedCardData thisCardData = networkedCards[thisCardID];
    //    //NetworkedCardData thatCardData = networkedCards[thatCardID];
    //    //Dictionary<string, string> changeData = new Dictionary<string, string>();
    //    //changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

    //    //string new_spread = thatCardData.spread.ToString();

    //    //this.SetHeld(thisCardID, false);

    //    //changeData.Add(thatCardID, thatCardData.BelowChangeString(thisCardID));
    //    //changeData.Add(thisCardID, thisCardData.AboveBelowChangeString(thatCardID, thatCardData.cardBelowID, new_spread: new_spread));

    //    //if (thisCardData.cardAboveID != "")
    //    //{
    //    //    NetworkedCardData aboveThisCardData = networkedCards[thisCardData.cardAboveID];
    //    //    changeData.Add(thisCardData.cardAboveID, aboveThisCardData.BelowChangeString(""));
    //    //    this.SetHeld(thisCardData.cardAboveID, true);
    //    //}

    //    //if (thatCardData.cardBelowID != "")
    //    //{
    //    //    string belowThatCardID = thatCardData.cardBelowID;
    //    //    NetworkedCardData belowThatCardData = networkedCards[belowThatCardID];
    //    //    changeData.Add(belowThatCardID, belowThatCardData.AboveChangeString(thisCardID));
    //    //}


    //    //PubNubManager.Instance.BroadcastMessage(changeData);
    //}


    internal void DropDeckOnTopOf(string deckBottomID, string deckTopID, string highlightedID)
    {
        this.SetHeld(deckBottomID, false);

        NetworkedCardData topCardData = networkedCards[deckTopID];
        NetworkedCardData bottomCardData = networkedCards[deckBottomID];
        NetworkedCardData highlightedCardData = networkedCards[highlightedID];

        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);

        string spread_str = highlightedCardData.spread.ToString();

        changeData.Add(deckBottomID, bottomCardData.ToDynamicNetworkString(new_cardBelowID: highlightedID, new_spread:spread_str));
        changeData.Add(deckTopID, topCardData.ToDynamicNetworkString(new_cardAboveID:highlightedCardData.cardAboveID, new_spread: spread_str));
        changeData.Add(highlightedID, highlightedCardData.ToDynamicNetworkString(new_cardAboveID:deckBottomID));

        if (highlightedCardData.cardAboveID!= "")
        {
            NetworkedCardData aboveHighlightedData = networkedCards[highlightedCardData.cardAboveID];
            changeData.Add(highlightedCardData.cardAboveID, aboveHighlightedData.ToDynamicNetworkString(new_cardBelowID:deckTopID));
        }

        //update spreads on cards between top and bottom of deck
        string nextCard = bottomCardData.cardAboveID;
        NetworkedCardData nextCardData;
        while (nextCard != deckTopID)
        {
            nextCardData = this.networkedCards[nextCard];
            changeData.Add(nextCard, nextCardData.ToDynamicNetworkString(new_spread:highlightedCardData.spread.ToString(),
                                                                         new_posX: highlightedCardData.position.x.ToString(),
                                                                         new_posY: highlightedCardData.position.y.ToString(),
                                                                         new_posZ: highlightedCardData.position.z.ToString(),
                                                                         new_rotX: highlightedCardData.rotation.x.ToString(),
                                                                         new_rotY: highlightedCardData.rotation.y.ToString(),
                                                                         new_rotZ: highlightedCardData.rotation.z.ToString()));
            nextCard = nextCardData.cardAboveID;
        }

        PubNubManager.Instance.BroadcastMessage(changeData);
    }

    internal void DropDeckOntoTable(string cardID)
    {
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        string nextCard = cardID;
        NetworkedCardData nextCardData;
        NetworkedCardData bottomCardData = this.networkedCards[cardID]; ;
        while (nextCard != "")
        {
            nextCardData = this.networkedCards[nextCard];
            changeData.Add(nextCard, nextCardData.ToDynamicNetworkString(new_posX: bottomCardData.position.x.ToString(),
                                                                         new_posY: bottomCardData.position.y.ToString(),
                                                                         new_posZ: bottomCardData.position.z.ToString(),
                                                                         new_rotX: bottomCardData.rotation.x.ToString(),
                                                                         new_rotY: bottomCardData.rotation.y.ToString(),
                                                                         new_rotZ: bottomCardData.rotation.z.ToString()
                                                                         ));
            nextCard = nextCardData.cardAboveID;
        }
        PubNubManager.Instance.BroadcastMessage(changeData);
        this.SetHeld(cardID, false);
    }




    internal void SetHideDeckTo(string cardID, string value)
    {
        //Debug.Log("Set Hide to " + value.ToString());
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        string nextCard = cardID;
        NetworkedCardData nextCardData;
        while (nextCard != "")
        {
            nextCardData = this.networkedCards[nextCard];
            changeData.Add(nextCard, nextCardData.ToDynamicNetworkString(new_hiddenBy:value));
            nextCard = nextCardData.cardAboveID;
        }
        PubNubManager.Instance.BroadcastMessage(changeData);
    }

    internal void SetHideCardTo(string cardID, string value)
    {
        Debug.Log("Set Hide to " + value.ToString());
        NetworkedCardData cardData = this.networkedCards[cardID];
        Dictionary<string, string> changeData = new Dictionary<string, string>();
        changeData.Add("type", PubNubManager.PLAYER_HAS_CHANGED_DECK_DATA);
        changeData.Add(cardID, cardData.ToDynamicNetworkString(new_hiddenBy:value));
        PubNubManager.Instance.BroadcastMessage(changeData);
    }


}
