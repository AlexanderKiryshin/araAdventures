using System.Collections.Generic;
using Assets._Scripts.Additional;
using Assets._Scripts.FakeHexes;
using Assets.Scripts;

namespace Assets._Scripts.Model
{
    public class PathVariant
    {
        public FakeMoveHero hero;
        public int fruitCount;
        public List<Position> path;
        public Dictionary<Position, HexWithPasses> hexMap;
        public Dictionary<Position, IAdditional> fruitMaps;

        public PathVariant(FakeMoveHero hero, int fruitCount, List<Position> path, Dictionary<Position, HexWithPasses> hexMap, Dictionary<Position, IAdditional> fruitMaps)
        {
            this.hero = hero;
            this.fruitCount = fruitCount;
            this.path = path;
            this.hexMap = hexMap;
            this.fruitMaps = fruitMaps;
        }
    }
}
