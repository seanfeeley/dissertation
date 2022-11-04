//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;


//using agora_rtm;
//using System;
//using System.Linq;

//[System.Serializable]
//public class Message
//{
//    public string text;
//    public Text textObj;
//    public MessageType messageType;

//    public enum MessageType
//    {
//        Info,
//        Error,
//        PlayerMessage,
//        ChannelMessage,
//        PeerMessage
//    }
//}

//[System.Serializable]
//public struct MessageColorStruct
//{
//    public Color infoColor, errorColor, playerColor, peerColor, channelColor;
//}


//public class AgoraSignalManager : MonoBehaviour
//{
//    private string serverUrl = "https://agora-token-service-production-aa7d.up.railway.app"; // The base URL to your token server. For example, https://app-name.herokuapp.com"
//    private int ExpireTime = 60 * 60 * 24; //Expire time in Seconds.


//    // Fill in your app ID.
//    private string _appID = "1123e853e8ac4062a601cbb76526c742";
//    // Fill in your channel name.
//    private string _channelName = "CardsAR01";
//    // Fill in the temporary token you obtained from Agora Console.
//    private string _token = "007eJxTYBCa8dOrv8Lr7+WbuwKcZCIY/8nNT7JgvMa8afbm+z3NETUKDIaGRsapFqZAnJhsYmBmlGhmYJiclGRuZmpklmxuYiS5PTm5IZCRgdWKiYmRAQJBfE4G58SilGLHIANDBgYAnqQeZw==";


//    private RtmClient rtmClient = null;
//    private RtmChannel channel;

//    private RtmClientEventHandler clientEventHandler;
//    private RtmChannelEventHandler channelEventHandler;
//    private RtmCallEventHandler callEventHandler;

//    public static AgoraSignalManager Instance;

//    agora_rtm.SendMessageOptions _MessageOptions = new agora_rtm.SendMessageOptions()
//    {
//        enableOfflineMessaging = true,
//        enableHistoricalMessaging = true
//    };

//    private void Awake()
//    {
//        Instance = this;
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        clientEventHandler = new RtmClientEventHandler();
//        channelEventHandler = new RtmChannelEventHandler();
//        callEventHandler = new RtmCallEventHandler();

//        rtmClient = new RtmClient(_appID, clientEventHandler);
//#if UNITY_EDITOR
//        rtmClient.SetLogFile("./rtm_log.txt");
//#endif

//        clientEventHandler.OnQueryPeersOnlineStatusResult = OnQueryPeersOnlineStatusResultHandler;
//        clientEventHandler.OnLoginSuccess = OnClientLoginSuccessHandler;
//        clientEventHandler.OnLoginFailure = OnClientLoginFailureHandler;
//        clientEventHandler.OnMessageReceivedFromPeer = OnMessageReceivedFromPeerHandler;

//        channelEventHandler.OnJoinSuccess = OnJoinSuccessHandler;
//        channelEventHandler.OnJoinFailure = OnJoinFailureHandler;
//        channelEventHandler.OnLeave = OnLeaveHandler;
//        channelEventHandler.OnMessageReceived = OnChannelMessageReceivedHandler;
//    }

//    public void JoinChat()
//    {
//        this._token = AgoraVoiceManager.Instance._token;
//        Login();
//        JoinChannel();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//    void OnApplicationQuit()
//    {
//        if (channel != null)
//        {
//            channel.Dispose();
//            channel = null;
//        }
//        if (rtmClient != null)
//        {
//            rtmClient.Dispose();
//            rtmClient = null;
//        }
//    }
//    // Log in
//    public void Login()
//    {


//        if (string.IsNullOrEmpty(PlayerManager.Instance.uid) || string.IsNullOrEmpty(PlayerManager.Instance.uid))
//        {
//            Debug.LogError("We need a username and appId to login");
//            return;
//        }

//        rtmClient.Login(_token, PlayerManager.Instance.uid);
//    }
//    // Log out
//    public void Logout()
//    {
//        Debug.Log(PlayerManager.Instance.uid + " logged out of the rtm " + Message.MessageType.Info);
//        rtmClient.Logout();
//    }

//    // Join the channel
//    public void JoinChannel()
//    {

//        channel = rtmClient.CreateChannel(_channelName, channelEventHandler);
//        ShowCurrentChannelName();
//        int success = channel.Join();
//        Debug.Log("JoinChannel Success: " + success);
//    }
//    // leave the channel
//    public void LeaveChannel()
//    {
//        Debug.Log(PlayerManager.Instance.uid + " left the chat " + Message.MessageType.Info);
//        channel.Leave();
//    }
//    // Send channel message
//    public void SendMessageToChannel()
//    {
//        string msg = "TEST MESSAGE";
//        string peer = "[channel:" + _channelName + "]";

