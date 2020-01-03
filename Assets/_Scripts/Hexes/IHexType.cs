using System;
using System.Collections;
using Assets.Scripts.Cells;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public interface IHexType
    {
        Position Position { get; set; }
        int Layer { get; set; }
        GameObject Model { get; set; }
        GameObject Instance { get; set; }
        void OnLeaveHex(Position nextHex);
        void OnEnterHex(Position previousCoordinate);
        bool isDestoyeble();
        TileBase GetTile();
        void OnLaserHit(Position previousPosition,int rangeInAir,int range);


    }

    [Serializable]
    public struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position Clone()
        {
            return new Position(x,y);
        }
    }
   
}
