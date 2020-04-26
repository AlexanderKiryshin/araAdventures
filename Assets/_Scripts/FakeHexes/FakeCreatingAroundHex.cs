using Assets._Scripts.Additional;
using Assets._Scripts.Model;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
    public class FakeCreatingAroundHex:BaseFakeHexType
    {
        public FakeCreatingAroundHex(Position position, int layer) : base(position, layer)
        {
        }
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.CREATING_AROUND_HEX);
        }
#endif
        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            throw new System.NotImplementedException();
        }

        public override HexEnum GetHexEnum()
        {
            return HexEnum.CreatingAroundHex;
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero,
            ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
            var positions = PositionCalculator.GetAroundSidePositions(Position);
            foreach (var position in positions)
            {
                BaseOperationWithMap.CreateHex(position, ref map, new FakeNormalHex(position, Layer));
            }
        }
    }
}
