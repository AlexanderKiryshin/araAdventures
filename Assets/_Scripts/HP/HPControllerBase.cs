using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Scripts.HP
{
    public abstract class HPControllerBase:MonoBehaviour
    {
        public const int SECOND_TO_RESTORE = 30;
        public Action onHearthEnded;
        public List<GameObject> hearths;
        protected bool isCounter;
      
        public string GetTime()
        {
            int seconds = SECOND_TO_RESTORE - GetSeconds();
            int minutes = Mathf.FloorToInt(seconds / 60);
            seconds = seconds - minutes * 60;
            return minutes + ":" + seconds;
        }
      /*  public void AddHearth()
        {
            if (PlayerPrefs.HasKey("timeRestoreHealth"))
            {
                int seconds = PlayerPrefs.GetInt("timeRestoreHealth");
                DateTime tsInterval = Convert.ToDateTime(seconds);
            }
        }*/

        public void Update()
        {
            if (isCounter)
            {
                RestoreHealth();
            }
        }

        public void SaveDate()
        {
            DateTime dt = DateTime.Now;

            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            TimeSpan tsInterval = dt.Subtract(dt1970);

            Int32 iSeconds = Convert.ToInt32(tsInterval.TotalSeconds);

            PlayerPrefs.SetInt("timeRestoreHealth", iSeconds);
            PlayerPrefs.Save();
        }

        protected int GetSeconds()
        {
            if (PlayerPrefs.HasKey("timeRestoreHealth"))
            {
                int seconds = PlayerPrefs.GetInt("timeRestoreHealth");
                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dt1970 = dt1970.AddSeconds(seconds);
               // Debug.LogError(dt1970);
                TimeSpan tsInterval = DateTime.Now.Subtract(dt1970);
                return Convert.ToInt32(tsInterval.TotalSeconds);
            }
            else
            {
                return 0;
            }
        }

        public int CountHeartes()
        {
            if (PlayerPrefs.HasKey("CountHearth"))
            {
                return PlayerPrefs.GetInt("CountHearth");
            }
            else
            {
                return 3;
            }
        }

        protected abstract void RestoreHealth();
    }
}
