using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets._Scripts.Additional;
using Assets.Scripts;
using Assets.Scripts.Cells;
using Assets._Scripts.Model;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
    public abstract class BaseFakeHexType : IFakeHexType,ICopy<BaseFakeHexType>
    {
        public Position Position { get; set; }
        public int Layer { get; set; }
        public GameObject Model { get; set; }
        public GameObject Instance { get; set; }

        public BaseFakeHexType(Position position, int layer)
        {
            Position = position;
            Layer = layer;
        }
        public BaseFakeHexType()
        { }
#if (UNITY_EDITOR)
        public abstract TileBase GetTile();
#endif


        public bool isDestoyeble()
        {
            throw new NotImplementedException();
        }

        public virtual void OnEnterHex(Position previousCoordinate,ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        { }

        public void OnLaserHit(Position previousPosition, int rangeInAir, int range)
        {
            throw new NotImplementedException();
        }

        public BaseFakeHexType ShallowCopy()
        {
            return (BaseFakeHexType)this.MemberwiseClone();
        }

        public virtual void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        { }

        public virtual bool IsPassable()
        {
            return true;
        }

        public virtual void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
        }

        public virtual void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref Dictionary<Position, HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap)
        {
        }

        public virtual bool NeedMakeOnLeaveHex()
        {
            return true;
        }

        public abstract HexEnum GetHexEnum();

        public virtual bool IsService()
        {
            return false;
        }
    }
}
