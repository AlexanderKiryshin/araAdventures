using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.Additional
{
    public class StartPosition : IAdditional
    {
        public IAdditional ShallowCopy()
        {
            return (IAdditional)this.MemberwiseClone();
        }
#if (UNITY_EDITOR)
        public TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.START);
        }
#endif
    }
}
