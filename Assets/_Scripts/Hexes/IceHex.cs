using UnityEngine;
using UnityEditor;
using Assets.Scripts.Cells;
using UnityEngine.Tilemaps;
using Assets.Scripts;
using Assets._Scripts.FakeHexes;

public class IceHex : BaseHexType
{
	public IceHex(Position position, int layer) : base(position, layer)
	{
	}
	public override TileBase GetTile()
	{
		return LevelManager.instance.GetHexType(Constants.ICE_HEX);
	}

	public override bool isDestoyeble()
	{
		return false;
	}

	Position lastPosition;
	public override void OnEnterHexEvent()
	{
		((MoveHero)MoveHero.instance).EndMove -= OnEnterHexEvent;


        var currentPosition = Position;
		var newPosition = PositionCalculator.GetOppositeSidePosition(lastPosition, currentPosition);
        OnLeaveHex(newPosition);
        LevelManager.instance.TryGetHex(newPosition, 0, out var hex);
        if (hex != null)
        {
            if (hex.GetType() != typeof(IceHex))
            {
                ((MoveHero)MoveHero.instance).UnlockInput();
            }
        }
        //((MoveHero)MoveHero.instance).UnlockInput();
        }

	public override void OnEnterHex(Position previousCoordinate)
	{
		((MoveHero)MoveHero.instance).LockInput();
        ((MoveHero)MoveHero.instance).SetNextPosition();
    
        ((MoveHero)MoveHero.instance).EndMove += OnEnterHexEvent;
        ((MoveHero)MoveHero.instance).Move(previousCoordinate,Position, 1f,false);
		lastPosition = previousCoordinate;
	}
	public override void OnLeaveHex(Position nextHex)
    {
        LevelManager.instance.TryGetHex(nextHex, 0, out var hex);
        if (hex != null)
        {
            if (hex.IsPassable())
            {
                ((MoveHero)MoveHero.instance).EndMove += OnLeaveHexEvent;
              
                ((MoveHero)MoveHero.instance).Move(Position, nextHex, 0.5f, false);
                ((MoveHero)MoveHero.instance).nextPosition = nextHex;
            }
            else
            {
                ((MoveHero)MoveHero.instance).Idle();
                ((MoveHero)MoveHero.instance).EndMove -= OnEnterHexEvent;
                ((MoveHero)MoveHero.instance).EndMove -= OnLeaveHexEvent;
                LevelManager.instance.SelectCells(Position);
            }
        }
        else
        {
            ((MoveHero)MoveHero.instance).EndMove += OnLoseEvent;
            ((MoveHero)MoveHero.instance).Move(Position, nextHex, 2f, false);
        }
    }

    public void OnLoseEvent()
    {

        ((MoveHero)MoveHero.instance).EndMove -= OnLoseEvent;
        ((MoveHero) MoveHero.instance).fallEndedAction += OnEndFall;
       ((MoveHero)MoveHero.instance).FallHero();     
    }

    public void OnEndFall()
    {
        ((MoveHero)MoveHero.instance).fallEndedAction -= OnEndFall;
        WinLoseManager.instance.OnLose();
    }

    public override void OnLeaveHexEvent()
    {
       // LeaveHexEvent?.Invoke(((MoveHero)MoveHero.instance).nextPosition, Position);
        ((MoveHero)MoveHero.instance).EndMove -= OnLeaveHexEvent;
       // Debug.LogError("onLeave otp");
        var levelManager = GameObject.FindObjectOfType<LevelManager>();
        levelManager.TryGetHex(((MoveHero)MoveHero.instance).nextPosition, 0, out var hex);
        if (hex == null)
        {
            WinLoseManager.instance.OnLose();
            return;
        }
        ((MoveHero)MoveHero.instance).SetHeroPosition(((MoveHero)MoveHero.instance).nextPosition, false);
        hex.OnEnterHex(Position);
    }

	public override void OnLaserHit(Position previousPosition,int rangeInAir,int range)
    {
        Position[] laserPositions = PositionCalculator.GetLaserPositions(previousPosition, Position);
        foreach (var position in laserPositions)
        {
            LaserGenerator.instance.DrawLaser(Position, position,rangeInAir,range);
        }
    }
    public override BaseFakeHexType GetFakeHex()
    {
        return new FakeIceHex(Position, Layer);
    }
}