using UnityEngine;
using UnityEditor;
using Assets.Scripts.Cells;
using UnityEngine.Tilemaps;
using Assets.Scripts;

public class IceHex : BaseHexType
{
	public IceHex(Position position, int layer) : base(position, layer)
	{
	}
	public override TileBase GetTile()
	{
		return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.ICE_HEX);
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
		var levelManager = GameObject.FindObjectOfType<LevelManager>();
		levelManager.TryGetHex(newPosition, 0, out var hex);
		var moveHero = GameObject.FindObjectOfType<MoveHero>();
		if (hex == null)
		{
			WinLoseManager.instance.OnLose();
			return;
		}
		moveHero.SetHeroPosition(newPosition, false);
		hex.OnEnterHex(currentPosition);
		((MoveHero)MoveHero.instance).UnlockInput();
	}

	public override void OnEnterHex(Position previousCoordinate)
	{
		((MoveHero)MoveHero.instance).LockInput();
		((MoveHero)MoveHero.instance).EndMove += OnEnterHexEvent;
		((MoveHero)MoveHero.instance).Move(previousCoordinate,Position, 1f,false);
		lastPosition = previousCoordinate;
	}
	public override void OnLeaveHex(Position nextHex)
	{
		//((MoveHero)MoveHero.instance).Move(Position,nextHex, 0.5f,false);
	}

	public override void OnLaserHit(Position previousPosition,int rangeInAir,int range)
    {
        Position[] laserPositions = PositionCalculator.GetLaserPositions(previousPosition, Position);
        foreach (var position in laserPositions)
        {
            LaserGenerator.instance.DrawLaser(Position, position,rangeInAir,range);
        }
    }
}