
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Assets._Scripts.Additional;
using Assets._Scripts.Model;
using Assets._Scripts.FakeHexes;
using Assets.Scripts;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Profiling;
using Assets.Scripts.Cells;

namespace Assets._Scripts
{
#if (UNITY_EDITOR)
    public static class PathFinder
    {
        public static PathFindData GetPath(List<BaseFruit> fruitMapList, List<BaseFakeHexType> hexMapList,
            Position heroPosition)
        {
            Position bottomLeft = new Position(100, 100);

            Position upRight = new Position(-100, -100);
            foreach (var hex in hexMapList)
            {
                if (hex.Position.x < bottomLeft.x)
                {
                    bottomLeft.x = hex.Position.x;
                }

                if (hex.Position.y < bottomLeft.y)
                {
                    bottomLeft.y = hex.Position.y;
                }

                if (hex.Position.x > upRight.x)
                {
                    upRight.x = hex.Position.x;
                }

                if (hex.Position.y > upRight.y)
                {
                    upRight.y = hex.Position.y;
                }
            }

            Vector2Int offset = new Vector2Int(0, 0);
            upRight.x -= bottomLeft.x;
            offset.x = -bottomLeft.x;
            bottomLeft.x = 0;
            upRight.y -= bottomLeft.y;
            offset.y = -bottomLeft.y;
            HelpManager.instance.Offset = offset;
            bottomLeft.y = 0;

            BaseFakeHexType[,] hexMap = new BaseFakeHexType[upRight.x - bottomLeft.x + 1, upRight.y - bottomLeft.y + 1];
            IAdditional[,] fruitMap = new IAdditional[upRight.x - bottomLeft.x + 1, upRight.y - bottomLeft.y + 1];
            for (int x = 0; x < hexMap.GetLength(0) - 1; x++)
            {
                for (int y = 0; y < hexMap.GetLength(1) - 1; y++)
                {
                    hexMap[x, y] = new FakeEmptyHex(new Position(x - offset.x, y - offset.y), 0);
                    fruitMap[x, y] = new EmptyAdditional();
                }
            }

            foreach (var hex in hexMapList)
            {
                //hex.Position=new Position(hex.Position.x/*+offset.x*/,hex.Position.y/*+offset.y*/);
                hexMap[hex.Position.x + offset.x, hex.Position.y + offset.y] = hex;
            }

            foreach (var fruit in fruitMapList)
            {
                fruitMap[((BaseFruit) fruit).position.x + offset.x, ((BaseFruit) fruit).position.y + offset.y] = fruit;
            }

            fruitMap[heroPosition.x + offset.x, heroPosition.y + offset.y] = new StartPosition();
            FakeMoveHero moveHero = new FakeMoveHero();
            moveHero.HeroPosition = new Position(heroPosition.x /* + offset.x*/, heroPosition.y /* + offset.y*/);
            int[,] ispassedHexes = new int[fruitMap.GetLength(0), fruitMap.GetLength(1)];
            List<Position> path = null;
            TimeChecker.Clear();
            TimeChecker.BeginMeasurement("PATH");
            PathFindResult results = GetPath(moveHero, hexMap, fruitMap, ispassedHexes, 100, 0, fruitMapList.Count,
                path, 0, 6);
            TimeChecker.EndMeasurement("PATH");
            TimeChecker.PrintResult();
            PathFindData result = null;
            foreach (var res in results.results)
            {
                if (res.lengthRightPath > -1)
                {
                    if (result != null)
                    {
                        if (res.lengthRightPath < result.lengthRightPath)
                        {
                            result = res;
                        }
                    }
                    else
                    {
                        result = res;
                    }
                }
            }

            return result;
        }

        public static PathVariant GetPath(FakeMoveHero hero, Dictionary<Position, BaseFakeHexType> hexMap,
            Dictionary<Position, IAdditional> fruitMaps)
        {
            int fruitCount = fruitMaps.Count;
            List<Position> startHexes = GetAvailableHexes(hero, hexMap);
            List<PathVariant> pathVariants=new List<PathVariant>();
            if (startHexes.Count == 0)
            {
                return null;
            }
            else
            {
                foreach (var hexPosition in startHexes)
                {

                   pathVariants.Add(new PathVariant(hero, fruitCount, null, hexMap, fruitMaps));
                }
            }
        }

