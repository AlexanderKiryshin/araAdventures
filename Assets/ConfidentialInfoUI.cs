using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfidentialInfoUI : MonoBehaviour
{
    public GameObject confidentialInfo;
    public Button confirmConfInfo;
    public static Action onAgeConfirm;
    public GameObject ageScreen;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Confidential"))
        {
            confidentialInfo.SetActive(true);
        }

        onAgeConfirm += AgeConfirm;
        confirmConfInfo.onClick.AddListener(ConfirmConfidential);
    }

    public void ConfirmConfidential()
    {
        confidentialInfo.SetActive(false);
        ageScreen.SetActive(true);
    }

    public void AgeConfirm()
    {
        ageScreen.SetActive(false);
        PlayerPrefs.SetInt("Confidential",1);
    }
}
