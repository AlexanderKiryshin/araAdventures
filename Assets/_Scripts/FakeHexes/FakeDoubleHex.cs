using Assets._Scripts.Additional;
using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets._Scripts.Model;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
    public class FakeDoubleHex:BaseFakeHexType
    {
        public FakeDoubleHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }
        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            BaseOperationWithMap.ChangeHex(Position,Layer,map,new FakeNormalHex(Position,Layer));
        }
        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref Dictionary<Position,HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
            BaseOperationWithMap.ChangeHex(this.Position, this.Layer,ref map, new FakeNormalHex(Position, Layer));
        }
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.DOUBLE_HEX);
        }
#endif
        public override HexEnum GetHexEnum()
        {
            return HexEnum.DoubleHex;
        }
    }
}