        public static int count = 0;

        public static PathWithFruitMap GetPath(ResultWithFruitMap map, int maxDepth, int maxPasses)
        {
            TimeChecker.BeginCount("COUNT_VARIANTS");
            TimeChecker.BeginMeasurement("PATH_INITIALIZE");
            FakeMoveHero moveHero = new FakeMoveHero();
            Dictionary<IAdditional[,], PathFindResult> results = new Dictionary<IAdditional[,], PathFindResult>();
            foreach (var fruitMap in map.fruitMaps)
            {
                int fruitCount = 0;
                for (int x = 0; x < fruitMap.GetLength(0); x++)
                {
                    for (int y = 0; y < fruitMap.GetLength(1); y++)
                    {
                        if (fruitMap[x, y] == null)
                        {
                            continue;
                        }

                        if (fruitMap[x, y].GetType() == typeof(StartPosition))
                        {
                            moveHero.HeroPosition = new Position(x, y);
                        }
                        else
                        {
                            if (fruitMap[x, y] != null)
                            {
                                fruitCount++;
                            }
                        }
                    }
                }

                int[,] ispassedHexes = new int[fruitMap.GetLength(0), fruitMap.GetLength(1)];
                List<Position> path = null;
                TimeChecker.EndMeasurement("PATH_INITIALIZE");
                TimeChecker.BeginMeasurement("PATH");
                PathFindResult result = GetPath(moveHero, map.hexMap, fruitMap, ispassedHexes, maxDepth, 0, fruitCount,
                    path, 0, maxPasses);
                TimeChecker.EndMeasurement("PATH");
                results.Add(fruitMap, result);
            }

            return new PathWithFruitMap(map.hexMap, results);
        }

