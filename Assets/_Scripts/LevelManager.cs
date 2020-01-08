using Assets.Scripts;
using System.Collections.Generic;
using Assets.Scripts.Cells;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.CodeDom;
using System.Collections;
using System.IO;
using Assets._Scripts;
using UnityEngine.SceneManagement;
using Assets;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{

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

    /*   [SerializeField]
       private Tilemap levelTilemap;*/

    /// <summary>
    /// палитра хексов
    /// </summary>
    private GameObject hexPrefab;

    private List<BaseFruit> fruits;

    public bool FindFruitAndRemove(Position position, int layer, out BaseFruit fruitResult)
    {
        foreach (var fruit in fruits)
        {
            if (fruit.position.x == position.x&& fruit.position.y == position.y && fruit.layer == layer)
            {
                fruitResult = fruit;
                Destroy(fruit.instance);
                fruits.Remove(fruit);
                StartCoroutine(CheckWinCondition());             
                return true;
            }
        }
        fruitResult = null;
        return false;
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

    public bool TryRemoveFruit(Position position, int layer)
    {
        for (int i=0;i<fruits.Count;i++)
        {
            if (fruits[i].position.x == position.x && fruits[i].position.y == position.y && fruits[i].layer == layer)
            {
                Destroy(fruits[i].instance);
                fruits.RemoveAt(i);
                StartCoroutine(CheckWinCondition());
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
            fruit.instance=Instantiate(gameObjectData.fruit,itemTilemap.GetCellCenterWorld(new Vector3Int(fruit.position.x, fruit.position.y, 0)),Quaternion.identity);
            Debug.LogError("straw " +itemTilemap.GetCellCenterLocal(new Vector3Int(fruit.position.x, fruit.position.y, 0)));
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
                        break;
                    case "Strawberry":
                       fruits.Add(new Strawberry(new Vector2Int(x,y),0));
                        break;                   
                }            
            }
        }
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

    public void RotateHexes(RotatingHex hex)
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
    }

    public IEnumerator CreateHexCoroutine(IHexType hex)
    {
        hex.Instance = GameObject.Instantiate(gameObjectData.dirt, levelTilemap.GetCellCenterWorld(new Vector3Int(hex.Position.x, hex.Position.y, 0)), gameObjectData.dirt.transform.rotation);
        hex.Instance.transform.localScale=new Vector3(0,0,0);
        hex.Instance.transform.DOScale(new Vector3(60, 69.2f, 60), 1f);
        yield return new WaitForSeconds(0);
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

    public IEnumerator CheckWinCondition()
    {
        if (fruits.Count == 0)
        {
            ((MoveHero)MoveHero.instance).LockInput();
            StartCoroutine(moveHero.WinMove());
            yield return new WaitForSeconds(0.5f);         
        }
    }
}

