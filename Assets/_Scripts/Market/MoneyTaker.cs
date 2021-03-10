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
    private MoneyVisualizer[] moneyVisualizer;
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
        if (!PlayerPrefs.HasKey(SceneManager.GetActiveScene().name))
        {
            if (PlayerPrefs.HasKey("progress"))
            {
                int progress = PlayerPrefs.GetInt("progress");
                PlayerPrefs.SetInt("progress", progress + 1);
            }
            else
            {
                PlayerPrefs.SetInt("progress", 1);
            }
        }
        PlayerPrefs.SetString(SceneManager.GetActiveScene().name, "");
        winText.gameObject.SetActive(true);
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
 
        int randomMoney = Random.Range(5, 11);
        
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
        foreach (var moneyVis in moneyVisualizer)
        {
            moneyVis.UpdateMoney();
        }
        
        return true;
    }
   IEnumerator CollectMoneyCoroutine(int money)
    {
        int currentMoney = 0;
        if (PlayerPrefs.HasKey("money"))
        {
            currentMoney = PlayerPrefs.GetInt("money");
        }
        currentMoney += money;
        PlayerPrefs.SetInt("money", currentMoney);
        for (int i=0;i<21;i++)
        {
            winText.text = ((int)(money * i / 20)).ToString();
            yield return new WaitForSeconds(0.1f);
        }
        SetMoneyPosition();
        yield return new WaitForSeconds(0.8f);
        
        foreach (var moneyVis in moneyVisualizer)
        {
            moneyVis.UpdateMoney();
        }
         winText.gameObject.SetActive(false);
         SetStartPosition();
    }

    private void SetStartPosition()
    {
       winText.transform.DOLocalMove(new Vector3(0, 0, 0), 0.25f);
    }
    private void SetMoneyPosition()
    {
        winText.transform.DOLocalMove(new Vector3(-660,1330, 0), 1f);
    }
}
