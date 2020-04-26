using Assets._Scripts.Additional;
using Assets._Scripts.Model;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
    public class FakeCreatingAwayHex:BaseFakeHexType
    {
        public FakeCreatingAwayHex(Position position, int layer) : base(position, layer)
        {
        }

       /* public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            for (int i = 0; i < 3; i++)
            {
                var newPosition = PositionCalculator.GetOppositeSidePosition(previousCoordinate,hero.HeroPosition);
                bool isExist = GameObject.FindObjectOfType<LevelManager>().TryGetHex(newPosition, Layer, out var hex);
                if (!isExist)
                {
                    BaseOperationWithMap.CreateHex(newPosition,ref map,new FakeNormalHex(newPosition, Layer));
                    lastPosition = currentPosition;
                    currentPosition = newPosition;
                }
                else return;
            }
        }*/

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero,
            ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
            var lastPosition = previousCoordinate;
            var currentPosition = Position;
            for (int i = 0; i < 3; i++)
            {
                var newPosition = PositionCalculator.GetOppositeSidePosition(lastPosition,currentPosition);
                bool isExist = map.TryGetValue(newPosition, out var hex);
                if (!isExist)
                {
                    BaseOperationWithMap.CreateHex(newPosition, ref map, new FakeNormalHex(newPosition, Layer));
                    lastPosition = currentPosition;
                    currentPosition = newPosition;
                }
                else return;
            }
        }

        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.CREATING_AWAY_HEX);
        }
#endif
        public override HexEnum GetHexEnum()
        {
            return HexEnum.CreatingAwayHex;
        }
    }
}
