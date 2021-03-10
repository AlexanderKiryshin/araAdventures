using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shopCanvas;
    [SerializeField]
    private MoneyVisualizer moneyVisualizer;
    public void OnClick()
    {
        shopCanvas.SetActive(true);
    }

    public void Close()
    {
        shopCanvas.SetActive(false);
        moneyVisualizer.UpdateMoney();
    }
}
