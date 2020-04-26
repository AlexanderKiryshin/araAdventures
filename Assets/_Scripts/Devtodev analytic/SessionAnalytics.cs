using DevToDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets._Scripts.Devtodev_analytic
{
    public class SessionAnalytics:MonoBehaviour
    {
        public string androidAppId;
        public string androidAppSecret;
        public bool logIsEnabled = true;
        public float secondsElapsed;

        public void Awake()
        {
            Debug.LogError("<color=yellow>[SessionAnalytic]</color>start session");
        }

        void Update()
        {
            secondsElapsed += Time.deltaTime;
        }

        void OnDestroy()
        {
            CustomEventParams customParams = new DevToDev.CustomEventParams();

            customParams.AddParam("seconds", secondsElapsed / ((float)60));

            if (PlayerPrefs.HasKey("first_launch"))
            {
                Debug.LogError("<color=yellow>[SessionAnalytic]</color>second_launch_session_time "+ secondsElapsed / ((float)60));
                DevToDev.Analytics.CustomEvent("second_launches_session_time",customParams);
            }
            else
            {
                Debug.LogError("<color=yellow>[SessionAnalytic]</color>first_launches_session_time " + secondsElapsed / ((float)60));
                PlayerPrefs.SetString("first_launch", "first");
                DevToDev.Analytics.CustomEvent("first_launch_session_time", customParams);
            }

        }
    }
}
