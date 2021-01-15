using System;
using Assets.Scripts;
using Assets._Scripts;
using Assets._Scripts.HP;
using UnityEngine.Events;

public class WinLoseManager : Singleton<WinLoseManager>
{
	public LevelRestarter restart;
	public Action loseEvent;
    public HPControllerInGame hpController;

    public void Awake()
    {
        loseEvent += OnLose;
    }
	public void OnLose()
	{
        hpController.LoseHearth();
	}

}