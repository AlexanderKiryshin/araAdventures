using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Assets._Scripts.Analytics
{
    public class AppEventManager:MonoBehaviour
    {
        private float secondsElapsed = 0;

        void Awake()
        {
            AnalyticsEvent.Custom("StartAPP");
        }

        void Update()
        {
            secondsElapsed += Time.deltaTime;
        }

        void OnDestroy()
        {
            Dictionary<string, object> customParams = new Dictionary<string, object>();
            customParams.Add("minutes_played", secondsElapsed / ((float)60));
            customParams.Add("gender",PlayerPrefs.GetInt("Gender"));
            customParams.Add("age", PlayerPrefs.GetInt("Age"));
            if (PlayerPrefs.HasKey("first_launch"))
            {               
                AnalyticsEvent.Custom("second_launches_session_time", customParams);
            }
            else
            {
                PlayerPrefs.SetString("first_launch","first");
                AnalyticsEvent.Custom("first_launch_session_time", customParams);
            }
          
        }
    }
}
