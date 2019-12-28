﻿
using Assets.Scripts;
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
        void OnLeaveHex(ref BaseFakeHexType[,] map);
        void OnEnterHex(ref FakeMoveHero hero, ref BaseFakeHexType[,] map);
        bool isDestoyeble();
        TileBase GetTile();
        void OnLaserHit(Position previousPosition, int rangeInAir, int range);
    }
}
