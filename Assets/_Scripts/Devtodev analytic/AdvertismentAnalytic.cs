using DevToDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Devtodev_analytic
{
    public static class AdvertismentAnalytic
    {
        public static void InterstitialShowRequest()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_show_request");
            DevToDev.Analytics.CustomEvent("interstitial_show_request");
        }

        public static void OnInterstitialClicked()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_clicked");
            DevToDev.Analytics.CustomEvent("interstitial_clicked");
        }

        public static void OnInterstitialClosed()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_closed");
            DevToDev.Analytics.CustomEvent("interstitial_closed");
        }

        public static void OnInterstitialExpired()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_expired");
            DevToDev.Analytics.CustomEvent("interstitial_expired");
        }

        public static void OnInterstitialFailedToLoad()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_failed_to_load");
            DevToDev.Analytics.CustomEvent("interstitial_failed_to_load");
        }

        public static void OnInterstitialLoaded(bool isPrecache)
        {
            CustomEventParams customParams = new DevToDev.CustomEventParams();
            customParams.AddParam("isPrecache", isPrecache.ToString());
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_loaded");
            DevToDev.Analytics.CustomEvent("interstitial_loaded",customParams);
        }

        public static void OnInterstitialShowFailed()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_show_failed");
            DevToDev.Analytics.CustomEvent("interstitial_show_failed");
        }

        public static void OnInterstitialShow()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>interstitial_show");
            DevToDev.Analytics.CustomEvent("interstitial_show");
        }

        public static void OnBannerClicked()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>on_banner_clicked");
            DevToDev.Analytics.CustomEvent("on_banner_clicked");
        }
        public static void OnBannerExpired()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>on_banner_expired");
            DevToDev.Analytics.CustomEvent("on_banner_expired");
        }
        public static void OnBannerFailedToLoad()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>on_banner_failed");
            DevToDev.Analytics.CustomEvent("on_banner_failed");
        }
        public static void OnBannerShown()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>on_banner_show");
            DevToDev.Analytics.CustomEvent("on_banner_show");
        }
        public static void OnBannerLoaded(bool isPrecache)
        {
            CustomEventParams customParams = new DevToDev.CustomEventParams();
            customParams.AddParam("isPrecache", isPrecache.ToString());
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>banner_loaded");
            DevToDev.Analytics.CustomEvent("banner_loaded", customParams);
        }

        public static void OnBannerRequest()
        {
            Debug.LogError("<color=yellow>[AdvertismentAnalytic]</color>banner_show_request");
            DevToDev.Analytics.CustomEvent("banner_show_request");
        }
    }
}
