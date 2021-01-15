using UnityEngine;
using UnityEditor;
using DevToDev;

namespace Assets._Scripts.Devtodev_analytic
{
    public static class InAppPurchaseAnalytic
    {
        public static void PurchaseInApp(PurchaseID purchase)
        {
            Debug.LogError("<color=yellow>[InAppPurchaseAnalytic]</color>in_app_purchase_"+purchase.ToString());
            DevToDev.Analytics.CustomEvent("in_app_purchase_" + purchase.ToString());
        }

        public static void OnFailedPurchase(string failedReason)
        {
            CustomEventParams customParams = new DevToDev.CustomEventParams();

            customParams.AddParam("reason", failedReason);
            DevToDev.Analytics.CustomEvent("failed_purchase",customParams);
        }
    }
}