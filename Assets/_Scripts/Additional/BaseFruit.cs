
using Assets._Scripts.Additional;
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts
{
    [Serializable]
    public abstract class BaseFruit: SerializedMonoBehaviour,IAdditional
    {
        public Vector2Int position;
        public int layer;
        public GameObject instance;
        [HideInInspector]
        public int countPasses=1;
        public BaseFruit(Vector2Int position,int layer)
        {
            this.position = position;
            this.layer = layer;
            countPasses = 1;
        }

        public void Initialize()
        {
            countPasses = 1;
        }
#if (UNITY_EDITOR)
        public abstract TileBase GetTile();
#endif

        public IAdditional ShallowCopy()
        {
            return (IAdditional)this.MemberwiseClone();
        }
    }
}
