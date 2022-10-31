
using UnityEngine.Networking;
using System;

using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;
using System.Collections;
using System.Linq;


#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
 using UnityEngine.Android;
#endif

public class TokenStruct
{
    public string rtcToken;
}

public class AgoraManager : MonoBehaviour
{
    private string serverUrl = "https://agora-token-service-production-aa7d.up.railway.app"; // The base URL to your token server. For example, https://app-name.herokuapp.com"
    private int ExpireTime = 60*60*24; //Expire time in Seconds.
    private string uid = "10"; // An integer that identifies the user.

    // Fill in your app ID.
    private string _appID = "1123e853e8ac4062a601cbb76526c742";
    // Fill in your channel name.
    private string _channelName = "CardsAR01";
    // Fill in the temporary token you obtained from Agora Console.
    private string _token = "";
    internal IRtcEngine RtcEngine;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
private ArrayList permissionList = new ArrayList() { Permission.Microphone };
#endif

    // Fetches the <Vg k="VSDK" /> token
    IEnumerator FetchToken(string url, string channel, string userId, int TimeToLive, Action<string> callback = null)
    {
        string request_string = string.Format("https://agora-token-service-production-aa7d.up.railway.app/rtc/CardsAR01/1/uid/{0}/?expiry=86400", userId);
        UnityWebRequest request = UnityWebRequest.Get(request_string);

        Debug.Log("request_string");
        Debug.Log(request_string);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
            callback(null);
            yield break;
        }
        TokenStruct tokenInfo = JsonUtility.FromJson<TokenStruct>(request.downloadHandler.text);
        Debug.Log("tokenInfo");
        Debug.Log(tokenInfo.rtcToken);
        callback(tokenInfo.rtcToken);
    }
    void FetchRenew(string newToken)
    {
        // Update RTC Engine with new token, which will not expire so soon
        RtcEngine.RenewToken(newToken);
        _channelName = newToken;
        Debug.Log(newToken);
    }

    private void Awake()
    {
        //take the system string identifier, extract the numbers to get a unique int ID
        string deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
        string deviceUniqueIdentifier_int = new String(deviceUniqueIdentifier.Where(Char.IsDigit).ToArray());
        this.uid = deviceUniqueIdentifier_int.Substring(0, 8);
    }


    // Start is called before the first frame update
    void Start()
    {
        SetupVoiceSDKEngine();
        InitEventHandler();
        SetupUI();
    }


    void Update()
    {
        CheckPermissions();
    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    foreach (string permission in permissionList)
    {
        if (!Permission.HasUserAuthorizedPermission(permission))
        {
            Permission.RequestUserPermission(permission);
        }
    }
#endif
    }

    private void SetupUI()
    {
        GameObject go = GameObject.Find("Leave Call");
        go.GetComponent<Button>().onClick.AddListener(Leave);
        go = GameObject.Find("Join Call");
        go.GetComponent<Button>().onClick.AddListener(Join);
    }


    private void SetupVoiceSDKEngine()
    {
        // Create an RtcEngine instance.
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();




        RtcEngineContext context = new RtcEngineContext(_appID, 0, 
        CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING,
        AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT);
        // Initialize RtcEngine.
        RtcEngine.Initialize(context);
    }


    private void InitEventHandler()
    {
        // Creates a UserEventHandler instance.
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }


    public void Join()
    { 
        StartCoroutine(FetchToken(serverUrl, _channelName, uid, ExpireTime, this.FetchRenew));
        // Enables the audio module.
        RtcEngine.EnableAudio();
        // Sets the user role ad broadcaster.
        RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // Joins a channel.
        Debug.Log("Token");
        Debug.Log(_token);
        Debug.Log("_channelName");
        Debug.Log(_channelName);
        var joinRet = RtcEngine.JoinChannel(_token, "CardsAR01", "", UInt32.Parse(this.uid));
        Debug.Log("joinRet");
        Debug.Log(joinRet);
    }

    public void Leave()
    {
        // Leaves the channel.
        RtcEngine.LeaveChannel();
        // Disable the audio modules.
        RtcEngine.DisableAudio();
    }

    //
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly AgoraManager _audioSample;

        internal UserEventHandler(AgoraManager audioSample)
        {
            _audioSample = audioSample;
        }

        // This callback is triggered when the local user joins the channel.
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("JOINED BITCH");
        }

        public override void OnTokenPrivilegeWillExpire(RtcConnection connection, string token)
        {
            // Retrieve a fresh token from the token server.
            _audioSample.StartCoroutine(_audioSample.FetchToken(_audioSample.serverUrl, _audioSample._channelName, _audioSample.uid, _audioSample.ExpireTime, _audioSample.FetchRenew));
            Debug.Log("Token Expired");
        }
    }



}
