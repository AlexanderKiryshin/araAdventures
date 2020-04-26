using System.Collections.Generic;
using Assets.Scripts;
using Assets._Scripts.Additional;
using UnityEngine;
using Assets._Scripts.Model;

namespace Assets._Scripts.FakeHexes
{
    public static class BaseOperationWithMap
    {
        public static void DestroyHex(Position position, int layout, IFakeHexType[,] map)
        {
            map[position.x + HelpManager.instance.Offset.x, position.y + HelpManager.instance.Offset.y] =
                new FakeEmptyHex();
        }

        public static void ChangeHex(Position position, int layout, IFakeHexType[,] map, IFakeHexType hex)
        {
            map[position.x + HelpManager.instance.Offset.x, position.y + HelpManager.instance.Offset.y] = hex;
        }

        public static void DestroyHex(Position position, int layout, ref Dictionary<Position, HexWithPasses> map)
        {
            map.Remove(position);
            /*if (map.TryGetValue(position, out var hex))
            {
                hex=new FakeEmptyHex();
            }*/
        }

        public static void ChangeHex(Position position, int layout, ref Dictionary<Position, HexWithPasses> map,
            IFakeHexType hex)
        {
            if (map.TryGetValue(position, out var hexmap))
            {
                map.Remove(position);
                map.Add(position, hexmap);
                hexmap.hex = (BaseFakeHexType) hex;
            }
        }

        public static void CreateHex(Position position, ref Dictionary<Position, HexWithPasses> map,
            BaseFakeHexType hex)
        {
            if (map.TryGetValue(position, out var hexmap))
            {
                return;
            }
            else
            {
                map.Add(position,new HexWithPasses(hex));
            }
        }

        public static void RotateHex(Position position, ref Dictionary<Position, HexWithPasses> map,
            ref Dictionary<Position, IAdditional> fruits)
        {
            map.TryGetValue(position, out var rotatingHex);
            Position[] positions = PositionCalculator.GetAroundSidePositions(rotatingHex.hex.Position);
            var rotatingFruits = new List<BaseFruit>();
            var newFruitsPositions = new List<Position>();
            var newFruitList = new List<BaseFruit>();
            var hexesForRotate = new List<HexWithPasses>();
            var newHexPositions = new List<Position>();

            foreach (var pos in positions)
            {
                if (map.TryGetValue(pos, out var hex))
                {
                   // Debug.LogError(hex.GetType());
                    if (hex.hex.GetType() != typeof(FakeCameraExtensionHex))
                    {
                        var adjustmentpos = PositionCalculator.GetAdjustmentPosition(pos,
                            !((FakeRotatingHex) rotatingHex.hex).isClockwiseRotating,
                            rotatingHex.hex.Position);
                        if (map.TryGetValue(adjustmentpos, out var promHex))
                        {
                            if (promHex.hex.GetType() == typeof(FakeCameraExtensionHex))
                            {
                                return;
                            }
                        }
                        newHexPositions.Add(adjustmentpos);
                        hexesForRotate.Add(hex);
                    }

                    if (fruits.TryGetValue(pos, out var fruit))
                    {
                        if (fruit.GetType() != typeof(StartPosition) && fruit.GetType() != typeof(EmptyAdditional))
                        {
                            rotatingFruits.Add((BaseFruit) fruit);
                            var fruitpos = PositionCalculator.GetAdjustmentPosition(pos,
                                !((FakeRotatingHex) rotatingHex.hex).isClockwiseRotating,
                                rotatingHex.hex.Position);
                            newFruitsPositions.Add(fruitpos);
                            newFruitList.Add((BaseFruit) fruit);
                        }
                    }
                }
            }

            for (int i = 0; i < hexesForRotate.Count; i++)
            {
                map.Remove(hexesForRotate[i].hex.Position);
                hexesForRotate[i].hex.Position = newHexPositions[i];
            }

            for (int i = 0; i < hexesForRotate.Count; i++)
            {
                map.Add(hexesForRotate[i].hex.Position,new HexWithPasses(hexesForRotate[i].CountPasses,hexesForRotate[i].hex));
            }

            for (int i = 0; i < rotatingFruits.Count; i++)
            {
                fruits.Remove(new Position(rotatingFruits[i].position.x, rotatingFruits[i].position.y));
                rotatingFruits[i].position = new Vector2Int(newFruitsPositions[i].x, newFruitsPositions[i].y);
            }

            for (int i = 0; i < rotatingFruits.Count; i++)
            {
                fruits.Add(new Position(rotatingFruits[i].position.x, rotatingFruits[i].position.y), rotatingFruits[i]);
            }

        }
    }
}
