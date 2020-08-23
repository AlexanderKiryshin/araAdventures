using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts
{
    public class HPController:MonoBehaviour
    {
        public const int SECOND_TO_RESTORE = 1800;
        public Action onHearthEnded;
        public List<GameObject> hearths;
        public void Awake()
        {
            if (PlayerPrefs.HasKey("CountHearth"))
            {
                if (PlayerPrefs.GetInt("CountHearth")>0)
                {

                }
                else
                {
                    
                }

                for (int i = 0; i < PlayerPrefs.GetInt("CountHearth"); i++)
                {
                    hearths[i].SetActive(true);
                }
            }
            else
            {
                PlayerPrefs.SetInt("CountHearth", 3);
            }

        }

        public void LoseHearth()
        {
            if (PlayerPrefs.HasKey("CountHearth"))
            {
                int countHearth=PlayerPrefs.GetInt("CountHearth");
                if (countHearth == 1)
                {
                    PlayerPrefs.SetInt("CountHearth",0);
                    SaveDate();
                    onHearthEnded?.Invoke();
                }
            }
            else
            {
                PlayerPrefs.SetInt("CountHearth", 2);
            }
        }

        public void SaveDate()
        {
            if (!PlayerPrefs.HasKey("timeRestoreHealth"))
            {
                DateTime dt = TimeZoneInfo.ConvertTimeToUtc(DateTime.Now);

                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                TimeSpan tsInterval = dt.Subtract(dt1970);

                Int32 iSeconds = Convert.ToInt32(tsInterval.TotalSeconds);

                PlayerPrefs.SetInt("timeRestoreHealth", iSeconds);
            }
        }
        public void AddHearth()
        {
            if (PlayerPrefs.HasKey("timeRestoreHealth"))
            {
                int seconds = PlayerPrefs.GetInt("timeRestoreHealth");
                DateTime tsInterval = Convert.ToDateTime(seconds);
            }
        }

        public void Update()
        {

        }
    }
}
