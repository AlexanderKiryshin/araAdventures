using UnityEngine;
using UnityEditor;
using Assets.Scripts.Cells;
using UnityEngine.Tilemaps;
using Assets.Scripts;

public class LaserHex : BaseHexType
{
	public LaserHex(Position position, int layer) : base(position, layer)
	{
	}

	/*public override void OnLeaveHex()
	{
	}*/

	public override TileBase GetTile()
	{
		return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.LASER_HEX);
	}

	public override bool isDestoyeble()
	{
		return false;
	}

	public override void OnEnterHex(Position previousCoordinate)
	{
        Position newPosition = PositionCalculator.GetOppositeSidePosition(previousCoordinate, Position);
		LaserGenerator.instance.DrawLaser(Position, newPosition,0,0);

	}
}