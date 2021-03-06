﻿using Assets.Scripts;
using System.Collections.Generic;
using Assets.Scripts.Cells;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;
using System.IO;
using Assets._Scripts;
using UnityEngine.SceneManagement;
using Assets;
using Assets._Scripts.Additional;
using Assets._Scripts.FakeHexes;
using Assets._Scripts.Hexes;
using DG.Tweening;

public class LevelManager :Singleton<LevelManager>
{
    public Action pathNotFoundAction;
    public Material borderMaterial;
    LineRenderer border;
    public CameraController cameraController;
    private MoveHero moveHero;

    private List<BaseHexType> hexes = new List<BaseHexType>();
    private List<TileBase> hexesTypes = new List<TileBase>();

    //public delegate void WinDelegate();
    public static Action WinEvent;

    private static int countDestroyable;

    [SerializeField]
    public Tilemap itemTilemap;
    public Tilemap levelTilemap;
    public GameObjectData gameObjectData;
    private List<GameObject> selectedCells;

    /// <summary>
    /// палитра хексов
    /// </summary>
    private GameObject hexPrefab;

    private List<BaseFruit> fruits;

    public List<BaseFruit> GetFruitList()
    {
        return fruits;
    }
    public List<BaseFakeHexType> MakeListFakeHexes()
    {
        List<BaseFakeHexType> results=new List<BaseFakeHexType>();
        foreach (var hex in hexes)
        {
            results.Add(hex.GetFakeHex());
        }
        return results;
    }
    public bool TryFindFruit(Position position, int layer, out BaseFruit fruitResult)
    {
        foreach (var fruit in fruits)
        {
            if (fruit.position.x == position.x && fruit.position.y == position.y && fruit.layer == layer)
            {
                fruitResult = fruit;
                return true;
            }
        }
        fruitResult = null;
        return false;
    }

