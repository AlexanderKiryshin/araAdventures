
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Assets._Scripts.Additional;
using Assets._Scripts.Model;
using Assets._Scripts.FakeHexes;
using Assets.Scripts;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Profiling;

namespace Assets._Scripts
{
#if (UNITY_EDITOR)
    public static class PathFinder
    {
        public static int count = 0;
        public static PathWithFruitMap GetPath(ResultWithFruitMap map, int maxDepth, int maxPasses)
        {
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
                PathFindResult result = GetPath(moveHero, map.hexMap, fruitMap, ispassedHexes, maxDepth, 0, fruitCount, path, 0, maxPasses);
                results.Add(fruitMap, result);
            }
            return new PathWithFruitMap(map.hexMap, results);
        }

        private static PathFindResult GetPath(FakeMoveHero hero, BaseFakeHexType[,] hexMap, IAdditional[,] fruitMaps, int[,] ispassedHexes,
            int maxDepth, int currentDepth, int fruitCount, List<Position> pathList,
            int isResultPathLength, int maxPasses)
        {
            string str = "";
            for (int x = 0; x < fruitMaps.GetLength(0); x++)
            {
                for (int y = 0; y < fruitMaps.GetLength(1); y++)
                {
                    if (fruitMaps[x, y] == null)
                    {
                        str += " [" + x + "," + y + "] NULL ";
                    }
                    else
                    {
                        str += " [" + x + "," + y + "] " + fruitMaps[x, y] + " ";
                    }
                }
            }
            Debug.LogError(str);
            PathFindResult pathFindResults = new PathFindResult();
            pathFindResults.results = new List<PathFindData>();
            pathFindResults.ispassedHexes = new int[hexMap.GetLength(0), hexMap.GetLength(1)];
            if (pathList == null)
            {
                pathList = new List<Position>();
                Debug.LogError("start " + hero.HeroPosition.x + " " + hero.HeroPosition.y);
                pathList.Add(hero.HeroPosition);
                ispassedHexes[hero.HeroPosition.x, hero.HeroPosition.y]++;

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
                        copyHexMap[x, y] = hexMap[x, y].ShallowCopy();
                    }
                }
                TimeChecker.EndMeasurement("PATH_HEX_MAP_COPY");
                if (ispassedHexes[hex.x, hex.y] + 1 > maxPasses)
                {
                    return pathFindResults;
                }

                // bool[,] copyIsPassedHexes =(bool[,]) ispassedHexes.Clone();
                FakeMoveHero copyHero = hero.ShallowCopy();
                ispassedHexes[hex.x, hex.y]++;
                TimeChecker.BeginMeasurement("PATH_ACTION");
                copyHexMap[copyHero.HeroPosition.x, copyHero.HeroPosition.y].OnLeaveHex(ref copyHexMap);
                copyHero.HeroPosition = new Position(hex.x, hex.y);
                copyHexMap[hex.x, hex.y].OnEnterHex(ref copyHero, ref copyHexMap);
                List<Position> copyPath = new List<Position>(pathList);
                copyPath.Add(hex);
                Debug.LogError(hex.x + " " + hex.y);
                TimeChecker.EndMeasurement("PATH_ACTION");

                int isResultLength = -1;
                if (isResultPathLength > 0)
                {
                    isResultLength = isResultPathLength;
                }
                int copyFruitCount = fruitCount;
                if (copyFruitMaps[hex.x, hex.y] != null)
                {
                    if (copyFruitMaps[hex.x, hex.y].GetType() != typeof(EmptyAdditional) && copyFruitMaps[hex.x, hex.y].GetType() != typeof(StartPosition))
                    {
                        ((BaseFruit)copyFruitMaps[hex.x, hex.y]).countPasses--;
                        if (((BaseFruit)copyFruitMaps[hex.x, hex.y]).countPasses == 0)
                        {
                            copyFruitMaps[hex.x, hex.y] = null;
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
                                continue;
                            }
                        }
                    }
                }
                if (currentDepth < maxDepth)
                {
                    if (currentDepth > 3)
                    {
                        int v = 3;
                    }

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
                }
                else
                {
                    pathFindResults.results.Add(new PathFindData(copyPath, isResultLength, true));
                }
            }

            pathFindResults.ispassedHexes = ispassedHexes;
            Debug.LogError("return");
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

        private static void MakeAction(ref CopyResult copy, Position hex)
        {
            TimeChecker.BeginMeasurement("PATH_ACTION");
            copy.hexMap[copy.hero.HeroPosition.x, copy.hero.HeroPosition.y].OnLeaveHex(ref copy.hexMap);
            copy.hero.HeroPosition = new Position(hex.x, hex.y);
            copy.hexMap[hex.x, hex.y].OnEnterHex(ref copy.hero, ref copy.hexMap);

            TimeChecker.EndMeasurement("PATH_ACTION");
        }

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
            int[,] copyIsPassedHexes = (int[,])ispassedHexes.Clone();
            FakeMoveHero copyHero = hero.ShallowCopy();
            return new CopyResult(copyFruitMaps, copyHexMap, copyIsPassedHexes, copyHero);
        }

        public static List<Position> GetAvailableHexes(FakeMoveHero moveHero, IFakeHexType[,] map)
        {
            List<Position> availableHexes = new List<Position>();
            Position[] potentialHexes = PositionCalculator.GetAroundSidePositions(moveHero.HeroPosition);
            int xLength = map.GetLength(0);
            int yLength = map.GetLength(1);
            foreach (var hex in potentialHexes)
            {
                //   Debug.LogError(xLength+"  "+ hex.x+" "+yLength+"  "+hex.y);
                if (hex.x < xLength && hex.x >= 0 && hex.y < yLength && hex.y >= 0 && map[hex.x, hex.y].GetType() != typeof(FakeEmptyHex))
                {
                    availableHexes.Add(new Position(hex.x, hex.y));
                }
            }

            return availableHexes;
        }
    }
#endif
}
