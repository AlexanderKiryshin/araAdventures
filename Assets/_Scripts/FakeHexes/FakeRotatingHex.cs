using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets._Scripts.Additional;
using UnityEngine;
using UnityEngine.Tilemaps;
using Assets._Scripts.Model;

namespace Assets._Scripts.FakeHexes
{
    public class FakeRotatingHex:BaseFakeHexType
    {
        public bool isClockwiseRotating;
        public FakeRotatingHex(Position position, int layer, bool isClockwiseRotating) : base(position, layer)
        {
            this.isClockwiseRotating = isClockwiseRotating;
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        
        }
        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
            BaseOperationWithMap.RotateHex(hero.HeroPosition, ref map, ref fruitMap);
        }

#if (UNITY_EDITOR)
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
#endif
        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
        }



        public override HexEnum GetHexEnum()
        {
            return HexEnum.RotatingHex;
        }
    }
}
