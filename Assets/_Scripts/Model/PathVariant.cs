using System.Collections.Generic;
using Assets._Scripts.Additional;
using Assets._Scripts.FakeHexes;
using Assets.Scripts;

namespace Assets._Scripts.Model
{
    public class PathVariant
    {
        private FakeMoveHero hero;
        private int fruitCount;
        private List<Position> path;
        private Dictionary<Position, BaseFakeHexType> hexMap;
        private Dictionary<Position, IAdditional> fruitMaps;

        public PathVariant(FakeMoveHero hero, int fruitCount, List<Position> path, Dictionary<Position, BaseFakeHexType> hexMap, Dictionary<Position, IAdditional> fruitMaps)
        {
            this.hero = hero;
            this.fruitCount = fruitCount;
            this.path = path;
            this.hexMap = hexMap;
            this.fruitMaps = fruitMaps;
        }
    }
}
