using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Assets._Scripts.Additional
{
    [Serializable]
    public class Banana : BaseFruit
    {
        public Banana(Vector2Int position, int layer) : base(position, layer)
        {
            this.position = position;
            this.layer = layer;
            CountPasses = 2;
        }

        public override void CreateFruit()
        {
            instance = Instantiate(LevelManager.instance.gameObjectData.banana,
                LevelManager.instance.itemTilemap.GetCellCenterWorld(new Vector3Int(position.x,position.y, 0)), LevelManager.instance.gameObjectData.banana.transform.rotation);
        }

        public override void OnEat()
        {
            CountPasses--;
            if (CountPasses == 1)
            {
                Destroy(instance);
                instance = Instantiate(LevelManager.instance.gameObjectData.halfBanana,
                    LevelManager.instance.itemTilemap.GetCellCenterWorld(new Vector3Int(position.x, position.y, 0)), LevelManager.instance.gameObjectData.halfBanana.transform.rotation);
            }
            else
            {
                Destroy(instance);
            }          
        }
#if (UNITY_EDITOR)
        public override TileBase GetTile()
        {
            throw new NotImplementedException();
        }
#endif
    }
}

