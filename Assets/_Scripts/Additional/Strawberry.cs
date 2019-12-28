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
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.STRAWBERRY);
        }
#endif
    }
}