        private static PathFindResult GetPath(FakeMoveHero hero, BaseFakeHexType[,] hexMap, IAdditional[,] fruitMaps,
            int[,] ispassedHexes,
            int maxDepth, int currentDepth, int fruitCount, List<Position> pathList,
            int isResultPathLength, int maxPasses)
        {
            TimeChecker.BeginCount("COUNT_VARIANTS");
            PathFindResult pathFindResults = new PathFindResult();
            pathFindResults.results = new List<PathFindData>();
            pathFindResults.ispassedHexes = new int[hexMap.GetLength(0), hexMap.GetLength(1)];
            if (pathList == null)
            {
                pathList = new List<Position>();
                Debug.LogError("start " + hero.HeroPosition.x + " " + hero.HeroPosition.y);
                pathList.Add(hero.HeroPosition);
                ispassedHexes[hero.HeroPosition.x + HelpManager.instance.Offset.x,
                    hero.HeroPosition.y + HelpManager.instance.Offset.y]++;

            }

            TimeChecker.BeginMeasurement("PATH_GET_AVAILABLE_HEXES");
            List<Position> startHexes = GetAvailableHexes(hero, hexMap);
            TimeChecker.EndMeasurement("PATH_GET_AVAILABLE_HEXES");
            if (startHexes.Count == 0)
            {
                bool resultFound = fruitCount == 0;
                if (resultFound)
                {
                    pathFindResults.results.Add(new PathFindData(pathList, isResultPathLength, false));
                }
                else
                {
                    pathFindResults.results.Add(new PathFindData(pathList, -1, false));
                }

            }

            foreach (var hex in startHexes)
            {
                IAdditional[,] copyFruitMaps = new IAdditional[fruitMaps.GetLength(0), fruitMaps.GetLength(1)];
                TimeChecker.BeginMeasurement("PATH_FRUIT_MAP_COPY");
                for (int x = 0; x < fruitMaps.GetLength(0); x++)
                {
                    for (int y = 0; y < fruitMaps.GetLength(1); y++)
                    {
                        if (fruitMaps[x, y] != null)
                        {
                            copyFruitMaps[x, y] = fruitMaps[x, y].ShallowCopy();
                        }
                        else
                        {
                            copyFruitMaps[x, y] = null;
                        }
                    }
                }

                TimeChecker.EndMeasurement("PATH_FRUIT_MAP_COPY");
                TimeChecker.BeginMeasurement("PATH_HEX_MAP_COPY");
                BaseFakeHexType[,] copyHexMap = new BaseFakeHexType[fruitMaps.GetLength(0), fruitMaps.GetLength(1)];
                for (int x = 0; x < copyHexMap.GetLength(0); x++)
                {
                    for (int y = 0; y < copyHexMap.GetLength(1); y++)
                    {
                        if (hexMap[x, y] != null)
                        {
                            copyHexMap[x, y] = hexMap[x, y].ShallowCopy();
                        }
                    }
                }

                TimeChecker.EndMeasurement("PATH_HEX_MAP_COPY");
                if (ispassedHexes[hex.x + HelpManager.instance.Offset.x, hex.y + HelpManager.instance.Offset.y] + 1 >
                    maxPasses)
                {
                    return pathFindResults;
                }

                // bool[,] copyIsPassedHexes =(bool[,]) ispassedHexes.Clone();
                TimeChecker.BeginMeasurement("HERO_COPY");
                FakeMoveHero copyHero = hero.ShallowCopy();
                TimeChecker.EndMeasurement("HERO_COPY");
                ispassedHexes[hex.x + HelpManager.instance.Offset.x, hex.y + HelpManager.instance.Offset.y]++;
                TimeChecker.BeginMeasurement("PATH_ACTION");
                copyHexMap[copyHero.HeroPosition.x + HelpManager.instance.Offset.x,
                        copyHero.HeroPosition.y + HelpManager.instance.Offset.y]
                    .OnLeaveHex(hex, ref copyHero, ref copyHexMap);
                if (copyHero == null)
                {
                    return pathFindResults;
                }

                var lastPosition = copyHero.HeroPosition;
                copyHero.HeroPosition = new Position(hex.x, hex.y);
                copyHexMap[hex.x + HelpManager.instance.Offset.x, hex.y + HelpManager.instance.Offset.y]
                    .OnEnterHex(lastPosition, ref copyHero, ref copyHexMap);
                if (copyHero == null)
                {
                    return pathFindResults;
                }

                List<Position> copyPath = new List<Position>(pathList);
                copyPath.Add(hex);
                // Debug.LogError(hex.x + " " + hex.y);
                TimeChecker.EndMeasurement("PATH_ACTION");

                int isResultLength = -1;
                if (isResultPathLength > 0)
                {
                    isResultLength = isResultPathLength;
                }

                int copyFruitCount = fruitCount;
                TimeChecker.BeginMeasurement("RESULT");
                if (copyFruitMaps[copyHero.HeroPosition.x + HelpManager.instance.Offset.x,
                        copyHero.HeroPosition.y + HelpManager.instance.Offset.y] != null)
                {
                    if (copyFruitMaps[copyHero.HeroPosition.x + HelpManager.instance.Offset.x,
                            copyHero.HeroPosition.y + HelpManager.instance.Offset.y].GetType() !=
                        typeof(EmptyAdditional)
                        && copyFruitMaps[copyHero.HeroPosition.x + HelpManager.instance.Offset.x,
                            copyHero.HeroPosition.y + HelpManager.instance.Offset.y].GetType() != typeof(StartPosition))
                    {
                        ((BaseFruit) copyFruitMaps[copyHero.HeroPosition.x + HelpManager.instance.Offset.x,
                            copyHero.HeroPosition.y + HelpManager.instance.Offset.y]).OnFakeEat();
                        if (((BaseFruit) copyFruitMaps[copyHero.HeroPosition.x + HelpManager.instance.Offset.x,
                                copyHero.HeroPosition.y + HelpManager.instance.Offset.y]).CountPasses == 0)
                        {
                            copyFruitMaps[copyHero.HeroPosition.x + HelpManager.instance.Offset.x,
                                copyHero.HeroPosition.y + HelpManager.instance.Offset.y] = null;
                            copyFruitCount--;
                            if (copyFruitCount == 0)
                            {
                                string pathres = "___";
                                foreach (var path in copyPath)
                                {
                                    pathres += " [" + path.x + " " + path.y + "]";
                                }

                                // Debug.LogError(pathres);
                                isResultLength = copyPath.Count;
                                pathFindResults.results.Add(new PathFindData(copyPath, isResultLength, true));
                                return pathFindResults;
                            }
                        }
                    }
                }

                TimeChecker.EndMeasurement("RESULT");
                if (currentDepth < maxDepth)
                {
                    count++;
                    PathFindResult pathFindResult = GetPath(copyHero, copyHexMap, copyFruitMaps, ispassedHexes,
                        maxDepth, ++currentDepth,
                        copyFruitCount, copyPath, isResultLength, maxPasses);
                    TimeChecker.BeginMeasurement("PATH_IS_PASSED_HEXES");
                    for (int x = 0; x < ispassedHexes.GetLength(0); x++)
                    {
                        for (int y = 0; y < ispassedHexes.GetLength(1); y++)
                        {
                            if (pathFindResult.ispassedHexes[x, y] > 0)
                            {
                                ispassedHexes[x, y] = 1;
                            }
                        }
                    }

                    TimeChecker.EndMeasurement("PATH_IS_PASSED_HEXES");
                    pathFindResults.results.AddRange(pathFindResult.results);
                    if (pathFindResult.results.Count > 0 && pathFindResult.results[0].lengthRightPath > 0)
                    {
                        return pathFindResults;
                    }
                }
                else
                {
                    pathFindResults.results.Add(new PathFindData(copyPath, isResultLength, true));
                }
            }

            pathFindResults.ispassedHexes = ispassedHexes;
            // Debug.LogError("return");
            return pathFindResults;
        }