//        string displayMsg = string.Format("{0}->{1}: {2}", PlayerManager.Instance.uid, peer, msg);

//        Debug.Log(displayMsg + " " + Message.MessageType.PlayerMessage);
//        channel.SendMessage(rtmClient.CreateMessage(msg));
//    }
//    // Send peer-to-peer message
//    //public void SendPeerMessage()
//    //{
//    //    string msg = peerMessageBox.text;
//    //    string peer = peerUserBox.text;

//    //    string displayMsg = string.Format("{0}->{1}: {2}", UserName, peer, msg);
//    //    messageDisplay.AddTextToDisplay(displayMsg, Message.MessageType.PlayerMessage);

//    //    rtmClient.SendMessageToPeer(
//    //        peerId: peer,
//    //        message: rtmClient.CreateMessage(msg),
//    //        options: _MessageOptions
//    //    );

//    //    peerMessageBox.text = "";
//    //}



//    void ShowCurrentChannelName()
//    {

//        Debug.Log("Channel name is " + _channelName);
//    }


//    void OnQueryPeersOnlineStatusResultHandler(int id, long requestId, PeerOnlineStatus[] peersStatus, int peerCount, QUERY_PEERS_ONLINE_STATUS_ERR errorCode)
//    {
//        if (peersStatus.Length > 0)
//        {
//            Debug.Log("OnQueryPeersOnlineStatusResultHandler requestId = " + requestId +
//            " peersStatus: peerId=" + peersStatus[0].peerId +
//            " online=" + peersStatus[0].isOnline +
//            " onlinestate=" + peersStatus[0].onlineState);
//            Debug.Log("User " + peersStatus[0].peerId + " online status = " + peersStatus[0].onlineState + Message.MessageType.Info);
//        }
//    }
//    // Callback when joining a channel successfully
//    void OnJoinSuccessHandler(int id)
//    {
//        string msg = "channel:" + _channelName + " OnJoinSuccess id = " + id;
//        Debug.Log(msg);

//    }
//    // Callback when failed to join a channel
//    void OnJoinFailureHandler(int id, JOIN_CHANNEL_ERR errorCode)
//    {
//        string msg = "channel OnJoinFailure  id = " + id + " errorCode = " + errorCode;
//        Debug.Log(msg);
//    }
//    // Callback when log in successfully
//    void OnClientLoginSuccessHandler(int id)
//    {
//        string msg = "client login successful! id = " + id;
//        Debug.Log(msg);
//    }
//    // Callback when failed to log in
//    void OnClientLoginFailureHandler(int id, LOGIN_ERR_CODE errorCode)
//    {
//        string msg = "client login unsuccessful! id = " + id + " errorCode = " + errorCode;
//        Debug.Log(msg);
//    }
//    // Callback when leave the channel
//    void OnLeaveHandler(int id, LEAVE_CHANNEL_ERR errorCode)
//    {
//        string msg = "client onleave id = " + id + " errorCode = " + errorCode;
//        Debug.Log(msg);
//    }
//    // Callback when receiving a channel message
//    void OnChannelMessageReceivedHandler(int id, string userId, TextMessage message)
//    {
//        Debug.Log("client OnChannelMessageReceived id = " + id + ", from user:" + userId + " text:" + message.GetText());
//    }
//    // Callback when receiving a peer-to-peer message
//    void OnMessageReceivedFromPeerHandler(int id, string peerId, TextMessage message)
//    {
//        Debug.Log("client OnMessageReceivedFromPeer id = " + id + ", from user:" + peerId + " text:" + message.GetText());
//    }
//    // Callback when a remote member joins the channel
//    void OnMemberJoinedHandler(int id, RtmChannelMember member)
//    {
//        string msg = "channel OnMemberJoinedHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
//        Debug.Log(msg);
//    }
//    // Callback when a remote member leaves the channel
//    void OnMemberLeftHandler(int id, RtmChannelMember member)
//    {
//        string msg = "channel OnMemberLeftHandler member ID=" + member.GetUserId() + " channelId = " + member.GetChannelId();
//        Debug.Log(msg);
//    }

//    // Returns the result of the `SendMessage` method call

//    void OnSendMessageResultHandler(int id, long messageId, PEER_MESSAGE_ERR_CODE errorCode)
//    {
//        string msg = string.Format("Sent message with id:{0} MessageId:{1} errorCode:{2}", id, messageId, errorCode);
//        Debug.Log(msg);
//    }

//    // Callback when the connection state changes between the SDK and the Agora <Vg k="MESS" /> system
//    void OnConnectionStateChangedHandler(int id, CONNECTION_STATE state, CONNECTION_CHANGE_REASON reason)
//    {
//        string msg = string.Format("connection state changed id:{0} state:{1} reason:{2}", id, state, reason);
//        Debug.Log(msg);
//    }
//}
