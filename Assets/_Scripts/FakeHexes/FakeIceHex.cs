using Assets._Scripts.Additional;
using Assets._Scripts.Model;
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
    public class FakeIceHex:BaseFakeHexType
    {
        Position lastPosition;
        public bool lastHexIsPassable;
        public FakeIceHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            var newPosition = PositionCalculator.GetOppositeSidePosition(previousCoordinate, Position);
            lastPosition = hero.HeroPosition;
            OnLeaveHex(newPosition, ref hero, ref map);
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero,
            ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
            var newPosition = PositionCalculator.GetOppositeSidePosition(previousCoordinate, Position);
            lastPosition = hero.HeroPosition;
            OnLeaveHex(newPosition, ref hero, ref map, ref  fruitMap);
        }
        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
            PathFinder.ActionIcePassed?.Invoke();
            if (map.TryGetValue(nextHex, out var hex))
            {
                if (!hex.hex.IsService())
                {
                    if (hex.hex.IsPassable())
                    {
                        lastHexIsPassable = true;
                        hero.HeroPosition = nextHex;

                        hex.hex.OnEnterHex(Position, ref hero, ref map, ref fruitMap);
                        /* if (lastHexIsPassable)
                         {
                             hex.OnEnterHex(lastPosition, ref hero, ref map);
                         }
                         else
                         {
                             hex.OnEnterHex(Position, ref hero, ref map);
                         }*/
                    }
                    else
                    {
                        lastHexIsPassable = false;
                    }
                }
                else
                {
                    lastHexIsPassable = false;
                    hero = null;
                }
            }
            else
            {
                lastHexIsPassable = false;
                hero = null;
            }
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
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.ICE_HEX);
        }
#endif
        public override HexEnum GetHexEnum()
        {
            return HexEnum.IceHex;
        }
    }
}
