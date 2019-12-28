using Assets._Scripts.Additional;
using Assets._Scripts.FakeHexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Model
{
    public class CopyResult
    {
        public IAdditional[,] fruitMaps;
        public BaseFakeHexType[,] hexMap;
        public int[,] ispassedHexes;
        public FakeMoveHero hero;

       public CopyResult(IAdditional[,] fruitMaps, BaseFakeHexType[,] hexMap, int[,] ispassedHexes, FakeMoveHero hero)
        {
            this.fruitMaps = fruitMaps;
            this.hexMap = hexMap;
            this.ispassedHexes = ispassedHexes;
            this.hero = hero;
        }
    }
}
