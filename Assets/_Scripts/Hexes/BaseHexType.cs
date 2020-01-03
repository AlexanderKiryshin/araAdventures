﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public abstract class BaseHexType : IHexType
    {
        public Action<Position,Position> LeaveHexEvent;
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
        public static Action<BaseHexType, Func<IEnumerator>> OnChangeHexEvent;
        public delegate void CreateHexDelegate(IHexType hex);
        public static Action<BaseHexType> OnCreateHexEvent;
        
        public virtual void OnLeaveHex(Position nextHex)
        {
            ((MoveHero)MoveHero.instance).EndMove += OnLeaveHexEvent;
           ((MoveHero)MoveHero.instance).Move(nextHex, 0.5f);
            ((MoveHero)MoveHero.instance).nextPosition = nextHex;
        }
        public virtual void OnLeaveHexEvent()
        {
            LeaveHexEvent?.Invoke(((MoveHero)MoveHero.instance).nextPosition, Position);
            ((MoveHero)MoveHero.instance).EndMove -= OnLeaveHexEvent;
        }
        protected void OnDestroyHex(Position position,int layer,Func<IEnumerator> method)
        {
            OnDestroyHexEvent(position,layer,method);
        }

        protected void OnCreateHex(BaseHexType hex)
        {
            OnCreateHexEvent(hex);
        }
        protected void OnChangeHex(BaseHexType hex,Func<IEnumerator> method)
        {
            OnChangeHexEvent(hex, method);
        }
        public abstract bool isDestoyeble();

        public abstract TileBase GetTile();

        public virtual void OnEnterHex(Position previousCoordinate)
        {
            var levelManager = GameObject.FindObjectOfType<LevelManager>();
             bool fruitIsFound=levelManager.TryFindFruit(Position, Layer, out var fruit);
            if (fruitIsFound)
            {
                ((MoveHero)MoveHero.instance).EndMove += OnEnterHexEvent;
                ((MoveHero)MoveHero.instance).EatWithMove(Position);
                SetHeroPositionWithEat(new Position(hex.Position.x, hex.Position.y),layer);
                yield return new WaitForSeconds(TIME_WALK);
            }
            else
            {
                SetHeroPosition(new Position(hex.Position.x, hex.Position.y), false);
                yield return new WaitForSeconds(TIME_WALK);
            }*/
            ((MoveHero)MoveHero.instance).EndMove += OnEnterHexEvent;
            ((MoveHero)MoveHero.instance).Move(Position, 1f);
        }
        public void OnEnterHexEvent()
        {
            ((MoveHero)MoveHero.instance).SetIdleAnimation();
            ((MoveHero)MoveHero.instance).SetNextPosition();
           ((MoveHero)MoveHero.instance).EndMove -= OnEnterHexEvent;
        }
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
