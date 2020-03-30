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
    public class FakeCreatingAwayHex:BaseFakeHexType
    {
        public FakeCreatingAwayHex(Position position, int layer) : base(position, layer)
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

        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.CREATING_AWAY_HEX);
        }
    }
#endif
}
