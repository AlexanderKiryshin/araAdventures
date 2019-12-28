using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using Assets.Scripts.Cells;
using Assets._Scripts.Additional;

using Assets._Scripts.Model;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;
using Assets._Scripts.FakeHexes;
using UnityEngine.Profiling;
using System.IO;
using Random = System.Random;

namespace Assets._Scripts
{
#if(UNITY_EDITOR)
    public class LevelGenerator : Singleton<LevelGenerator>
    {
        public Vector3Int fieldSize;
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
        public Dictionary<Tilemap, int>[] patterns;
        public List<FruitCount> fruits;
        public int maxLengthPath = 50;
        public int maxPasses = 2;
        public int countResults = 10;
        public int minLength = 4;
        public int maxCountFruitVariants = 3;
        public float percentIdentity = 0.2f;
        public Dictionary<float, int> difficultes;

        public int minDifficult;
        public int maxDifficult;
        public int countVariants;
        private GameObject hexPrefab;
        private List<TileBase> hexesTypes = new List<TileBase>();

        void LoadPalitre()
        {
            var tilemap = hexPrefab.GetComponentInChildren<Tilemap>();
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
                    if (tile != null)
                    {
                        hexesTypes.Add(tile);
                    }
                }
            }
        }

        /// <summary>
        /// Ищем тайл по имени
        /// </summary>
        /// <param name="name">имя тайла</param>
        /// <returns></returns>
        public Tile GetHexType(string name)
        {
            foreach (Tile tile in hexesTypes)
            {
                if (tile.name == name)
                {
                    return tile;
                }
            }
            throw new FileNotFoundException("Спрайт не найден");
        }

        public void GenerateMap()
        {
            hexPrefab = Resources.Load<GameObject>(Constants.HEX_PATH);
            LoadPalitre();
            Dictionary<List<BaseFakeHexType>, int> hexes = new Dictionary<List<BaseFakeHexType>, int>();
            int count = 0;
            var listHexes = new List<List<BaseFakeHexType>>();
            foreach (var pattern in patterns[0].Keys)
            {
                var newHexes = Utility.instance.LoadLevelTiles(pattern);
                patterns[0].TryGetValue(pattern, out var value);
                count += newHexes.Count * value;
                hexes.Add(newHexes, value);
                for (int i = 0; i < value; i++)
                {
                    listHexes.Add(newHexes);
                }
            }
            int emptyCells = fieldSize.x * fieldSize.y - count;
            if (emptyCells > 0)
            {
                var list = new List<BaseFakeHexType>();
                list.Add(new FakeEmptyHex());
                hexes.Add(list, emptyCells);
                for (int i = 0; i < emptyCells; i++)
                {
                    listHexes.Add(list);
                }
            }
            else
            {
                if (emptyCells < 0)
                {
                    Debug.LogError("No wariants");
                    return;
                }
            }

            List<BaseFakeHexType[,]> results = new List<BaseFakeHexType[,]>();
            var filteredResults = new List<FilteredResult>();
            TimeChecker.Clear();
            PathFinder.count = 0;
            GenerateMapRecurse(results, hexes, fieldSize, new Dictionary<float, int>(difficultes), count, null);
            Debug.LogError("Count " + PathFinder.count);
            TimeChecker.PrintResult();
            foreach (var result in results)
            {
                foreach (var hex in result)
                {
                    Debug.LogError(hex + "Position x " + hex.Position.x + "  y " + hex.Position.y);
                }
                Debug.LogError("________________________");
                // Debug.LogError(result[0, 0] + " " + result[0, 1] /*+ " "+result[1, 0] + " "+result[1, 1]*/);
            }
            /* for (int fieldSizeX = 0; fieldSizeX < fieldSize.x; fieldSizeX++)
             {
                 for (int fieldSizeY = 0; fieldSizeY < fieldSize.y; fieldSizeY++)
                 {
                     var copyHexes = listHexes;
 
                 }
             }*/
        }

        [Button("Generate")]
        public void GenerateButton()
        {
            GenerateMap();
        }

        public List<ResultWithFruitMap> SetFruitsAndStart(List<BaseFakeHexType[,]> results)
        {

            List<IAdditional[,]> fruitsResult = new List<IAdditional[,]>();
            var resultsWithFruitsmap = new List<ResultWithFruitMap>();
            foreach (var result in results)
            {

                List<List<IAdditional>> listFruits = MakeListFruts(this.fruits);
                foreach (var fruit in listFruits)
                {
                    fruit.Add(new StartPosition());
                }
                List<IAdditional[,]> fruitList = new List<IAdditional[,]>();
                fruitList = SetFruits(listFruits, result);
                fruitsResult.AddRange(fruitList);
                /*for (int x = 0; x < result.GetLength(0); x++)
                {
                    for (int y = 0; y < result.GetLength(1); y++)
                    {
                        if (result[x, y].GetType() != typeof(FakeEmptyHex))
                        {
                            List<IAdditional[,]> fruitList =new List<IAdditional[,]>();
                            fruitList[x, y] = new StartPosition();
                            List<IAdditional[,]> fruits=new List<IAdditional[,]>();
                            fruits.Add(fruitList);
                            fruitList = SetFruits(listFruits, result);
                                SetFruits(listFruits,result);
                            fruitsResult.AddRange(fruits);
                        }
                    }
                }*/
                resultsWithFruitsmap.Add(new ResultWithFruitMap(result, fruitsResult));
            }

            return resultsWithFruitsmap;
        }

        public List<List<IAdditional>> MakeListFruts(List<FruitCount> fruits)
        {

            List<List<IAdditional>> resultsFruits = new List<List<IAdditional>>();
            for (int fruit = 0; fruit < fruits.Count; fruit++)
            {
                List<List<IAdditional>> promFruits = new List<List<IAdditional>>();
                for (int count = fruits[fruit].minCount; count <= fruits[fruit].maxCount; count++)
                {

                    if (resultsFruits.Count == 0)
                    {
                        List<IAdditional> listFruit = new List<IAdditional>();
                        for (int fruitCount = 0; fruitCount < count; fruitCount++)
                        {
                            fruits[fruit].fruit.Initialize();
                            listFruit.Add((fruits[fruit].fruit));
                        }

                        promFruits.Add(listFruit);
                    }
                    else
                    {
                        List<IAdditional> listFruit = new List<IAdditional>();
                        for (int fruitCount = 0; fruitCount < count; fruitCount++)
                        {
                            fruits[fruit].fruit.Initialize();
                            listFruit.Add((fruits[fruit].fruit));
                        }

                        foreach (var list in resultsFruits)
                        {
                            List<IAdditional> promList = new List<IAdditional>(list);
                            promList.AddRange(listFruit);
                            promFruits.Add(promList);
                        }
                    }


                }
                resultsFruits = promFruits;
            }

            /*foreach (var result in resultsFruits)
            {
                int count = result.Count;
                for (int i = 0; i < countfilled - count-1; i++)
                {
                    result.Add(new EmptyAdditional());
                }
            }*/
            return resultsFruits;
        }
        public void SetFruits(ref List<IAdditional[,]> results, List<List<IAdditional>> fruits, BaseFakeHexType[,] hexmap)
        {
            if (fruits.Count == 0)
            {
                return;
            }
            if (results == null || results.Count == 0)
            {
                results = new List<IAdditional[,]>();
                results.Add(new IAdditional[fieldSize.x, this.fieldSize.y]);
            }
            int countResults = results.Count;
            List<IAdditional[,]> newResults = new List<IAdditional[,]>();
            foreach (var fruitList in fruits)
            {
                foreach (var fruit in fruitList)
                {
                    List<IAdditional[,]> promResults = new List<IAdditional[,]>();
                    for (int result = 0; result < countResults; result++)
                    {
                        bool fruitPlaceFound = false;
                        IAdditional[,] promResult = (IAdditional[,])results[result].Clone();
                        for (int x = 0; x < results[result].GetLength(0); x++)
                        {
                            for (int y = 0; y < results[result].GetLength(1); y++)
                            {
                                if (results[result][x, y] == null && hexmap[x, y].GetType() != typeof(FakeEmptyHex))
                                {
                                    promResult[x, y] = fruit;
                                    fruitPlaceFound = true;
                                    break;
                                }
                            }

                            if (fruitPlaceFound)
                            {
                                break;
                            }
                        }

                        if (fruitPlaceFound)
                        {
                            promResults.Add(promResult);
                        }

                    }

                    if (promResults.Count > 0)
                    {
                        List<List<IAdditional>>
                            nextFruits = new List<List<IAdditional>>(); //hexes.ToArray().ToDictionary();
                        foreach (var frui in fruits)
                        {
                            nextFruits.Add(new List<IAdditional>(frui));
                        }
                        nextFruits[0].Remove(fruit);
                        if (nextFruits[0].Count == 0)
                        {
                            nextFruits.RemoveAt(0);
                        }

                        SetFruits(ref promResults, nextFruits, hexmap);
                        newResults.AddRange(promResults);
                    }
                }
            }

            results = newResults;
        }

        public List<IAdditional[,]> SetFruits(List<List<IAdditional>> fruits, BaseFakeHexType[,] hexmap)
        {

            List<IAdditional[,]> results = new List<IAdditional[,]>();

            List<Vector2Int> suitableCoordinates = new List<Vector2Int>();
            for (int x = 0; x < hexmap.GetLength(0); x++)
            {
                for (int y = 0; y < hexmap.GetLength(1); y++)
                {
                    if (hexmap[x, y].GetType() != typeof(FakeEmptyHex))
                    {
                        suitableCoordinates.Add(new Vector2Int(x, y));
                    }
                }
            }

            Random random = new Random();


            for (int i = 0; i < maxCountFruitVariants; i++)
            {
                int fruitListNumber = random.Next(0, fruits.Count - 1);
                List<Vector2Int> copySuitableCoordinates = new List<Vector2Int>(suitableCoordinates);
                List<IAdditional> fruitsList = new List<IAdditional>(fruits[fruitListNumber]);
                IAdditional[,] promResult = new IAdditional[hexmap.GetLength(0), hexmap.GetLength(1)];
                foreach (var fruit in fruitsList)
                {
                    int index = random.Next(0, copySuitableCoordinates.Count - 1);
                    promResult[copySuitableCoordinates[index].x, copySuitableCoordinates[index].y] = fruit;
                    copySuitableCoordinates.RemoveAt(index);
                }
                bool isNewresult = true;
                foreach (var result in results)
                {
                    bool isNew = false;
                    for (int x = 0; x < hexmap.GetLength(0); x++)
                    {
                        for (int y = 0; y < hexmap.GetLength(1); y++)
                        {
                            if (promResult[x, y] != result[x, y])
                            {
                                isNew = true;
                                break;
                            }
                        }

                        if (isNew)
                        {
                            break;
                        }
                    }

                    if (!isNew)
                    {
                        isNewresult = false;
                        break;
                    }
                }

                if (isNewresult)
                {
                    results.Add(promResult);
                }
                else
                {
                    i--;
                }
            }

            return results;
        }


        public List<FilteredResult> FilterResults(List<PathWithFruitMap> results, Dictionary<float, int> difficultes)
        {
            // Debug.LogError("Filter");
            List<PathWithFruitMap> verifiedResults = new List<PathWithFruitMap>();

            foreach (var result in results)
            {
                for (int i = 0; i < result.pathResults.Values.Count; i++)
                {
                    bool isSuitable = true;
                    bool pathFounded = false;
                    /*  for (int x = 0; x < result.pathResults.Values.ToList()[i].ispassedHexes.GetLength(0); x++)
                      {
                          for (int y = 0; y < result.pathResults.Values.ToList()[i].ispassedHexes.GetLength(1); y++)
                          {
                              if (result.hexMap[x, y].GetType() != typeof(FakeEmptyHex))
                              {
                                  if (result.pathResults.Values.ToList()[i].ispassedHexes[x, y] == 0)
                                  {
                                      isSuitable = false;
                                  }
                              }
                          }
                      }*/

                    foreach (var pathFindData in result.pathResults.Values.ToList()[i].results)
                    {
                        if (pathFindData.lengthRightPath > -1)
                        {
                            pathFounded = true;
                        }

                        if (pathFindData.lengthRightPath < minLength && pathFindData.lengthRightPath != -1)
                        {
                            isSuitable = false;
                            break;
                        }
                    }
                    if (isSuitable && pathFounded)
                    {
                        // result.pathResults.Keys.ToList()[i], result.pathResults.Values.ToList()[i]
                        Dictionary<IAdditional[,], PathFindResult> dictionary = new Dictionary<IAdditional[,], PathFindResult>();
                        dictionary.Add(result.pathResults.Keys.ToList()[i], result.pathResults.Values.ToList()[i]);
                        PathWithFruitMap verifiedPathFindResults = new PathWithFruitMap(result.hexMap, dictionary);
                        verifiedResults.Add(verifiedPathFindResults);
                    }
                }
            }
            List<FilteredResult> promResults = new List<FilteredResult>();
            foreach (var verifiedResult in verifiedResults)
            {
                int min = 100;
                PathFindData minPathFindData = null;
                IAdditional[,] fruits = null;
                int impassablePath = 0;
                int passablePath = 0;
                for (int i = 0; i < verifiedResult.pathResults.Values.Count; i++) //verifiedResult.pathResults.Values
                {
                    foreach (var pathFindData in verifiedResult.pathResults.Values.ToList()[i].results)
                    {
                        if (pathFindData.lengthRightPath < min && pathFindData.lengthRightPath > -1)
                        {
                            min = pathFindData.lengthRightPath;
                            minPathFindData = pathFindData;
                            fruits = verifiedResult.pathResults.Keys.ToList()[i];
                        }

                        if (pathFindData.lengthRightPath == -1)
                        {
                            impassablePath++;
                        }
                        else
                        {
                            passablePath++;
                        }
                    }
                }

                if (min != 100)
                {
                    float difficulty = min * (1 + impassablePath) / ((float)(passablePath + 1));
                    minPathFindData.difficulty = difficulty;
                    promResults.Add(new FilteredResult(verifiedResult.hexMap, minPathFindData, fruits));
                }
            }

            return promResults;
        }

        public void OperateResults(List<BaseFakeHexType[,]> results)
        {
            TimeChecker.BeginMeasurement("FRUIT");
            List<ResultWithFruitMap> ResultWithFruitMap = SetFruitsAndStart(results);
            foreach (var res in ResultWithFruitMap)
            {
                for (int x = 0; x < res.hexMap.GetLength(0); x++)
                {
                    for (int y = 0; y < res.hexMap.GetLength(1); y++)
                    {
                        foreach (var fruit in res.fruitMaps)
                        {
                            if (fruit[x, y] != null && res.hexMap[x, y].GetType() == typeof(FakeEmptyHex))
                            {
                                int v = 1;
                            }
                        }
                    }
                }
            }
            TimeChecker.EndMeasurement("FRUIT");
            TimeChecker.BeginMeasurement("PATH");
            List<PathWithFruitMap> pathResults = new List<PathWithFruitMap>();
            foreach (var res in ResultWithFruitMap)
            {
                pathResults.Add(PathFinder.GetPath(res, maxLengthPath, maxPasses));
            }
            TimeChecker.EndMeasurement("PATH");
            List<FilteredResult> promResults = new List<FilteredResult>();
            TimeChecker.BeginMeasurement("FILTER");
            promResults.AddRange(FilterResults(pathResults, difficultes));
            TimeChecker.EndMeasurement("FILTER");
            for (int i = 0; i < promResults.Count; i++)
            {
                bool isSuitable = false;
                foreach (var key in difficultes.Keys)
                {
                    if (key == 0)
                    {
                        isSuitable = true;
                        break;
                    }
                    if ((key < promResults[i].pathFindData.difficulty + 0.5f) && (key > promResults[i].pathFindData.difficulty - 0.5f))
                    {
                        isSuitable = true;
                        break;
                    }

                }

                if (!isSuitable)
                {
                    promResults.RemoveAt(i);
                    i--;
                }
            }

            bool isFounded = false;
            foreach (var promResult in promResults)
            {
                foreach (var difficulty in difficultes.Keys)
                {
                    if ((promResult.pathFindData.difficulty < difficulty + 0.5f) && (promResult.pathFindData.difficulty > difficulty - 0.5f) || difficulty == 0)
                    {
                        TilemapCreator.instance.CreateMap(promResult.hexMap, promResult.fruits, promResult.pathFindData);
                        isFounded = true;
                        //filteredResults.Add(promResult);
                        // TilemapCreator.instance.CreateTilemap(promResult.hexMap);
                        difficultes.TryGetValue(difficulty, out var result);
                        if (result > 1)
                        {
                            difficultes.Remove(difficulty);
                            difficultes.Add(difficulty, result - 1);
                        }
                        else
                        {
                            difficultes.Remove(difficulty);
                        }
                        break;
                    }
                }

                if (difficultes.Count == 0)
                {
                    return;
                }
                if (isFounded)
                {
                    break;
                }
            }
        }

        private List<BaseFakeHexType[,]> verResults;
        // private List<float> percentIndentity;

        public void GenerateMapRecurse(List<BaseFakeHexType[,]> results, Dictionary<List<BaseFakeHexType>, int> hexes,
            Vector3Int fieldSize, Dictionary<float, int> difficultes, int hexesCount, List<float> percentIndentity)
        {
            if (hexes.Count == 0)
            {
                TimeChecker.BeginMeasurement("FRUIT");
                List<ResultWithFruitMap> ResultWithFruitMap = SetFruitsAndStart(results);

                // percentIndentity.Add(0);
                TimeChecker.EndMeasurement("FRUIT");
                TimeChecker.BeginMeasurement("PATH");
                List<PathWithFruitMap> pathResults = new List<PathWithFruitMap>();
                foreach (var res in ResultWithFruitMap)
                {
                    pathResults.Add(PathFinder.GetPath(res, maxLengthPath, maxPasses));
                }
                TimeChecker.EndMeasurement("PATH");
                List<FilteredResult> promResults = new List<FilteredResult>();
                TimeChecker.BeginMeasurement("FILTER");
                promResults.AddRange(FilterResults(pathResults, difficultes));
                TimeChecker.EndMeasurement("FILTER");

                for (int i = 0; i < promResults.Count; i++)
                {
                    bool isSuitable = false;
                    foreach (var key in difficultes.Keys)
                    {
                        if (key == 0)
                        {
                            isSuitable = true;
                            break;
                        }
                        if ((key < promResults[i].pathFindData.difficulty + 0.5f) && (key > promResults[i].pathFindData.difficulty - 0.5f))
                        {
                            isSuitable = true;
                            break;
                        }

                    }

                    if (!isSuitable)
                    {
                        promResults.RemoveAt(i);
                        i--;
                    }
                }

                bool isFounded = false;
                foreach (var promResult in promResults)
                {
                    foreach (var difficulty in difficultes.Keys)
                    {
                        if ((promResult.pathFindData.difficulty < difficulty + 0.5f) && (promResult.pathFindData.difficulty > difficulty - 0.5f) || difficulty == 0)
                        {
                            TilemapCreator.instance.CreateMap(promResult.hexMap, promResult.fruits, promResult.pathFindData);
                            isFounded = true;
                            //filteredResults.Add(promResult);
                            // TilemapCreator.instance.CreateTilemap(promResult.hexMap);
                            verResults.Add(promResult.hexMap);
                            difficultes.TryGetValue(difficulty, out var result);
                            for (int i = 0; i < percentIndentity.Count; i++)
                            {
                                percentIndentity[i] = 0;
                            }
                            if (result > 1)
                            {
                                difficultes.Remove(difficulty);
                                difficultes.Add(difficulty, result - 1);
                            }
                            else
                            {
                                difficultes.Remove(difficulty);
                            }
                            int count = verResults.Count - percentIndentity.Count;
                            for (int i = 0; i < count; i++)
                            {
                                percentIndentity.Add(0);
                            }
                            break;
                        }
                    }

                    if (difficultes.Count == 0)
                    {
                        return;
                    }
                    if (isFounded)
                    {
                        break;
                    }
                }

                return;
            }
            if (results == null || results.Count == 0)
            {
                verResults = new List<BaseFakeHexType[,]>();
                results = new List<BaseFakeHexType[,]>();
                results.Add(new BaseFakeHexType[fieldSize.x, this.fieldSize.y]);
                percentIndentity = new List<float>();
            }

            //IHexType[fieldSize.x, fieldSize.y]
            int countPatterns = hexes.Count;
            bool success = true;
            int countResults = results.Count;
            List<BaseFakeHexType[,]> newResults = new List<BaseFakeHexType[,]>();
            bool fail = false;
            int currentCountResult = verResults.Count;
            foreach (var pattern in hexes)
            {
               // List<float> promPercentIdentity = new List<float>(percentIndentity);
                if (verResults.Count > currentCountResult)
                {
                    for (int i = 0; i < percentIndentity.Count; i++)
                    {
                        percentIndentity[i] = 0;
                    }
                    int index = 0;
                    
                    for (int i = 0; i < verResults.Count - currentCountResult; i++)
                    {
                        if (percentIndentity.Count < verResults.Count)
                        {
                            percentIndentity.Add(0);
                        }

                        for (int x = 0; x < verResults[currentCountResult + i].GetLength(0); x++)
                        {
                            for (int y = 0; y < verResults[currentCountResult + i].GetLength(1); y++)
                            {
                                if (verResults[currentCountResult + i][x, y] == null ||
                                    verResults[currentCountResult + i][x, y].GetType() == typeof(FakeEmptyHex)||
                                    results[0][x, y]==null||
                                    results[0][x, y].GetType()==typeof(FakeEmptyHex)
                                    )
                                {
                                    continue;
                                }

                                if (verResults[currentCountResult + i][x, y].GetType() ==results[0][x,y].GetType() )
                                {
                                    percentIndentity[index] +=
                                        1f / ((float)hexesCount);
                                    if (percentIndentity[index] > percentIdentity)
                                    {
                                        fail = true;
                                        break;
                                    }
                                }

                            }

                            if (fail)
                            {
                                break;
                            }
                        }

                        if (fail)
                        {
                            break;
                        }

                        index++;
                    }
                }

                if (fail)
                {
                    continue;
                }

                List<BaseFakeHexType[,]> promResults = new List<BaseFakeHexType[,]>();
                for (int result = 0; result < countResults; result++)
                {

                    /*  for (int pattern = 0; pattern < countPatterns; pattern++)
                      {*/
                    success = true;


                    Position startCoordinate = pattern.Key[0].Position;
                    Vector2Int? successCoordinates = null;


                    BaseFakeHexType[,] promResult = (BaseFakeHexType[,])results[result].Clone();
                    bool hexPlaceFound = false;
                    for (int x = 0; x < fieldSize.x; x++)
                    {
                        for (int y = 0; y < fieldSize.y; y++)
                        {
                            TimeChecker.BeginMeasurement("HEXES");
                            if (results[result][x, y] == null)
                            {
                                if (x + pattern.Key[0].Position.x - startCoordinate.x >= 0 && x + pattern.Key[0].Position.x -
                                                                                startCoordinate.x <
                                                                                promResult.GetLength(0)
                                                                                && y + pattern.Key[0].Position.y -
                                                                                startCoordinate.y >= 0 &&
                                                                                y + pattern.Key[0].Position.y -
                                                                                startCoordinate.y <
                                                                                promResult.GetLength(1))
                                {
                                    if (promResult[x + pattern.Key[0].Position.x - startCoordinate.x,
                                            y + pattern.Key[0].Position.y - startCoordinate.y] == null)
                                    {
                                        Vector2Int pos = new Vector2Int(x + pattern.Key[0].Position.x - startCoordinate.x, y + pattern.Key[0].Position.y - startCoordinate.y);
                                        if (verResults != null)
                                        {
                                            int index = 0;
                                            foreach (var res in verResults)
                                            {
                                                if (res[pos.x, pos.y] == null || res[pos.x, pos.y].GetType() == typeof(FakeEmptyHex))
                                                {
                                                    continue;
                                                }

                                                if (res[pos.x, pos.y].GetType() == pattern.Key[0].GetType())
                                                {
                                                    percentIndentity[index] +=
                                                        1f / ((float)hexesCount);
                                                    if (percentIndentity[index] > percentIdentity)
                                                    {
                                                        success = false;
                                                        break;
                                                    }
                                                }

                                                index++;
                                            }

                                            if (!success)
                                            {
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            results = new List<BaseFakeHexType[,]>();
                                            percentIndentity = new List<float>();
                                        }
                                        hexPlaceFound = true;

                                        promResult[pos.x, pos.y] = pattern.Key[0].ShallowCopy();
                                        promResult[pos.x, pos.y].Position = new Position(x, y);


                                        successCoordinates = new Vector2Int(x, y);
                                        break;
                                    }
                                    else
                                    {
                                        success = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    success = false;
                                    break;
                                }
                            }
                            TimeChecker.EndMeasurement("HEXES");
                            if (!success)
                            {
                                break;
                            }

                        }

                        if (hexPlaceFound)
                        {
                            TimeChecker.BeginMeasurement("HEXES");
                            var count = pattern.Key.Count;
                            for (int i = 1; i < count; i++)
                            {
                                if (successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x >
                                    promResult.GetLength(0) - 1)
                                {
                                    success = false;
                                    break;
                                }
                                if (successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y >
                                    promResult.GetLength(1) - 1)
                                {
                                    success = false;
                                    break;
                                }

                                if (successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x ==-1 ||
                                    successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y ==-1)
                                {
                                    int v = 5;
                                    v++;
                                }
                                Debug.LogError("x "+(successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x)+
                                               "y "+ (successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y));
                                if (successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x < 0 ||
                                    successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y < 0)
                                {
                                    success = false;
                                    break;
                                }
                                if (promResult[successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x,
                                            successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y] == null)
                                {
                                    promResult[successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x,
                                            successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y] =
                                        pattern.Key[i].ShallowCopy();
                                    promResult[successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x,
                                        successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y].Position =
                                        new Position(successCoordinates.Value.x + pattern.Key[i].Position.x - startCoordinate.x, successCoordinates.Value.y + pattern.Key[i].Position.y - startCoordinate.y);
                                }
                                else
                                {
                                    success = false;
                                    break;
                                }
                            }
                            TimeChecker.EndMeasurement("HEXES");
                            break;
                        }
                        if (!success)
                        {
                            break;
                        }
                    }
                    if (success)
                    {
                        promResults.Add(promResult);
                    }
                }

                if (promResults.Count > 0)
                {
                    Dictionary<List<BaseFakeHexType>, int> nextHexes = new Dictionary<List<BaseFakeHexType>, int>(hexes); //hexes.ToArray().ToDictionary();
                    // nextHexes.Remove()
                    nextHexes.TryGetValue(pattern.Key, out var value);
                    if (value > 1)
                    {
                        nextHexes.Remove(pattern.Key);
                        nextHexes.Add(pattern.Key, value - 1);
                    }
                    else
                    {
                        nextHexes.Remove(pattern.Key);
                    }

                    GenerateMapRecurse(promResults, nextHexes, fieldSize, difficultes, hexesCount, percentIndentity);
                    // results.AddRange(promResults);
                    if (difficultes.Count == 0)
                    {
                        return;
                    }
                }
            }
            // }
            results = newResults;
        }
    }
#endif
}
