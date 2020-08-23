//using Assets._Scripts.DevtodevAnalytic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfidentialInfoUI : MonoBehaviour
{
    public GameObject confidentialInfo;
    public Button confirmConfInfo;
    public Button confInfoButton1;
    public Button confInfoButton2;
    public static Action onAgeConfirm;
    public GameObject ageScreen;
    // Start is called before the first frame update
    void Awake()
    {
        if (!PlayerPrefs.HasKey("Confidential"))
        {
            confidentialInfo.SetActive(true);
        }
        confInfoButton1.onClick.AddListener(ShowConfidentialInfo);
    //    confInfoButton1.onClick.AddListener(PlayerAnalytic.FirstButtonConfidentialClick);

        confInfoButton2.onClick.AddListener(ShowConfidentialInfo);
      //  confInfoButton2.onClick.AddListener(PlayerAnalytic.SecondButtonConfidentialClick);
        onAgeConfirm += AgeConfirm;
        confirmConfInfo.onClick.AddListener(ConfirmConfidential);
    }

    public void ConfirmConfidential()
    {
        Debug.LogError("Click");
       // PlayerAnalytic.FirstScreenConfidentialConfirm();
        confidentialInfo.SetActive(false);
        ageScreen.SetActive(true);
    }

    public void AgeConfirm()
    {
       // ageScreen.SetActive(false);
        PlayerPrefs.SetInt("Confidential",1);
        SceneManager.LoadScene("level_1");
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }

    public void ShowConfidentialInfo()
    {
        Application.OpenURL("https://docs.google.com/document/d/1-NChTrwqa5_XFBGc8TQsuEG8dRtp52_-7akpwz-BM6o/export?format=pdf");
    }
}
