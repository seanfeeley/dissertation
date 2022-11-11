using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PubNubAPI;
using System;
using Newtonsoft.Json;




public class PubNubManager : MonoBehaviour
{

    public static PubNubManager Instance;
    public bool WaitingOnUpdate = false;

    private PubNub pubnub;

    private string _subscribeKey = "sub-c-5f7e2cce-68db-48d4-ab94-902cb1050287";
    private string _publishKey = "pub-c-7474fd32-7212-446d-933f-4f4900669643";
    private string _secretKey = "sec-c-YmIyYmMxYTctNmVkMC00ZWEzLTliMWUtYjU4MjUwMmNlZDVk";
    private string _channel = "CardsAR";

    private bool _waiting_on_deck_init;


    public const string PLAYER_HAS_CHANGED_POSITION = "0";

    public const string PLAYER_HAS_CHANGED_DECK_DATA = "1";
    public const string PLAYER_HIGHLIGHT_START = "2";
    public const string PLAYER_HIGHLIGHT_STOP = "3";
    public const string PLAYER_HELD_START = "4";
    public const string PLAYER_HELD_STOP = "5";
    //public const string PLAYER_HAS_CHANGED_HELD_DATA = "3";

    //public const string HOST_HAS_CHANGED_DECK_DATA = "4";
    //public const string HOST_HAS_CHANGED_HIGHLIGHT_DATA = "5";


    public const string PLAYER_REQUESTING_HOST_UPDATE = "90";

    Dictionary<string, string> HighlightChangeData;
    Dictionary<string, string> HeldChangeData;


    private void Awake()
    {
        Instance = this;
      
    }

    private void Start()
    {
        InvokeRepeating("BroadcastPosition", 1f, 0.2f);  //1s delay, repeat every 1s
        InvokeRepeating("BroadcastHighlight", 1f, 0.2f);  //1s delay, repeat every 1s
        //InvokeRepeating("BroadcastHeld", 1f, 0.1f);  //1s delay, repeat every 1s
        //InvokeRepeating("BroadcastChanges", 1f, 1.0f);  //1s delay, repeat every 1s
        
        
    }

    private void RequestUpdateFromHost()
    {
        Dictionary<string, object> message = new Dictionary<string, object>();
        message.Add("type", PLAYER_REQUESTING_HOST_UPDATE);
        this._PostMessage(message);
    }



