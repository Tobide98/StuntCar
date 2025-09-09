using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookInitScript : MonoBehaviour
{
    void Awake()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }
    }

    public void LogAchieveLevelEvent(string level)
    {
        var parameters = new Dictionary<string, object>();
        parameters[AppEventParameterName.Level] = level;
        FB.LogAppEvent(AppEventName.AchievedLevel, null, parameters);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
