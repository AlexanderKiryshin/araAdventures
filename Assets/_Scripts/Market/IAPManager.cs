using System;
using System.Collections.Generic;
using Assets._Scripts.Devtodev_analytic;
using Assets._Scripts.Market;
using Assets.Scripts;
using UnityEngine;

/*public class IAPManager : Singleton<IAPManager>, IStoreListener
{
    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

    public PurchasesHandler purchaseRewardGiver;

    public Action<PurchaseID> purchaseSuccessful;
    public Action<string> purchaseFailed;
    public Action<string> purchaseInited;
    public Action restoreBegin;
    public Action restoreEnd;

    public static Dictionary<PurchaseID, Purchase> playMarketProductId = new Dictionary<PurchaseID, Purchase>
    {
        {PurchaseID.none,new Purchase("none",ProductType.Consumable)},
        {PurchaseID.c100,new Purchase( "C100",ProductType.Consumable)},
        {PurchaseID.c250,new Purchase( "C250",ProductType.Consumable)},
        {PurchaseID.c500,new Purchase( "C500",ProductType.Consumable)},
        {PurchaseID.c1000,new Purchase( "C1000",ProductType.Consumable)},
        {PurchaseID.noads,new Purchase( "noads",ProductType.NonConsumable)}
    };

    void Start()
    {
        InitializePurchasing();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

#if UNITY_ANDROID

        foreach (var purchase in playMarketProductId.Values)
        {
            builder.AddProduct(purchase.productId, purchase.productType);
        }
#endif
        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProduct(PurchaseID productId)
    {
        // If Purchasing has been initialized ...
        if (IsInitialized())
        {
            playMarketProductId.TryGetValue(productId, out var productInfo);
            Product product = m_StoreController.products.WithID(productInfo.productId);

            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("<color=yellow>[IAP]</color> Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("<color=yellow>[IAP]</color> BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("<color=yellow>[IAP]</color> BuyProductID FAIL. Not initialized.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        // Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("<color=yellow>[IAP]</color> OnInitialized: PASS");

        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;

    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // CheckSubscriptions();
    }



    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("<color=yellow>[IAP]</color> OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        bool idIsFound = false;
        PurchaseID purchaseID = PurchaseID.none;
        foreach (var key in playMarketProductId.Keys)
        {
            if (playMarketProductId.TryGetValue(key, out var result))
            {
                if (result.productId == args.purchasedProduct.definition.id)
                {
                    purchaseID = key;
                    idIsFound = true;
                    break;
                }
            }
        }
        if (!idIsFound)
        {
            Debug.LogError("<color=yellow>[IAP]</color> Unexpected product ID");
            return PurchaseProcessingResult.Pending;
        }
        InAppPurchaseAnalytic.PurchaseInApp(purchaseID);

        purchaseRewardGiver.GivePurchaseReward(args.purchasedProduct.definition.id);

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        //LOCALIZATION
       // purchaseFailed.Invoke(LocalizationManager.Localize("_OopsPanel/" + failureReason));
        Debug.Log(string.Format("<color=yellow>[IAP]</color> OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
 }*/

