using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shopCanvas;

    public void OnClick()
    {
        shopCanvas.SetActive(true);
    }

    public void Close()
    {
        shopCanvas.SetActive(false);
    }
}
