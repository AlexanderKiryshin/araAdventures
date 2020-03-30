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
        Position lastPosition;
        public FakeIceHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            var newPosition = PositionCalculator.GetOppositeSidePosition(previousCoordinate, Position);
            lastPosition = hero.HeroPosition;
            OnLeaveHex(newPosition, ref hero, ref map);
        }
        
        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            BaseFakeHexType hex=null;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] == null)
                    {
                        continue;
                    }
                    if (map[x, y].Position.x == nextHex.x && map[x, y].Position.y == nextHex.y)
                    {
                        hex = map[x, y];
                    }
                }
            }

            if (hex != null)
            {
                if (hex.IsPassable())
                {
                    hero.HeroPosition = nextHex;
                    hex.OnEnterHex(lastPosition, ref hero, ref map);
                }
            }
            else
            {
                hero = null;
            }
        }
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.ICE_HEX);
        }
    }
#endif
}
