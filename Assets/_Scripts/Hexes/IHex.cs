using UnityEngine;

namespace Assets.Scripts.Cells
{
    public interface IHex
    {
        Vector2Int Position { get; set; }
        int Layer { get; set; }
        void DestroyHex();
    }
}
