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
    public class FakeNormalHex:BaseFakeHexType
    {
        public FakeNormalHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }

        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            BaseOperationWithMap.DestroyHex(this.Position,this.Layer,map);
        }

        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
            BaseOperationWithMap.DestroyHex(this.Position,this.Layer,ref map);
        }
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.NORMAL_HEX);
        }
#endif

        public override HexEnum GetHexEnum()
        {
            return HexEnum.NormalHex;
        }
    }
}
