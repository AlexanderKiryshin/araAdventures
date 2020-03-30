using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._Scripts.FakeHexes
{
#if (UNITY_EDITOR)
    public class FakeCreatingAroundHex:BaseFakeHexType
    {
        public FakeCreatingAroundHex(Position position, int layer) : base(position, layer)
        {
        }

        public override TileBase GetTile()
        {
            return LevelGenerator.instance.GetHexType(Constants.CREATING_AROUND_HEX);
        }

        public override void OnEnterHex(Position previousCoordinate, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLeaveHex(Position nextHex, ref FakeMoveHero hero, ref BaseFakeHexType[,] map)
        {
            throw new System.NotImplementedException();
        }
    }
#endif
}
