using TMPro;
using UnityEngine;

public class MoneyVisualizer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyText;

    private void OnEnable()
    {
        UpdateMoney();
    }
    public void UpdateMoney()
    {
        if (PlayerPrefs.HasKey("money"))
        {
            moneyText.text = PlayerPrefs.GetInt("money").ToString();
        }
        else
        {
            moneyText.text = "0";
        }
    }
}
