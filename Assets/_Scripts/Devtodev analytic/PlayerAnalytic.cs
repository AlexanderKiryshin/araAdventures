using DevToDev;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Assets._Scripts.DevtodevAnalytic
{
    public static class PlayerAnalytic
    {
        public static void SecondButtonConfidentialClick()
        {
           // CustomEventParams customParams = new DevToDev.CustomEventParams();

           // customParams.AddParam("second_confidential_button_clicked", "true");
            Debug.LogError("<color=yellow>[PlayerAnalytic]</color>second_confidential_button_clicked");
            DevToDev.Analytics.CustomEvent("second_confidential_button_clicked");
        }

        public static void FirstButtonConfidentialClick()
        {
           // CustomEventParams customParams = new DevToDev.CustomEventParams();

            Debug.LogError("<color=yellow>[PlayerAnalytic]</color>first_confidential_button_clicked");
            DevToDev.Analytics.CustomEvent("first_confidential_button_clicked");
        }

        public static void FirstScreenConfidentialConfirm()
        {
           // CustomEventParams customParams = new DevToDev.CustomEventParams();

          //  customParams.AddParam("first_screen_confidential_confirm", "true");
            Debug.LogError("<color=yellow>[PlayerAnalytic]</color>first_screen_confidential_confirm");
            DevToDev.Analytics.CustomEvent("first_screen_confidential_confirm");
        }

        public static void SendPlayerData(bool isMan, int age)
        {
            CustomEventParams customParams = new DevToDev.CustomEventParams();
           
            customParams.AddParam("IsMan", isMan.ToString());
            customParams.AddParam("Age", age);
            Debug.LogError("<color=yellow>[PlayerAnalytic]</color>Player_info,isMan=" + isMan.ToString()+",Age="+age);
            DevToDev.Analytics.CustomEvent("Player_info", customParams);
        }
    }
}
