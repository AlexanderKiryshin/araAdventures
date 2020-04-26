using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
    public class FakeImpassableHex : BaseFakeHexType
    {
        public FakeImpassableHex(Position position, int layer) : base(position, layer)
        {
        }

#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            throw new NotImplementedException();
        }
#endif

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }

        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }

        public override bool IsPassable()
        {
            return false;
        }

        public override HexEnum GetHexEnum()
        {
            return HexEnum.ImpassableHex;
        }
    }
}
