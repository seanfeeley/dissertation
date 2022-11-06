using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PubNubAPI;
using System;
using Newtonsoft.Json;




public class PubNubManager : MonoBehaviour
{

    public static PubNubManager Instance;
    private PubNub pubnub;

    private string _subscribeKey = "sub-c-5f7e2cce-68db-48d4-ab94-902cb1050287";
    private string _publishKey = "pub-c-7474fd32-7212-446d-933f-4f4900669643";
    private string _secretKey = "sec-c-YmIyYmMxYTctNmVkMC00ZWEzLTliMWUtYjU4MjUwMmNlZDVk";
    private string _channel = "CardsAR";

    private void Awake()
    {
        Instance = this;
      
    }

    private void Start()
    {
        InvokeRepeating("BroadcastPosition", 1f, 0.1f);  //1s delay, repeat every 1s
        InvokeRepeating("BroadcastDeck", 1f, .1f);  //1s delay, repeat every 1s
        InvokeRepeating("BroadcastChanges", 1f, .1f);  //1s delay, repeat every 1s



    }

    const string PLAYER_POS = "playerPos";
    const string PLAYER_DECK_CHANGE = "deckChange";
    const string HOST_DECK = "deck";

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
        string type_str = (string)message_payload["type"];
        //switch (type_str)
        //{
        //    case PLAYER_POS:
        //        // code block
        //        this.ProcessPlayerPos(message_payload);
        //        break;
        //    case PLAYER_DECK_CHANGE:
        //        // code block
        //        if (CardDeckManager.Instance.networkedCardChanges.Peek() != null)
        //        {
        //            this.ProcessPlayerDeckChange(message_payload);
        //        }
        //        break;
        //    case HOST_DECK:
        //        // code block
        //        this.ProcessDeckData(message_payload);
        //        break;
        //    default:
        //        // code block
        //        break;
        //}


    }

    private void ProcessPlayerDeckChange(Dictionary<string, object> message_payload)
    {
        foreach (string cardId in CardDeckManager.Instance.networkedCards.Keys)
        {
            CardDeckManager.Instance.networkedCards[cardId].fromNetworkString((string)message_payload[cardId]);
        }
    }

    private void ProcessDeckData(Dictionary<string, object> message_payload)
    {
        foreach (string cardId in CardDeckManager.Instance.networkedCards.Keys)
        {
            CardDeckManager.Instance.networkedCards[cardId].fromNetworkString((string)message_payload[cardId]);
            //Debug.Log((string)message_payload[cardId] + CardDeckManager.Instance.networkedCards[cardId].position.ToString());
        }
    }

    private void ProcessPlayerPos(Dictionary<string, object> message_payload)
    {
        string player_uid = (string)message_payload["uid"];
        Vector3 pos = StringToVector3((string)message_payload["pos"]);
        Vector3 rot = StringToVector3((string)message_payload["rot"]);
        MultiplayerNetworkingManager.Instance.OnPeerPoseReceived(player_uid, pos, rot);
    }

    public void BroadcastChanges()
    {
        if (CardDeckManager.Instance.networkedCardChanges.Count != 0)
        {
            Dictionary<string, NetworkedCardData> changeData = CardDeckManager.Instance.networkedCardChanges.Dequeue();
            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("type", PLAYER_DECK_CHANGE);
            //Dictionary<string, NetworkedCardData>() networkedCards = ;
            foreach (string cardId in changeData.Keys)
            {
                NetworkedCardData cardData = CardDeckManager.Instance.networkedCards[cardId];
                message.Add(cardId, cardData.ToNetworkString());
            }
            this._PostMessage(message);

        }
        ////Debug.Log("BroadcastDeck");
        //Dictionary<string, object> message = new Dictionary<string, object>();


    }

    public void BroadcastDeck()
    {
        if (MultiplayerNetworkingManager.Instance.AmIHost())
        {
            //Debug.Log("BroadcastDeck");
            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("type", HOST_DECK);
            //Dictionary<string, NetworkedCardData>() networkedCards = ;
            foreach (string cardId in CardDeckManager.Instance.networkedCards.Keys)
            {
                NetworkedCardData cardData = CardDeckManager.Instance.networkedCards[cardId];
                message.Add(cardId, cardData.ToNetworkString());
            }
            this._PostMessage(message);
        }
    }
    


    public void BroadcastPosition()
    {
        
            Dictionary<string, object> message = new Dictionary<string, object>();
            message.Add("type", PLAYER_POS);
            Vector3 pos = PlayerManager.Instance.GetPlayerPos();
            Vector3 rot = PlayerManager.Instance.GetPlayerRot();
            message.Add("pos", pos.ToString());
            message.Add("rot", rot.ToString());
            message.Add("uid", PlayerManager.Instance.uid);
            this._PostMessage(message);
        
    }

    // Update is called once per frame
    public void _PostMessage(Dictionary<string, object> message)
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

}
