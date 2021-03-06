﻿
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
        public int CountPasses { get;protected set; }
        public BaseFruit(Vector2Int position,int layer)
        {
            this.position = position;
            this.layer = layer;
            CountPasses = 1;
        }
       /* public BaseFruit(Vector2Int position, int layer,int countPasses)
        {
            this.position = position;
            this.layer = layer;
            CountPasses = countPasses;
        }*/
        public void Initialize()
        {
            CountPasses = 1;
        }
#if (UNITY_EDITOR)
        public abstract TileBase GetTile();
#endif

        public IAdditional ShallowCopy()
        {
            return (IAdditional)this.MemberwiseClone();
        }

        public virtual void OnEat()
        {
            CountPasses--;
            Destroy(instance);
        }

        public virtual void OnFakeEat()
        {
            CountPasses--;
        }

        public abstract void CreateFruit();

    }
}
