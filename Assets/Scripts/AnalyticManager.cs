using UnityEngine;
//using Firebase;
//using Firebase.Analytics;
using System.Collections.Generic;
using AppsFlyerSDK;
using FlurrySDK;

public class AnalyticManager : MonoBehaviour
{
    public static AnalyticManager instance;
    const string APPFLYER_DEV_KEY = "BDt9xS3kmRET5qbDQgEMYk";
    #if UNITY_ANDROID
    private readonly string FLURRY_API_KEY = "R922T7N73MXVJ68J75KQ";
    #elif UNITY_IPHONE
    private readonly string FLURRY_API_KEY = "FLURRY_IOS_API_KEY";
    #endif

    #region UNITY CALLBACKS
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance.InitSdk();
        DontDestroyOnLoad(this.gameObject);
    }
#endregion

#region PRIVATE FUNCTIONS
    void InitSdk()
    {
        if (IsEditor()) return;

        AppsFlyer.initSDK(APPFLYER_DEV_KEY, null);
        AppsFlyer.startSDK();

        new Flurry.Builder()
                  .WithCrashReporting(true)
                  .WithLogEnabled(true)
                  .WithLogLevel(Flurry.LogLevel.VERBOSE)
                  .WithMessaging(true)
                  .Build(FLURRY_API_KEY);
    }
#endregion

#region PUBLIC FUNCTIONS

    public void LogEvent(string _eventName)
    {
        if (IsEditor()) return;

        //FirebaseAnalytics.LogEvent(_eventName);
        AppsFlyer.sendEvent(_eventName, null);
        Flurry.LogEvent(_eventName, null);
    }
    public void LogEvent(string _eventName, string eventParam1, string eventParam2)
    {
        if (IsEditor()) return;
        Dictionary<string, string> analyticParam = new Dictionary<string, string>();
        analyticParam.Add(eventParam1, eventParam2);
        AppsFlyer.sendEvent(_eventName, analyticParam);
        Flurry.LogEvent(_eventName, analyticParam);
    }

    private static bool IsEditor()
    {
#if UNITY_EDITOR
        return true;
#else
            return false;
#endif
    }
#endregion
}
