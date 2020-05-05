using System.Collections;
using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using Assets._Scripts.Devtodev_analytic;
using UnityEngine;

public class Advertisments : MonoBehaviour, IPermissionGrantedListener, IInterstitialAdListener, IBannerAdListener,
    IMrecAdListener, INonSkippableVideoAdListener, IRewardedVideoAdListener
{
    public bool consentValue;
    public bool isTest;

#if UNITY_EDITOR && !UNITY_ANDROID && !UNITY_IPHONE
        string appKey = "";
#elif UNITY_ANDROID
    string appKey = "02294551c5c911265196876d5c33caecebc451b46c2b1763";
#endif
    void Start()
    {

#if UNITY_ANDROID&&!UNITY_EDITOR
        Init();
        if (PlayerPrefs.HasKey("first_launch"))
        {
           ShowInterstitialOnLoad();
        }
        else
        {
            Debug.LogError("no_first_launch");
        }
#endif
    }

    public void ShowInterstitialOnLoad()
    {
        StartCoroutine(ShowInterstitialCoroutine());
    }
    public void ShowInterstitial()
    {
            Appodeal.show(Appodeal.INTERSTITIAL);
            //Appodeal.setInterstitialCallbacks(this);
            AdvertismentAnalytic.InterstitialShowRequest();
    }

    IEnumerator ShowInterstitialCoroutine()
    {
        for (int i = 0; i < 30; i++)
        {
            if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
            {
                Appodeal.show(Appodeal.INTERSTITIAL);
               // Appodeal.setInterstitialCallbacks(this);
                AdvertismentAnalytic.InterstitialShowRequest();
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void Init()
    {
        Appodeal.setInterstitialCallbacks(this);
        Debug.LogError("Initializing");
        Appodeal.setLogLevel(Appodeal.LogLevel.Verbose);
        Appodeal.setTesting(isTest);
        Appodeal.setAutoCache(Appodeal.INTERSTITIAL, true);
        // Appodeal.setUserId("1");
        // Appodeal.setUserAge(25);
        // Appodeal.setUserGender(UserSettings.Gender.FEMALE);
        Appodeal.disableLocationPermissionCheck();
        Appodeal.disableWriteExternalStoragePermissionCheck();
        Appodeal.setTriggerOnLoadedOnPrecache(Appodeal.INTERSTITIAL, true);
        Appodeal.setSmartBanners(true);
        Appodeal.setBannerAnimation(true);
        Appodeal.setTabletBanners(true);
        Appodeal.setBannerBackground(true);
        Appodeal.setChildDirectedTreatment(false);
        Appodeal.muteVideosIfCallsMuted(true);
      //  Appodeal.setAutoCache(Appodeal.INTERSTITIAL, false);
        Appodeal.setExtraData(ExtraData.APPSFLYER_ID, "1527256526604-2129416");
        Appodeal.initialize(appKey,
            Appodeal.INTERSTITIAL | Appodeal.BANNER_VIEW | Appodeal.REWARDED_VIDEO | Appodeal.MREC, false);
        Debug.LogError("Success");
        /* Appodeal.setBannerCallbacks(this);
         Appodeal.setInterstitialCallbacks(this);
         Appodeal.setRewardedVideoCallbacks(this);
         Appodeal.setMrecCallbacks(this);

         Appodeal.setSegmentFilter("newBoolean", true);
         Appodeal.setSegmentFilter("newInt", 1234567890);
         Appodeal.setSegmentFilter("newDouble", 123.123456789);
         Appodeal.setSegmentFilter("newString", "newStringFromSDK");*/
    }
#region interfaces
    public void accessCoarseLocationResponse(int result)
    {
        throw new System.NotImplementedException();
    }

    public void onBannerClicked()
    {
        throw new System.NotImplementedException();
    }

    public void onBannerExpired()
    {
        throw new System.NotImplementedException();
    }

    public void onBannerFailedToLoad()
    {
        throw new System.NotImplementedException();
    }

    public void onBannerLoaded(int height, bool isPrecache)
    {
        throw new System.NotImplementedException();
    }

    public void onBannerShown()
    {
        throw new System.NotImplementedException();
    }

    public void onInterstitialClicked()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            AdvertismentAnalytic.OnInterstitialClicked();

        });       
    }

    public void onInterstitialClosed()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            AdvertismentAnalytic.OnInterstitialClosed();
        });
    }

    public void onInterstitialExpired()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            AdvertismentAnalytic.OnInterstitialExpired();
        });
    }

    public void onInterstitialFailedToLoad()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            AdvertismentAnalytic.OnInterstitialFailedToLoad();
        });
    }

    public void onInterstitialLoaded(bool isPrecache)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            AdvertismentAnalytic.OnInterstitialLoaded(isPrecache);
        });
    }

    public void onInterstitialShowFailed()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            AdvertismentAnalytic.OnInterstitialShowFailed();
        });
    }

    public void onInterstitialShown()
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => {
            AdvertismentAnalytic.OnInterstitialShow();
        });
    }

    public void onMrecClicked()
    {
        throw new System.NotImplementedException();
    }

    public void onMrecExpired()
    {
        throw new System.NotImplementedException();
    }

    public void onMrecFailedToLoad()
    {
        throw new System.NotImplementedException();
    }

    public void onMrecLoaded(bool isPrecache)
    {
        throw new System.NotImplementedException();
    }

    public void onMrecShown()
    {
        throw new System.NotImplementedException();
    }

    public void onNonSkippableVideoClosed(bool finished)
    {
        throw new System.NotImplementedException();
    }

    public void onNonSkippableVideoExpired()
    {
        throw new System.NotImplementedException();
    }

    public void onNonSkippableVideoFailedToLoad()
    {
        throw new System.NotImplementedException();
    }

    public void onNonSkippableVideoFinished()
    {
        throw new System.NotImplementedException();
    }

    public void onNonSkippableVideoLoaded(bool isPrecache)
    {
        throw new System.NotImplementedException();
    }

    public void onNonSkippableVideoShowFailed()
    {
        throw new System.NotImplementedException();
    }

    public void onNonSkippableVideoShown()
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoClicked()
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoClosed(bool finished)
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoExpired()
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoFailedToLoad()
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoFinished(double amount, string name)
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoLoaded(bool precache)
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoShowFailed()
    {
        throw new System.NotImplementedException();
    }

    public void onRewardedVideoShown()
    {
        throw new System.NotImplementedException();
    }

    public void writeExternalStorageResponse(int result)
    {
        throw new System.NotImplementedException();
    }
#endregion

}
