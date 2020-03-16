using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticManager : Singleton<AnalyticManager>
{
    // Start is called before the first frame update
    void Start()
    {
        Analytics.enabled = true;
        AnalyticsEvent.GameStart();
    }

    public void LevelStart(string name)
    {
      /*  var customParams = new Dictionary<string, object>();
        customParams.Add("seconds_played", secondsElapsed);
        customParams.Add("points", score);
        customParams.Add("deaths", deaths);
        AnalyticsEvent.LevelStart(name);*/
    }

    public void LevelEnd(string name)
    {
        AnalyticsEvent.LevelComplete(name);
    }

    public void LevelFail(string name)
    {
        AnalyticsEvent.LevelFail(name);
    }

    public void LevelSkip(string name)
    {

    }
}
