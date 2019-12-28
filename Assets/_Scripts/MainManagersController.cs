using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Assets.Scripts;

[DefaultExecutionOrder((int) ExecutionOrderList.MainManager)]
public class MainManagersController : Singleton<MainManagersController>
{
    List<ISubscibable> SubscribeManagers;
    List<IManagerInitializable> InitManagers;
    List<IFinishManagerInitialize> InvokableManagers;
    public Action OnFinishInitialize;
    int prelaunchCount, totalCount, managerLvl;
    int[] arr;
    private int maxPriority;

    public void Awake()
    {
        StartApp();
    }

    public void StartApp()
    {
        managerLvl = 0;
        prelaunchCount = 0;
        InvokableManagers = FindObjectsOfType<MonoBehaviour>().OfType<IFinishManagerInitialize>().ToList();

        foreach (var invokableManager in InvokableManagers)
        {
            if (!invokableManager.IsFinishSubscribed)
            {
                invokableManager.SubscribeFinishAction(ref OnFinishInitialize);
                invokableManager.IsFinishSubscribed = true;
            }
        }

        SubscribeManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISubscibable>().ToList();
        InitManagers = FindObjectsOfType<MonoBehaviour>().OfType<IManagerInitializable>().ToList();
        foreach (var manager in InitManagers)
        {
            if (manager.GetPriority() > maxPriority)
            {
                maxPriority = manager.GetPriority();
            }
               
        }
        foreach (var m in SubscribeManagers)
        {
            if (!m.IsSubscribed)
            {

                m.Subscribe();
                m.IsSubscribed = true;
            }
        }

        StartInvokeLevelManagers(0);
    }

    public void AddSubscribe(IManagerInitializable manager)
    {
        if (InitManagers == null)
        {
            InitManagers = new List<IManagerInitializable>();
        }

        InitManagers.Add(manager);
    }

    public void StartNextLvl(int lvl)
    {

        if (lvl < maxPriority)
        {
            lvl += 1;
            //  Debug.Log("START INVOKING " + lvl);
            StartInvokeLevelManagers(lvl);
        }
        else
        {
            OnFinishInitialize?.Invoke();
            OnFinishInitialize = null;
        }
    }

    public void CheckToInitialize()
    {
        prelaunchCount++;
        if (prelaunchCount == totalCount)
        {
            StartNextLvl(managerLvl);
        }
    }


    public void StartInvokeLevelManagers(int lvl)
    {
        prelaunchCount = 0;
        managerLvl = lvl;
        List<IManagerInitializable> managers = new List<IManagerInitializable>();
        foreach (var s in InitManagers)
        {
            if (!s.IsInitialized && s.GetPriority() == lvl)
            {
                managers.Add(s);
            }
        }

        if (managers.Count == 0)
        {
            StartNextLvl(lvl);
            return;
        }

        totalCount = managers.Count;

        foreach (var s in managers)
        {
            s.SubscribePreLaunch(CheckToInitialize);
            s.InvokeController();
            s.IsInitialized = true;
        }
    }
}

public interface IFinishManagerInitialize
{
    void SubscribeFinishAction (ref  Action action);
    bool IsFinishSubscribed { get; set; }
}
public interface IManagerInitializable
{
    int GetPriority();
    void InvokeController();
    void SubscribePreLaunch(Action action); 
	bool IsInitialized { get; set; }
}

public interface ISubscibable
{
    void Subscribe();
    bool IsSubscribed { get; set; }
}

enum ExecutionOrderList
{
    PreLaunchManagers=0,
    MapPresenter = 3,
	GameZoneController=4,
    DataLoader =5,  
    MainManager = 10,

}
 
public enum ExecutionManagerPriority
{
    First=0,
    Second=1,
    Third=2,
    fourth=3,
     
}

 

