using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public abstract class BaseHexType : IHexType
    {
        public Position Position { get; set; }
        public int Layer { get; set; }
        public GameObject Model { get; set; }
        public GameObject Instance { get; set; }
        public BaseHexType(Position position, int layer)
        {
            Position = position;
            Layer = layer;
        }
        public BaseHexType()
        { }

      //  public delegate void DestroyHexDelegate(Vector2Int position,int layer);
        public static Action<Position,int,Func<IEnumerator>> OnDestroyHexEvent;
       // public delegate void ChangeHexDelegate(IHexType hex, Func<IEnumerator> method);
        public static Action<IHexType, Func<IEnumerator>> OnChangeHexEvent;
        public delegate void CreateHexDelegate(IHexType hex);
        public static Action<IHexType> OnCreateHexEvent;

        public abstract void OnLeaveHex();

        protected void OnDestroyHex(Position position,int layer,Func<IEnumerator> method)
        {
            OnDestroyHexEvent(position,layer,method);
        }

        protected void OnCreateHex(IHexType hex)
        {
            OnCreateHexEvent(hex);
        }
        protected void OnChangeHex(IHexType hex,Func<IEnumerator> method)
        {
            OnChangeHexEvent(hex, method);
        }
        public abstract bool isDestoyeble();

        public abstract TileBase GetTile();

        public abstract void OnEnterHex(Position previousCoordinate);
        public virtual void OnLaserHit(Position previousPosition,int rangeInAir,int range)
        {
            OnDestroyHex(Position, Layer,Destroy);
        }
        public IEnumerator Destroy()
        {
            yield return new WaitForSeconds(0);
        }

        public BaseHexType ShallowCopy()
        {
            return (BaseHexType)this.MemberwiseClone();
        }
    }
}
