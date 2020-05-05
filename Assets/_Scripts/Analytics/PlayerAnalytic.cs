using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Assets._Scripts.Analytics
{
    public static class PlayerAnalytic
    {
        public static void SendPlayerData(bool isMan, int age)
        {
            Dictionary<string, object> customParams = new Dictionary<string, object>();

            customParams.Add("isMan", isMan);
            customParams.Add("Age",age);
            AnalyticsEvent.Custom("Player_info", customParams);         
        }
    }
}
