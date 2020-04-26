using Assets._Scripts.FakeHexes;
using Assets.Scripts;
using Assets.Scripts.Cells;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.Hexes
{
    public class ImpassableHex : BaseHexType
    {
        public ImpassableHex(Position position, int layer) : base(position, layer)
        {
        }
        public override TileBase GetTile()
        {
            return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.IMPASSABLE_HEX);
        }

        public override bool isDestoyeble()
        {
            return false;
        }
        public override bool IsPassable()
        {
            return false;
        }

        public override BaseFakeHexType GetFakeHex()
        {
            return new FakeImpassableHex(Position, Layer);
        }
    }
    
}
