using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyTaker : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI winText;
    [SerializeField]
    private MoneyVisualizer moneyVisualizer;
    [SerializeField]
    private bool getMoneyOnAwake = true;
    void OnEnable()
    {  
        if (!getMoneyOnAwake)
        {
            return;
        }
        string currentLevel = PlayerPrefs.GetString("current_level");
        if (PlayerPrefs.HasKey(currentLevel))
        {
            return;
        }
        winText.gameObject.SetActive(true);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
        if (PlayerPrefs.HasKey("progress"))
        {
            PlayerPrefs.SetInt("progress", PlayerPrefs.GetInt("progress") + 1);
        }
        int randomMoney = Random.Range(10, 20);
        
        StartCoroutine(CollectMoneyCoroutine(randomMoney));
    }
    public void CollectMoney(int money)
    {
        winText.gameObject.SetActive(true);
        StartCoroutine(CollectMoneyCoroutine(money));
    }

    public bool TrySpendMoney(int money)
    {
        int currentMoney = 0;
        if (PlayerPrefs.HasKey("money"))
        {
            currentMoney = PlayerPrefs.GetInt("money");
        }
        if (currentMoney<money)
        {
            return false;
        }
        currentMoney -= money;
        PlayerPrefs.SetInt("money", currentMoney);
        moneyVisualizer.UpdateMoney();
        return true;
    }
   IEnumerator CollectMoneyCoroutine(int money)
    {
        
        for (int i=0;i<21;i++)
        {
            winText.text = ((int)(money * i / 20)).ToString();
            yield return new WaitForSeconds(0.1f);
        }
        SetMoneyPosition();
        yield return new WaitForSeconds(0.8f);
         int currentMoney = 0;
         if (PlayerPrefs.HasKey("money"))
         {
             currentMoney = PlayerPrefs.GetInt("money");
         }
         currentMoney += money;
         PlayerPrefs.SetInt("money", currentMoney);
         moneyVisualizer.UpdateMoney();
         Debug.LogError("disable");
         winText.gameObject.SetActive(false);
         SetStartPosition();
    }

    private void SetStartPosition()
    {
       winText.transform.DOLocalMove(new Vector3(0, 0, 0), 0.25f);
        //winText.transform.localPosition = new Vector3(0, -23);
        Debug.LogError("yyyy"+winText.transform.localPosition);
    }
    private void SetMoneyPosition()
    {
        Debug.LogError("xxx"+winText.transform.localPosition);
        winText.transform.DOLocalMove(new Vector3(-660,1330, 0), 1f);
    }
}
