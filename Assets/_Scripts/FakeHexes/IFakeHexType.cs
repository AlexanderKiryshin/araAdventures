
using Assets._Scripts.Additional;
using Assets._Scripts.Model;
using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
    public interface IFakeHexType
    {
        Position Position { get; set; }
        int Layer { get; set; }
        GameObject Model { get; set; }
        GameObject Instance { get; set; }
        //void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map);
        //void OnEnterHex(Position previousCoordinate,ref FakeMoveHero hero, ref BaseFakeHexType[,] map);
        void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref Dictionary<Position, HexWithPasses> map,
            ref Dictionary<Position, IAdditional> fruitMap);
        void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref Dictionary<Position,
            HexWithPasses> map, ref Dictionary<Position, IAdditional> fruitMap);
        bool isDestoyeble();
        bool IsPassable();
        bool IsService();
#if (UNITY_EDITOR)
        TileBase GetTile();
#endif
        void OnLaserHit(Position previousPosition, int rangeInAir, int range);
    }
}

