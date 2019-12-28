
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public class CreatingAroundHex : BaseHexType
    {
        public CreatingAroundHex(Position position, int layer) : base(position, layer)
        {
        }
        public override void OnLeaveHex()
        {
           // OnDestroyHex(Position, Layer);
        }

        public override TileBase GetTile()
        {
            return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.CREATING_AROUND_HEX); 
        }

        public override bool isDestoyeble()
        {
            return false;
        }


        public override void OnEnterHex(Position previousCoordinate)
        {
            var positions =PositionCalculator.GetAroundSidePositions(Position);
            foreach (var position in positions)
            {
                OnCreateHex(new NormalHex(position, Layer));
            }          
        }
        
  
    }
}
