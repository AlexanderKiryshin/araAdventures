using Assets._Scripts.FakeHexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public class CreatingAwayHex : BaseHexType
    {
        public CreatingAwayHex(Position position, int layer) : base(position, layer)
        {
        }
        /*public override void OnLeaveHex()
        {
            // OnDestroyHex(Position, Layer);
        }*/

        public override TileBase GetTile()
        {
            return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.CREATING_AWAY_HEX);
        }

        public override bool isDestoyeble()
        {
            return false;
        }

        public override void OnEnterHex(Position previousCoordinate)
        {
            base.OnEnterHex(previousCoordinate);
            var lastPosition = previousCoordinate;
            var currentPosition = Position;
            for (int i = 0; i < 3; i++)
            {
                var newPosition = PositionCalculator.GetOppositeSidePosition(lastPosition, currentPosition);
                bool isExist = GameObject.FindObjectOfType<LevelManager>().TryGetHex(newPosition, Layer, out var hex);
                if (!isExist)
                {
                    OnCreateHex(new NormalHex(newPosition, Layer));
                    lastPosition = currentPosition;
                    currentPosition = newPosition;
                }
                else return;
            }
        }
        public override BaseFakeHexType GetFakeHex()
        {
            return new FakeCreatingAwayHex(Position, Layer);
        }
    }
}