    public void ConnectToPubNub()
    {
        PNConfiguration pnConfiguration = new PNConfiguration();
        pnConfiguration.SubscribeKey = _subscribeKey;
        pnConfiguration.PublishKey = _publishKey;
        pnConfiguration.SecretKey = _secretKey;
        pnConfiguration.LogVerbosity = PNLogVerbosity.BODY;
        pnConfiguration.UserId = "myUniqueUserId";
        pubnub = new PubNub(pnConfiguration);

        Dictionary<string, string> message = new Dictionary<string, string>();
        pubnub.SubscribeCallback += (sender, e) => {
            SubscribeEventEventArgs mea = e as SubscribeEventEventArgs;


            if (mea.MessageResult != null)
            {
                //Debug.Log("In Example, SubscribeCallback in message" + mea.MessageResult.Channel);
                Dictionary<string, object> message_payload = mea.MessageResult.Payload as Dictionary<string, object>;
                //Debug.Log("msg: " + msg["msg"]);
                this.ProcessMessage(message_payload);
            }
            if (mea.PresenceEventResult != null)
            {
                //Debug.Log("In Example, SubscribeCallback in presence" + mea.PresenceEventResult.Channel + mea.PresenceEventResult.Occupancy + mea.PresenceEventResult.Event);
            }
        };

        pubnub.Subscribe()
            .Channels(new List<string>(){
            "my_channel"
            })
            .Execute();



        if (MultiplayerNetworkingManager.Instance.AmIHost())
        {
            // i am the authority
            _waiting_on_deck_init = false;
        }
        else
        {
            // i need to be informed of the deck state
            this.RequestUpdateFromHost();
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    private void ProcessMessage(Dictionary<string, object> message_payload)
    {
        string message_type = (string)message_payload["type"];
        switch (message_type)
        {
            //player avatar movements
            case PLAYER_HAS_CHANGED_POSITION:
                this.ProcessPlayerPos(message_payload);
                break;


            //highlight changes
            case PLAYER_HIGHLIGHT_START:
                Debug.Log("PLAYER_HIGHLIGHT_START");
                this.ProcessHostHighlightStart(message_payload);
                break;
            case PLAYER_HIGHLIGHT_STOP:
                Debug.Log("PLAYER_HIGHLIGHT_STOP");
                this.ProcessHostHighlightStop(message_payload);
                break;


            //held changes
            case PLAYER_HELD_START:
                Debug.Log("PLAYER_HELD_START");
                this.ProcessHostHeldStart(message_payload);
                break;
            case PLAYER_HELD_STOP:
                Debug.Log("PLAYER_HELD_STOP");
                this.ProcessHostHeldStop(message_payload);
                break;


            case PLAYER_HAS_CHANGED_DECK_DATA:
                Debug.Log("PLAYER_HAS_CHANGED_DECK_DATA");
                // only react to other player changes if I am the host
                this.ProcessPlayerDeckChange(message_payload);
                break;

            case PLAYER_REQUESTING_HOST_UPDATE:
                Debug.Log("PLAYER_REQUESTING_HOST_UPDATE");
                if (MultiplayerNetworkingManager.Instance.AmIHost())
                {
                    BroadcastDeck();
                }
                break;
            //case HOST_DECK:
            //    Debug.Log("HOST_DECK");
            //    // only react to hosty broadcasts changes if I am NOT the host
            //    //if (!MultiplayerNetworkingManager.Instance.AmIHost())
            //    //{
            //    this.ProcessDeckData(message_payload);
            //    //}
            //    break;
            //case HOST_HIGHLIGHTS:
            //    Debug.Log("HOST_HIGHLIGHTS");
            //    // only react to hosty broadcasts changes if I am NOT the host
            //    //if (!MultiplayerNetworkingManager.Instance.AmIHost())
            //    //{
            //    this.ProcessDeckHighlights(message_payload);
            //    //}
            //    break;
            default:
                // code block
                break;
        }


    }

    private void ProcessHostHeldStop(Dictionary<string, object> message_payload)
    {
        string userID = (string)message_payload["user"];
        NetworkCardManager.Instance.NetworkedCardHeldBy[userID] = "";
    }

    private void ProcessHostHeldStart(Dictionary<string, object> message_payload)
    {
        string userID = (string)message_payload["user"];
        string cardID = (string)message_payload["card"];
        NetworkCardManager.Instance.NetworkedCardHeldBy[userID] = cardID;
    }

    private void ProcessHostHighlightStop(Dictionary<string, object> message_payload)
    {
        string userID = (string)message_payload["user"];
        NetworkCardManager.Instance.NetworkedCardHighlightedBy[userID] = "";
    }

    private void ProcessHostHighlightStart(Dictionary<string, object> message_payload)
    {
        string userID = (string)message_payload["user"];
        string cardID = (string)message_payload["card"];
        NetworkCardManager.Instance.NetworkedCardHighlightedBy[userID] = cardID;
    }


    private void ProcessPlayerDeckChange(Dictionary<string, object> message_payload)
    {
        foreach (string cardId in message_payload.Keys)
        {
            if (cardId != "type")
            {
                NetworkedCardData cardData = NetworkCardManager.Instance.networkedCards[cardId];

                Debug.Log("============================");

                Debug.Log("CARD DATA CHANGING: "+ cardId);
                Debug.Log("change string: " + (string)message_payload[cardId]);
                Debug.Log("before: " + cardData.ToNetworkString());
                cardData.fromNetworkString((string)message_payload[cardId]);
                Debug.Log("after: " + cardData.ToNetworkString());
                NetworkCardManager.Instance.printy();


            }
        }
    }

    //private void ProcessDeckData(Dictionary<string, object> message_payload)
    //{
    //    foreach (string cardId in NetworkCardManager.Instance.networkedCards.Keys)
    //    {
    //        NetworkCardManager.Instance.networkedCards[cardId].fromNetworkString((string)message_payload[cardId]);
    //        //Debug.Log((string)message_payload[cardId] + CardDeckManager.Instance.networkedCards[cardId].position.ToString());
    //    }
    //}

    private void ProcessPlayerPos(Dictionary<string, object> message_payload)
    {
        string player_uid = (string)message_payload["uid"];
        Vector3 pos = StringToVector3((string)message_payload["pos"]);
        Vector3 rot = StringToVector3((string)message_payload["rot"]);
        MultiplayerNetworkingManager.Instance.OnPeerPoseReceived(player_uid, pos, rot);
    }

    //public void BroadcastChanges()
    //{

    //    while (NetworkCardManager.Instance.networkedCardDataChanges.Count != 0)
    //    {
    //        Dictionary<string, string> changeData = NetworkCardManager.Instance.networkedCardDataChanges.Dequeue();
    //        Dictionary<string, object> message = new Dictionary<string, object>();
    //        message.Add("type", PLAYER_HAS_CHANGED_DECK_DATA);
    //        //Dictionary<string, NetworkedCardData>() networkedCards = ;
    //        foreach (string cardId in changeData.Keys)
    //        {
    //            message.Add(cardId, changeData[cardId]);
    //        }
    //        this._PostMessage(message);

    //    }

    //    ////Debug.Log("BroadcastDeck");
    //    //Dictionary<string, object> message = new Dictionary<string, object>();
    //}

    //internal void BroadcastHighlightChanges()
    //{

    //    while (NetworkCardManager.Instance.networkedCardHightlightChanges.Count != 0)
    //    {
    //        Dictionary<string, string> changeData = NetworkCardManager.Instance.networkedCardHightlightChanges.Dequeue();
    //        Dictionary<string, object> message = new Dictionary<string, object>();
    //        message.Add("type", PLAYER_HIGHLIGHT_CHANGE);
    //        //Dictionary<string, NetworkedCardData>() networkedCards = ;
    //        foreach (string cardId in changeData.Keys)
    //        {
    //            message.Add(cardId, changeData[cardId]);
    //        }
    //        this._PostMessage(message);

    //    }
    //}

    public void BroadcastDeck()
    {

        //Debug.Log("BroadcastDeck");
        Dictionary<string, object> message = new Dictionary<string, object>();
        message.Add("type", PLAYER_HAS_CHANGED_DECK_DATA);
        foreach (string cardId in NetworkCardManager.Instance.networkedCards.Keys)
        {
            NetworkedCardData cardData = NetworkCardManager.Instance.networkedCards[cardId];
            message.Add(cardId, cardData.ToNetworkString());
        }
        this._PostMessage(message);

    }
    public void BroadcastHighlight()
    {
        if (this.HighlightChangeData != null)
        {
            Dictionary<string, object> message = new Dictionary<string, object>();
            foreach (string key in this.HighlightChangeData.Keys)
            {
                message.Add(key, HighlightChangeData[key]);
            }
            this._PostMessage(message);
        }
        this.HighlightChangeData = null;
    }

    public void BroadcastHeld()
    {
        if (this.HeldChangeData != null)
        {
            Dictionary<string, object> message = new Dictionary<string, object>();
            foreach (string key in this.HeldChangeData.Keys)
            {
                message.Add(key, HeldChangeData[key]);
            }
            this._PostMessage(message);
        }
        this.HeldChangeData = null;
    }


    //private void BroadcastHighlights()
    //{
    //    Debug.Log("BroadcastHighlights");
    //    Dictionary<string, object> message = new Dictionary<string, object>();
    //    message.Add("type", HOST_HIGHLIGHTS);
    //    //Dictionary<string, NetworkedCardData>() networkedCards = ;
    //    foreach (string userID in NetworkCardManager.Instance.networkedCardHighlights.Keys)
    //    {
    //        message.Add(userID, NetworkCardManager.Instance.networkedCardHighlights[userID]);
    //    }
    //    this._PostMessage(message);
    //}

    public void BroadcastPosition()
    {

        Dictionary<string, object> message = new Dictionary<string, object>();
        message.Add("type", PLAYER_HAS_CHANGED_POSITION);
        Vector3 pos = PlayerManager.Instance.GetPlayerPos();
        Vector3 rot = PlayerManager.Instance.GetPlayerRot();
        message.Add("pos", pos.ToString());
        message.Add("rot", rot.ToString());
        message.Add("uid", PlayerManager.Instance.uid);
        this._PostMessage(message);

    }


    internal void BroadcastMessage(Dictionary<string, string> changeData)
    {
        Dictionary<string, object> message = new Dictionary<string, object>();
        foreach (string key in changeData.Keys)
        {
            message.Add(key, changeData[key]);
        }
        this._PostMessage(message);
    }

    // Update is called once per frame
    private void _PostMessage(Dictionary<string, object> message)
    {
        if (pubnub != null)
        {
            pubnub.Publish()
             .Channel("my_channel")
             .Message(message)
             .Async((result, status) => {
                 if (!status.Error)
                 {
                     //Debug.Log(string.Format("PostingMessage..."));
                 }
                 else
                 {
                     Debug.Log(status.Error);
                     Debug.Log(status.ErrorData.Info);
                 }
             });

        }
    }
    void OnApplicationQuit()
    {
        if (pubnub != null)
        {
            pubnub.CleanUp();
        }
    }

    internal void StoreHighlightMessage(Dictionary<string, string> changeData)
    {
        this.HighlightChangeData = changeData;
    }
    internal void StoreHeldMessage(Dictionary<string, string> changeData)
    {
        this.HeldChangeData = changeData;
    }
}