    public bool TryEatFruit(Position position, int layer)
    {
        for (int i=0;i<fruits.Count;i++)
        {
            if (fruits[i].position.x == position.x && fruits[i].position.y == position.y && fruits[i].layer == layer)
            {
                fruits[i].OnEat();

                //Destroy(fruits[i].instance);
                if (fruits[i].CountPasses == 0)
                {
                    fruits.RemoveAt(i);
                    StartCoroutine(CheckWinCondition());
                }

                return true;
            }
        }
        /*foreach (var fruit in fruits)
        {
            if (fruit.position.x == position.x && fruit.position.y == position.y && fruit.layer == layer)
            {
                Destroy(fruit.instance);
                fruits.Remove(fruit);
                StartCoroutine(CheckWinCondition());
                return true;
            }
        }*/
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        selectedCells=new List<GameObject>();
        fruits=new List<BaseFruit>();
        if (SceneManager.sceneCount == 1)
        {
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
      
        BaseHexType.OnChangeHexEvent = ChangeHex;
        BaseHexType.OnCreateHexEvent = CreateHex;
        moveHero = FindObjectOfType<MoveHero>();
        BaseHexType.OnDestroyHexEvent = null;
        BaseHexType.OnDestroyHexEvent += moveHero.CheckHex;
        BaseHexType.OnDestroyHexEvent += DestroyHex;
        hexPrefab = Resources.Load<GameObject>(Constants.HEX_PATH);
        LoadLevelTiles();
        LoadPalitre();
        LoadItemTiles();
        itemTilemap.gameObject.SetActive(false);
        levelTilemap.gameObject.SetActive(false);
    }

    public void LoadFruits()
    {
        foreach (var fruit in fruits)
        {
            fruit.CreateFruit();
           // fruit.instance=Instantiate(gameObjectData.strawberry,itemTilemap.GetCellCenterWorld(new Vector3Int(fruit.position.x, fruit.position.y, 0)),Quaternion.identity);
         //   Debug.LogError("straw " +itemTilemap.GetCellCenterLocal(new Vector3Int(fruit.position.x, fruit.position.y, 0)));
        }
    }

    public void LoadHexes()
    {

    }
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

    void LoadItemTiles()
    {
        Position startPosition=new Position(0,0);
        for (int x = itemTilemap.cellBounds.xMin; x < itemTilemap.cellBounds.xMax; x++)
        {
            for (int y = itemTilemap.cellBounds.yMin; y < itemTilemap.cellBounds.yMax; y++)
            {

                TileBase tileBase =
                    itemTilemap.GetTile(new Vector3Int(x, y, 0));
                if (tileBase == null)
                {
                    continue;
                }
                switch (tileBase.name)
                {
                    case "StartHex":
                        moveHero.SetHeroPosition(new Position(x,y),true);
                       startPosition= new Position(x, y);
                        break;
                    case "Strawberry":
                       fruits.Add(new Strawberry(new Vector2Int(x,y),0));
                        break;
                    case "banana":
                        fruits.Add(new Banana(new Vector2Int(x,y),0 ));
                        break;
                }            
            }
        }

        SelectCells(startPosition);
        LoadFruits();
    }
    void LoadLevelTiles()
    {
        for (int x = levelTilemap.cellBounds.xMin; x < levelTilemap.cellBounds.xMax; x++)
        {
            for (int y = levelTilemap.cellBounds.yMin; y < levelTilemap.cellBounds.yMax; y++)
            {

                TileBase tileBase =
                    levelTilemap.GetTile(new Vector3Int(x, y, 0));
                if (tileBase == null)
                {
                    continue;
                }

                BaseHexType hextype = null;
                switch (tileBase.name)
                {
                    case "DoubleHex":
                        hextype = new DoubleHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.grass;
                        break;
                    case "NormalHex":
                        hextype = new NormalHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.dirt;
                        break;
                    case "UndestractableHex":
                        hextype = new UndestractableHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.stone;
                        break;
                    case "CreatingAroundHex":
                        hextype = new CreatingAroundHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.exploder;
                        break;
                    case "CreatingAwayHex":
                        hextype = new CreatingAwayHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.hex3x;
                        break;
                    case "ClockwiseRotatingHex":
                        hextype = new RotatingHex(new Position(x, y), 0, true);
                        hextype.Model = gameObjectData.turnerRight;
                        break;
                    case "CounterClockwiseRotateHex":
                        hextype = new RotatingHex(new Position(x, y), 0, false);
                        hextype.Model = gameObjectData.turnerLeft;
                        break;
                    case "IceHex":
                        hextype = new IceHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.ice;
                        break;
                    case "LaserHex":
                        hextype = new LaserHex(new Position(x, y), 0);
                        break;
                    case "ImpassableHex":
                        hextype=new ImpassableHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.doubleStone;
                        break;
                    case "borderHex":
                        hextype = new CameraExtensionHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.cameraExtension;
                        break;
                    case "SnowHex":
                        hextype = new SnowHex(new Position(x, y), 0);
                        hextype.Model = gameObjectData.snow;
                        break;
                }

                if (hextype != null)
                {
                    hexes.Add(hextype);
                    if (hextype.isDestoyeble())
                    {
                        countDestroyable++;
                    }
                }
            }
        }

        Vector2 min=new Vector2(9999,9999);
        Vector2 max=new Vector2(-9999,-9999);
        foreach (var hex in hexes)
        {
           hex.Instance= Instantiate(hex.Model,
                levelTilemap.GetCellCenterWorld(new Vector3Int(hex.Position.x, hex.Position.y, hex.Layer)),hex.Model.transform.rotation);
#if UNITY_EDITOR
            var tilemapMarker=hex.Instance.AddComponent<TilemapMarker>();
            tilemapMarker.position = new Vector2Int(hex.Position.x, hex.Position.y);
#endif
            if (hex.Instance.transform.position.x > max.x)
           {
               max.x = hex.Instance.transform.position.x;
           }
           if (hex.Instance.transform.position.x < min.x)
           {
               min.x = hex.Instance.transform.position.x;
           }
           if (hex.Instance.transform.position.y > max.y)
           {
               max.y = hex.Instance.transform.position.y;
           }
           if (hex.Instance.transform.position.y < min.y)
           {
               min.y = hex.Instance.transform.position.y;
           }           
        }
        if (hexes.Count != 0)
        {
            cameraController.SetCamera(min, max,hexes[0].Instance.transform.position.z);
        }
    }
   
    public bool TryGetHex(Position position, int layout, out BaseHexType outHex)
    {
        /* if (Math.Abs(moveHero.HeroPosition.x - position.x) <= 1 && Math.Abs(moveHero.HeroPosition.y - position.y) <= 1)
         {*/
        foreach (var hex in hexes)
        {
            if (hex.Position.x == position.x && hex.Position.y == position.y)
            {
                outHex = hex;
                return true;
            }
        }
        /* }*/

        outHex = null;
        return false;
    }

    public IHexType FindHexOnGameObject(GameObject go)
    {
        foreach (var hex in hexes)
        {
            if (hex.Instance == go)
            {
                return hex;
            }
        }
        return null;
    }
    /* public bool TryGetHex(Vector3 worldPosition, int layout, out IHexType outHex)
     {
         Vector3Int position=map.LocalToCell(worldPosition);
         Vector3Int heroPosition = map.LocalToCell(new Vector3(moveHero.WorldPosition.x, moveHero.WorldPosition.y,0));
         if (Math.Abs( heroPosition.x- position.x) <= 1 && Math.Abs(moveHero.HeroPosition.y - position.y) <= 1)
         {
             foreach (var hex in hexes)
             {
                 if (hex.Position.x == position.x && hex.Position.y == position.y)
                 {
                     outHex = hex;
                     return true;
                 }
             }
         }

         outHex = null;
         return false;
     }
     */
    public void CreateHex(BaseHexType hexType)
    {
        foreach (var hex in hexes)
        {
            if (hex.Position.x == hexType.Position.x&& hex.Position.y == hexType.Position.y)
            {
                return;
            }
        }
        hexes.Add(hexType);
        levelTilemap.SetTile(new Vector3Int(hexType.Position.x, hexType.Position.y, 0), hexType.GetTile());
        if (hexType.isDestoyeble())
        {
            countDestroyable++;
        }
        StartCoroutine(CreateHexCoroutine(hexType));
    }

   /* public void RotateHexes(RotatingHex hex)
    {
        Position[] positions = PositionCalculator.GetAroundSidePositions(hex.Position);
        var levelManager = GameObject.FindObjectOfType<LevelManager>();
        var hexes = new List<BaseHexType>();
        var newPositions = new List<Position>();
        var hexesForRotate = new List<IHexType>();
        foreach (var position in positions)
        {
            levelManager.TryGetHex(position, hex.Layer, out var reshex);
            if (hex != null)
            {
                hexesForRotate.Add(hex);
                hexes.Add(hex);
                newPositions.Add(PositionCalculator.GetAdjustmentPosition(position, !hex.isClockwiseRotating, hex.Position));
               // levelManager.DestroyHex(position, hex.Layer, Destroy);
            }
        }
        for (int i = 0; i < hexes.Count; i++)
        {
            hexes[i].Position = newPositions[i];
            levelManager.CreateHex(hexes[i]);
        }
    }*/

    public void RotateHexes(RotatingHex rotatingHex)
    {
        StartCoroutine(RotateHexesCoroutine(rotatingHex));
    }
    
    public IEnumerator RotateHexesCoroutine(RotatingHex rotatingHex)
    {
        Position[] positions = PositionCalculator.GetAroundSidePositions(rotatingHex.Position);
        var levelManager = GameObject.FindObjectOfType<LevelManager>();
        var promhexes = new List<BaseHexType>();
        var newPositions = new List<Position>();
        var newFruitsPositions = new List<Position>();
        var newPositionsVector = new List<Vector3>();
        var newFruitsPositionsVector = new List<Vector3>();
        var hexesForRotate = new List<IHexType>();
        var fruits = new List<BaseFruit>();
        var hexesScales = new List<Vector3>();
        var fruitsScales = new List<Vector3>();
 
        foreach (var position in positions)
        {
            levelManager.TryGetHex(position, rotatingHex.Layer, out var hex);
            if (levelManager.TryFindFruit(position, rotatingHex.Layer, out var fruit))
            {
                fruits.Add(fruit);
                var pos = PositionCalculator.GetAdjustmentPosition(position, !rotatingHex.isClockwiseRotating,
                    rotatingHex.Position);
                newFruitsPositions.Add(pos);
                newFruitsPositionsVector.Add(itemTilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0)));
            }
            if (hex != null)
            {
                if (hex.GetType() != typeof(CameraExtensionHex))
                {
                    var pos = PositionCalculator.GetAdjustmentPosition(position, !rotatingHex.isClockwiseRotating,
                        rotatingHex.Position);
                    var cameraExtensionList = new List<CameraExtensionHex>();
                    if (levelManager.TryGetHex(pos, rotatingHex.Layer, out var promhex))
                    {
                       
                        if (promhex.GetType() == typeof(CameraExtensionHex))
                        {
                            foreach (var findHex in hexes)
                            {
                                if (findHex.GetType() == typeof(CameraExtensionHex))
                                {
                                    cameraExtensionList.Add((CameraExtensionHex)findHex);
                                }
                            }
                          
                            Vector3 leftUp=new Vector3(100,-100);
                            Vector3 rightUp = new Vector3(-100,-100);
                            Vector3 leftDown = new Vector3(100,100);
                            Vector3 rightDown = new Vector3(-100,100);
                            foreach (var vertice in cameraExtensionList)
                            {
                                Vector3 posit =
                                    levelTilemap.GetCellCenterWorld(new Vector3Int(vertice.Position.x,
                                        vertice.Position.y, 0));
                                if (posit.x <= leftUp.x && posit.y >= leftUp.y)
                                {
                                    leftUp = new Vector3(posit.x,posit.y,-0.5f);
                                }
                                if (posit.x >= rightUp.x && posit.y >= rightUp.y)
                                {
                                    rightUp = new Vector3(posit.x, posit.y, -0.5f);
                                }
                                if (posit.x <= leftDown.x && posit.y <= leftDown.y)
                                {
                                    leftDown = new Vector3(posit.x, posit.y, -0.5f);
                                }
                                if (posit.x >= rightDown.x && posit.y <= rightDown.y)
                                {
                                    rightDown = new Vector3(posit.x, posit.y, -0.5f);
                                }
                            }
                           
                            border = GetComponent<LineRenderer>();
                            if (border == null)
                            {
                                gameObject.AddComponent<LineRenderer>();
                                border = GetComponent<LineRenderer>();                               
                            }
                            else
                            {
                                border.enabled = true;
                            }
                            border.positionCount = 5;
                            border.SetPositions(new Vector3[]{ leftUp ,rightUp,rightDown,leftDown,leftUp});
                            border.alignment = LineAlignment.TransformZ;
                            border.widthMultiplier = 0.2f;
                            border.material = borderMaterial;
                            rotatingHex.EndRotateAction.Invoke();
                            for (int j = 0; j < 2; j++)
                            {
                                for (int i = 0; i < 30; i++)
                                {
                                    border.startColor = new Color(1f, 0.4f - ((float) i) / 100,
                                        0.5f - ((float) i) / 100);
                                    border.endColor = new Color(1f, 0.4f - ((float) i) / 100, 0.4f - ((float) i) / 100);
                                    yield return new WaitForSeconds(0.02f);
                                }

                                for (int i = 0; i < 30; i++)
                                {
                                    border.startColor = new Color(1f,0.1f+ ((float) i) / 100, 0.1f + ((float) i) / 100);
                                    border.endColor = new Color(1f, 0.1f + ((float) i) / 100, 0.1f + ((float) i) / 100);
                                    yield return new WaitForSeconds(0.02f);
                                }
                            }

                            border.enabled = false;                            
                            yield break;
                        }
                    }

                    hexesForRotate.Add(hex);
                    promhexes.Add(hex);
                    newPositions.Add(pos);
                    newPositionsVector.Add(levelTilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0)));
                    //levelManager.DestroyHex(position, layer, Destroy);
                }
            }
          
        }
        for (int i = 0; i < promhexes.Count; i++)
        {
           // promhexes[i].Instance.transform.DOMoveZ(promhexes[i].Instance.transform.position.z - 1, 1f);
            hexesScales.Add(promhexes[i].Instance.transform.localScale);         
            promhexes[i].Instance.transform.DOScale(promhexes[i].Instance.transform.localScale * 0.6f, 0.5f);
            promhexes[i].Position =new Position(newPositions[i].x,newPositions[i].y); 
            levelManager.CreateHex(promhexes[i]);
        }
        for (int i = 0; i < fruits.Count; i++)
        {
            // promhexes[i].Instance.transform.DOMoveZ(promhexes[i].Instance.transform.position.z - 1, 1f);
            fruitsScales.Add(fruits[i].instance.transform.localScale);
            fruits[i].instance.transform.DOScale(fruits[i].instance.transform.localScale * 0.6f, 0.5f);
            fruits[i].position = new Vector2Int(newFruitsPositions[i].x, newFruitsPositions[i].y);
            levelManager.CreateHex(promhexes[i]);
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < promhexes.Count; i++)
        {
            promhexes[i].Instance.transform.DOMove(newPositionsVector[i], 1f);
        }
        for (int i = 0; i < fruits.Count; i++)
        {
            fruits[i].instance.transform.DOMove(newFruitsPositionsVector[i], 1f);
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < promhexes.Count; i++)
        {
            promhexes[i].Instance.transform.DOScale(hexesScales[i], 0.5f);
           
        }
        for (int i = 0; i < fruits.Count; i++)
        {
            fruits[i].instance.transform.DOScale(fruitsScales[i], 0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        SelectCells(rotatingHex.Position);
        rotatingHex.EndRotateAction.Invoke();
    }

    public IEnumerator CreateHexCoroutine(IHexType hex)
    {
        hex.Instance = GameObject.Instantiate(gameObjectData.dirt, levelTilemap.GetCellCenterWorld(new Vector3Int(hex.Position.x, hex.Position.y, 0)), gameObjectData.dirt.transform.rotation);
        hex.Instance.transform.localScale=new Vector3(0,0,0);
        hex.Instance.transform.DOScale(new Vector3(60, 69.2f, 60), 1f);
        yield return new WaitForSeconds(0);
    }

    public int FruitCount()
    {
        if (fruits != null)
        {
            return fruits.Count;
        }

        return 0;
    }
    public void ChangeHex(BaseHexType hexType,Func<IEnumerator> method)
    {
       // method();
        StartCoroutine(method());
        foreach (var hex in hexes)
        {
            if (hex.Position.x == hexType.Position.x&& hex.Position.y == hexType.Position.y)
            {
                hexType.Instance = hex.Instance;
                hexes.Add(hexType);
                hexes.Remove(hex);
                levelTilemap.SetTile(new Vector3Int(hexType.Position.x, hexType.Position.y, 0), hexType.GetTile());
                if (hex.isDestoyeble())
                {
                    countDestroyable--;
                }
                if (hexType.isDestoyeble())
                {
                    countDestroyable++;
                }
                return;
            }
        }
    }

    public void DestroyHex(Position position, int layout,Func<IEnumerator> method)
    {
        StartCoroutine(method());
        foreach (var hex in hexes)
        {
            if (hex.Position.x == position.x && hex.Position.y == position.y)
            {
                hexes.Remove(hex);
                levelTilemap.SetTile(new Vector3Int(hex.Position.x, hex.Position.y, 0), null);
                
                //hex.OnLeaveHex(
                if (hex.isDestoyeble())
                {
                    countDestroyable--;
                }
                return;
            }
        }
    }

    public void SelectCells(Position position)
    {
        if (FruitCount() == 0)
        {
            return;
        }
        Position[] positions = PositionCalculator.GetAroundSidePositions(position);
        if (HelpManager.instance.GetNumberAvailibleTips > 0)
        {
            bool isPathExist = HelpManager.instance.TryGetHelpStep(position, out var nextposition);
            if (isPathExist)
            {
                foreach (var pos in positions)
                {
                    LevelManager.instance.TryGetHex(pos, 0, out var hex);
                    if (hex != null && hex.IsPassable())
                    {
                        Vector3 posit = hex.Instance.transform.position;
                        posit.x -= 0.28f;
                        posit.y -= 0.5f;
                        posit.z -= 0.4f;
                        if (nextposition.x == pos.x /*+ HelpManager.instance.Offset.x*/ &&
                            nextposition.y == pos.y /*+ HelpManager.instance.Offset.y*/)
                        {

                            selectedCells.Add(Instantiate(gameObjectData.helpCell, posit, Quaternion.identity));
                        }
                        else
                        {
                            selectedCells.Add(Instantiate(gameObjectData.selectedCell, posit, Quaternion.identity));
                        }
                    }
                }
            }
            else
            {
                pathNotFoundAction?.Invoke();
            }
        }
        else
        {
            foreach (var pos in positions)
            {
                LevelManager.instance.TryGetHex(pos, 0, out var hex);
                if (hex != null && hex.IsPassable())
                {
                    Vector3 posit = hex.Instance.transform.position;
                    posit.x -= 0.28f;
                    posit.y -= 0.5f;
                    posit.z -= 0.4f;
                    selectedCells.Add(Instantiate(gameObjectData.selectedCell, posit, Quaternion.identity));                   
                }
            }
        }
    }

    public void DeselectCells()
    {
        foreach (var cell in selectedCells)
        {
            Destroy(cell);
        }
        selectedCells.Clear();
    }

    public IEnumerator CheckWinCondition()
    {
        if (fruits.Count == 0)
        {
           // Debug.Log("LOCK INPUT");
            ((MoveHero)MoveHero.instance).LockInput();
            // LevelManager.WinEvent?.Invoke();
            PlayerPrefs.SetString("current_level", SceneManager.GetActiveScene().name);           
             yield return new WaitForSeconds(0.5f);
             StartCoroutine(moveHero.WinMove());  
        }
    }
}

