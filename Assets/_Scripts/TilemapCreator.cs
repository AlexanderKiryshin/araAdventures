using Assets._Scripts.FakeHexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using Assets._Scripts.Additional;
using Assets._Scripts.Model;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Assets._Scripts
{
#if (UNITY_EDITOR)
    public class TilemapCreator : Singleton<TilemapCreator>
    {
        public static int number = 0;
        public void CreateMap(BaseFakeHexType[,] hexes, IAdditional[,] fruits, PathFindData path)
        {
            number++;
            if (!TryGetComponent(typeof(Grid), out var grid))
            {
                var gridTilemap = gameObject.AddComponent<Grid>();
                gridTilemap.cellSize = new Vector3(1.2f, 1.2f, 1);
                gridTilemap.cellSwizzle = GridLayout.CellSwizzle.YXZ;
            }
            var asset = ScriptableObject.CreateInstance<PathFindData>();
            asset = path;
            AssetDatabase.CreateAsset(asset, "Assets/_prefab/maps/" + SceneManager.GetActiveScene().name + "_" + number + ".asset");
            AssetDatabase.SaveAssets();
            var variant=new GameObject();
            variant.SetActive(false);
             variant.transform.SetParent(gameObject.transform);
             variant.name = SceneManager.GetActiveScene().name + "_" + number;
            /* var pathFindData = variant.AddComponent<PathFindData>();
             pathFindData = path;*/
             var mapGO = new GameObject();
             mapGO.transform.SetParent(variant.transform);
             mapGO.name = "map";
             var map = mapGO.AddComponent<Tilemap>();
             mapGO.AddComponent<TilemapRenderer>();
             map.tileAnchor = Vector3.zero;

             for (int x = 0; x < hexes.GetLength(0); x++)
             {
                 for (int y = 0; y < hexes.GetLength(1); y++)
                 {
                     if (x % 2 == 0)
                     {
                         map.SetTile(new Vector3Int(x, y, 0), hexes[x, y].GetTile());
                     }
                     else
                     {
                         map.SetTile(new Vector3Int(x, y, 0), hexes[x, y].GetTile());
                     }
                 }
             }

             var fruitmapGO = new GameObject();
             fruitmapGO.transform.SetParent(variant.transform);
             fruitmapGO.name = "fruitMap";
             var fruitmap = fruitmapGO.AddComponent<Tilemap>();
             fruitmapGO.AddComponent<TilemapRenderer>();

             fruitmap.tileAnchor = Vector3.zero;

             for (int x = 0; x < hexes.GetLength(0); x++)
             {
                 for (int y = 0; y < hexes.GetLength(1); y++)
                 {
                     if (fruits[x, y] != null)
                     {
                         if (x % 2 == 0)
                         {
                             fruitmap.SetTile(new Vector3Int(x, y, 0), fruits[x, y].GetTile());
                         }
                         else
                         {
                             fruitmap.SetTile(new Vector3Int(x, y, 0), fruits[x, y].GetTile());
                         }
                     }
                 }
             }
           /* var prefab = PrefabUtility.SaveAsPrefabAsset(variant,
                "Assets/_prefab/maps/" + SceneManager.GetActiveScene().name + "_" + number + ".prefab");*/
            /* tilemap.SetTile(new Vector3Int(0, 0, 0), hexes[0, 0].GetTile());
             tilemap.SetTile(new Vector3Int(-1, 0, 0), hexes[0, 0].GetTile());
             tilemap.SetTile(new Vector3Int(-1, 1, 0), hexes[0, 0].GetTile());
             tilemap.SetTile(new Vector3Int(-2, 1, 0), hexes[0, 0].GetTile());*/
        }
    }
#endif
}
