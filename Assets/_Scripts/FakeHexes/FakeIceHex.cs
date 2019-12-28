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
    public class FakeIceHex:BaseFakeHexType
    {
        public FakeIceHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnEnterHex(ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            throw new NotImplementedException();
        }

        public override void OnLeaveHex(ref BaseFakeHexType[,] map)
        {
            throw new NotImplementedException();
        }
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.ICE_HEX);
        }
    }
#endif
}
