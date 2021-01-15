using UnityEngine;

//Holds all PurchaseBoxes, after purchase gives reward to the player
//TO DO: can need refactor
public class PurchasesHandler : MonoBehaviour
{
    [SerializeField]
    MoneyTaker moneyTaker;

     public bool GivePurchaseReward(string _purchaseID)
     {
        switch(_purchaseID)
        {
            case "C100":moneyTaker.CollectMoney(100); break;
            case "C250": moneyTaker.CollectMoney(250); break;
            case "C500": moneyTaker.CollectMoney(500); break;
            case "C1000": moneyTaker.CollectMoney(1000); break;
            case "noads": DisableAds(); break;
            default: return false;
        }
            return true;
     }

    private void DisableAds()
    {
        PlayerPrefs.SetString("noads", "noads");
    }
}