        /*  private static PathFindResult GetPath(FakeMoveHero hero, BaseFakeHexType[,] hexMap, IAdditional[,] fruitMaps,
             int maxDepth, int currentDepth, int fruitCount, List<Position> pathList, int isResultPathLength, int maxPasses)
          {
  
              int[,] ispassedHexes = new int[hexMap.GetLength(0), hexMap.GetLength(1)];
              CopyResult copy = CopyData(fruitMaps, hexMap, ispassedHexes, hero);
              pathList = new List<Position>();
              pathList.Add(hero.HeroPosition);
  
              ispassedHexes[hero.HeroPosition.x, hero.HeroPosition.y]++;
              TimeChecker.BeginMeasurement("PATH_GET_AVAILABLE_HEXES");
              List<Position> startHexes = GetAvailableHexes(hero, hexMap);
              TimeChecker.EndMeasurement("PATH_GET_AVAILABLE_HEXES");
  
              if (startHexes.Count == 0)
              {
                  //TODO выкидываем решение
              }
              PathFindResult pathFindResults = new PathFindResult();
              foreach (var hex in startHexes)
              {
                  while (fruitCount > 0)
                  {
                      int isResultLength = -1;
                      if (ispassedHexes[hex.x, hex.y] + 1 > maxPasses)
                      {
                          //возврат к предыдущему шагу
                      }
  
                      copy.ispassedHexes[hex.x, hex.y]++;
                      MakeAction(ref copy, hex);
                      List<Position> copyPath = new List<Position>(pathList);
                      copyPath.Add(hex);
                      int copyFruitCount = fruitCount;
                      if (copy.fruitMaps[hex.x, hex.y] != null)
                      {
                          if (copy.fruitMaps[hex.x, hex.y].GetType() != typeof(EmptyAdditional) && copy.fruitMaps[hex.x, hex.y].GetType() != typeof(StartPosition))
                          {
                              ((BaseFruit)copy.fruitMaps[hex.x, hex.y]).countPasses--;
                              if (((BaseFruit)copy.fruitMaps[hex.x, hex.y]).countPasses == 0)
                              {
                                  copy.fruitMaps[hex.x, hex.y] = null;
                                  copyFruitCount--;
                                  if (copyFruitCount == 0)
                                  {
                                      string pathres = "___";
                                      foreach (var path in copyPath)
                                      {
                                          pathres += " [" + path.x + " " + path.y + "]";
                                      }
                                      Debug.LogError(pathres);
                                      isResultLength = copyPath.Count;
                                      pathFindResults.results.Add(new PathFindData(copyPath, isResultLength, true));
                                  }
                              }
                          }
                      }
                  }
  
              }
          }*/

