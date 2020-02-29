

using Assets.Scripts;
using Assets.Scripts.Cells;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.Hexes
{
    public class CameraExtensionHex : BaseHexType
    {
        public CameraExtensionHex(Position position, int layer) : base(position, layer)
        {
        }
        public override TileBase GetTile()
        {
            return LevelManager.instance.GetHexType(Constants.CAMERA_EXTENSION_HEX);
        }

        public override bool isDestoyeble()
        {
            return false;
        }
        public override bool IsPassable()
        {
            return false;
        }
    }
}
