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
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.NORMAL_HEX);
        }

    }
#endif
}
