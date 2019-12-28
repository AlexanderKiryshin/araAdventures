﻿
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Cells
{
    public class UndestractableHex : BaseHexType
    {
        public UndestractableHex(Position position, int layer) : base(position, layer)
        {
        }

        public override void OnLeaveHex()
        {
        }

        public override TileBase GetTile()
        {
            return GameObject.FindObjectOfType<LevelManager>().GetHexType(Constants.UNDESTRACTABLE_HEX);
        }

        public override bool isDestoyeble()
        {
            return false;
        }

        public override void OnEnterHex(Position previousCoordinate)
        {
        }
    }
}
