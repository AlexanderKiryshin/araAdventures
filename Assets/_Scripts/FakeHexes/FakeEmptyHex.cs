using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
    public class FakeEmptyHex:BaseFakeHexType
    {
        public FakeEmptyHex(Position position, int layer) : base(position, layer)
        {
        }

        public FakeEmptyHex()
        {

        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            throw new System.NotImplementedException();
        }
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return null;
        }
#endif
        public override HexEnum GetHexEnum()
        {
            return HexEnum.EmptyHex;
        }
    }
}
