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
#if (UNITY_EDITOR)
    public class FakeDoubleHex:BaseFakeHexType
    {
        public FakeDoubleHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnEnterHex(ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }

        public override void OnLeaveHex(ref BaseFakeHexType[,] map)
        {
            BaseOperationWithMap.ChangeHex(Position,Layer,map,new FakeNormalHex(Position,Layer));
        }
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.DOUBLE_HEX);
        }
    }
#endif
}
