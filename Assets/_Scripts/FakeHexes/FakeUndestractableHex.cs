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
    public class FakeUndestractableHex:BaseFakeHexType
    {
        public FakeUndestractableHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnEnterHex(ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }

        public override void OnLeaveHex(ref BaseFakeHexType[,] map)
        {
        }
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.UNDESTRACTABLE_HEX);
        }
    }
#endif
}
