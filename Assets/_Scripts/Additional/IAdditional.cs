using System;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.Additional
{
    public interface IAdditional
    {
        IAdditional ShallowCopy();
#if (UNITY_EDITOR)
        TileBase GetTile();
#endif
    }
}
