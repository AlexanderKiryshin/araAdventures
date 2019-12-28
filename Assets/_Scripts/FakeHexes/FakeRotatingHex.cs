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
    public class FakeRotatingHex:BaseFakeHexType
    {
        public bool isClockwiseRotating;
        public FakeRotatingHex(Position position, int layer, bool isClockwiseRotating) : base(position, layer)
        {
            this.isClockwiseRotating = isClockwiseRotating;
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
            if (isClockwiseRotating)
            {
                return LevelGenerator.instance.GetHexType(Constants.CLOCKWISE_ROTATING_HEX);
            }
            else
            {
                return LevelGenerator.instance.GetHexType(Constants.COUNTER_CLOCKWISE_ROTATING_HEX);
            }

        }
    }
#endif
}
