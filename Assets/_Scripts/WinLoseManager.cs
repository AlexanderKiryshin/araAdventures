using Assets.Scripts;
using UnityEngine.Events;

public class WinLoseManager : Singleton<WinLoseManager>
{
	public LevelRestarter restart;
	public UnityEvent loseEvent;
	public void Start()
	{
		loseEvent = new UnityEvent();
		loseEvent.AddListener(OnLose);
	}
	public void OnLose()
	{
		restart.RestartLevel();
	}
}