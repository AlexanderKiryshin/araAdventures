using System;
using System.Collections;
using System.Collections.Generic;
using Assets._Scripts.Analytics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfidentialDataCollector : MonoBehaviour
{
    public Toggle man;
    public Toggle woman;
    public TMP_InputField age;
    public Button confirm;
    private bool toggleActivated = false;

    private bool ageSetted = false;
    // Start is called before the first frame update
    void Start()
    {
        man.isOn = false;
        woman.isOn = false;
        man.onValueChanged.AddListener(OnManToggle);
        woman.onValueChanged.AddListener(OnWomanToggle);
        age.onValueChanged.AddListener(OnAgeChange);
        confirm.interactable = false;
        confirm.onClick.AddListener(OKButton);
    }

    void OnManToggle(bool value)
    {
        if (value)
        {
            woman.isOn = false;
            toggleActivated = true;
        }
        else
        {
            toggleActivated = false;
        }
        CheckReadyConfirm();
    }

    void OnWomanToggle(bool value)
    {
        if (value)
        {
            man.isOn = false;
            toggleActivated = true;
        }
        else
        {
            toggleActivated = false;
        }
        CheckReadyConfirm();
    }

    void OnAgeChange(string value)
    {
        if (value.Length > 0)
        {
            ageSetted = true;
        }
        else
        {
            ageSetted = false;
        }

        CheckReadyConfirm();
    }

    void CheckReadyConfirm()
    {
        if (toggleActivated && ageSetted)
        {
            confirm.interactable = true;
        }
        else
        {
            confirm.interactable = false;
        }
    }

    public void OKButton()
    {
        PlayerAnalytic.SendPlayerData(man.isOn,Convert.ToInt32(age.text));
        ConfidentialInfoUI.onAgeConfirm?.Invoke();
    }
}
