using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using Assets._Scripts.Devtodev_analytic;
using UnityEngine;

namespace Assets._Scripts.Ads
{
    public class AdsOnLevel:MonoBehaviour, IBannerAdListener
    {
        public void Start()
        {
            if (!PlayerPrefs.HasKey("noads"))
            {
                Appodeal.show(Appodeal.BANNER_BOTTOM);
                AdvertismentAnalytic.OnBannerRequest();
            }
        }

        public void onBannerClicked()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                AdvertismentAnalytic.OnBannerClicked();
            });
        }

        public void onBannerExpired()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                AdvertismentAnalytic.OnBannerExpired();
            });
        }

        public void onBannerFailedToLoad()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                AdvertismentAnalytic.OnBannerFailedToLoad();
            });
        }

        public void onBannerLoaded(int height, bool isPrecache)
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                AdvertismentAnalytic.OnInterstitialLoaded(isPrecache);
            });
        }

        public void onBannerShown()
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() => {
                AdvertismentAnalytic.OnBannerShown();
            });
        }
    }
}
