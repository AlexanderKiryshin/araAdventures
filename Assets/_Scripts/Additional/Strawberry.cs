using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts
{
    [Serializable]
    public class Strawberry : BaseFruit
    {
        public Strawberry(Vector2Int position, int layer) : base(position, layer)
        {
            
        }

        public override void CreateFruit()
        {
            instance = Instantiate(LevelManager.instance.gameObjectData.strawberry,
                LevelManager.instance.itemTilemap.GetCellCenterWorld(new Vector3Int(position.x, position.y, 0)), Quaternion.identity);
        }   
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.STRAWBERRY);
        }
#endif
    }
}