        /*  private static void MakeAction(ref CopyResult copy, Position hex)
          {
              TimeChecker.BeginMeasurement("PATH_ACTION");
              copy.hexMap[copy.hero.HeroPosition.x, copy.hero.HeroPosition.y].OnLeaveHex(ref copy.hexMap);
              copy.hero.HeroPosition = new Position(hex.x, hex.y);
              copy.hexMap[hex.x, hex.y].OnEnterHex(ref copy.hero, ref copy.hexMap);
  
              TimeChecker.EndMeasurement("PATH_ACTION");
          }*/

        private static CopyResult CopyData(IAdditional[,] fruitMaps, BaseFakeHexType[,] hexMap, int[,] ispassedHexes,
            FakeMoveHero hero)
        {
            IAdditional[,] copyFruitMaps = new IAdditional[fruitMaps.GetLength(0), fruitMaps.GetLength(1)];
            TimeChecker.BeginMeasurement("PATH_FRUIT_MAP_COPY");
            for (int x = 0; x < fruitMaps.GetLength(0); x++)
            {
                for (int y = 0; y < fruitMaps.GetLength(1); y++)
                {
                    if (fruitMaps[x, y] != null)
                    {
                        copyFruitMaps[x, y] = fruitMaps[x, y].ShallowCopy();
                    }
                    else
                    {
                        copyFruitMaps[x, y] = null;
                    }
                }
            }

            TimeChecker.EndMeasurement("PATH_FRUIT_MAP_COPY");
            TimeChecker.BeginMeasurement("PATH_HEX_MAP_COPY");
            BaseFakeHexType[,] copyHexMap =
                new BaseFakeHexType[fruitMaps.GetLength(0), fruitMaps.GetLength(1)];
            for (int x = 0; x < copyHexMap.GetLength(0); x++)
            {
                for (int y = 0; y < copyHexMap.GetLength(1); y++)
                {
                    copyHexMap[x, y] = hexMap[x, y].ShallowCopy();
                }
            }

            TimeChecker.EndMeasurement("PATH_HEX_MAP_COPY");
            int[,] copyIsPassedHexes = (int[,]) ispassedHexes.Clone();
            FakeMoveHero copyHero = hero.ShallowCopy();
            return new CopyResult(copyFruitMaps, copyHexMap, copyIsPassedHexes, copyHero);
        }

        public static List<Position> GetAvailableHexes(FakeMoveHero moveHero, IFakeHexType[,] map)
        {
            List<Position> availableHexes = new List<Position>();
            Position[] potentialHexes = PositionCalculator.GetAroundSidePositions(moveHero.HeroPosition);
            //  int xLength = map.GetLength(0);
            //   int yLength = map.GetLength(1);
            foreach (var hex in potentialHexes)
            {
                //   Debug.LogError(xLength+"  "+ hex.x+" "+yLength+"  "+hex.y);
                if (hex.x + HelpManager.instance.Offset.x < map.GetLength(0)
                    && hex.y + HelpManager.instance.Offset.y < map.GetLength(1) &&
                    hex.x + HelpManager.instance.Offset.x >= 0 &&
                    hex.y + HelpManager.instance.Offset.y >= 0 &&
                    (map[hex.x + HelpManager.instance.Offset.x, hex.y + HelpManager.instance.Offset.y] != null)
                    && map[hex.x + HelpManager.instance.Offset.x, hex.y + HelpManager.instance.Offset.y].GetType() !=
                    typeof(FakeEmptyHex))
                {
                    availableHexes.Add(new Position(hex.x, hex.y));
                }
            }

            return availableHexes;
        }

        public static List<Position> GetAvailableHexes(FakeMoveHero moveHero,
            Dictionary<Position, BaseFakeHexType> map)
        {
            List<Position> availableHexes = new List<Position>();
            Position[] potentialHexes = PositionCalculator.GetAroundSidePositions(moveHero.HeroPosition);
            foreach (var hex in potentialHexes)
            {
                map.TryGetValue(hex, out var result);
                if (result != null)
                {
                    availableHexes.Add(hex);
                }
            }
            return availableHexes;
        }
    }
#endif
}
