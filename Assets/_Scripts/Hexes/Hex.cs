
using Assets.Scripts.Cells;
using UnityEngine;

namespace Assets.Scripts
{
    public class Hex<T>:IHex where  T:IHexType
    {
        public T HexType { get; set; }
        public Vector2Int Position { get; set; }
        public int Layer { get; set; }

        public Hex(Vector2Int position, int layer)
        {
            Position = position;
            Layer = layer;
        }
        public Hex()
        { }

        public void DestroyHex()
        {
            HexType.OnLeaveHex();
        }
    }
}
