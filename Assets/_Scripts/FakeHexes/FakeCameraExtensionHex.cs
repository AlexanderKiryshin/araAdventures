using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
   public  class FakeCameraExtensionHex : BaseFakeHexType
    {
            public FakeCameraExtensionHex(Position position, int layer) : base(position, layer)
            {
            }

            public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
            {
            }

            public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
            {
            }
#if (UNITY_EDITOR)
            public override TileBase GetTile()
            {
                return LevelGenerator.instance.GetHexType(Constants.UNDESTRACTABLE_HEX);
            }
#endif
        public override HexEnum GetHexEnum()
        {
            return HexEnum.CameraExtensionHex;
        }

        public override bool IsPassable()
        {
            return false;
        }

        public override bool IsService()
        {
            return true;
        }
    }

    }

