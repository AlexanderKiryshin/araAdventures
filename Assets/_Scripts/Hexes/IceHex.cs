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

	public override void OnEnterHex(Position previousCoordinate)
	{
		var lastPosition = previousCoordinate;
		var currentPosition = Position;
		var newPosition = PositionCalculator.GetOppositeSidePosition(lastPosition, currentPosition);
		var levelManager = GameObject.FindObjectOfType<LevelManager>();
		levelManager.TryGetHex(newPosition,0,out var hex);
		var moveHero = GameObject.FindObjectOfType<MoveHero>();
		if(hex==null)
		{
			WinLoseManager.instance.OnLose();
			return;
		}
		moveHero.SetHeroPosition(newPosition,false);
		hex.OnEnterHex(currentPosition);
	}

	public override void OnLeaveHex()
	{
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