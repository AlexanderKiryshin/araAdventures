using Assets._Scripts.Additional;
using Assets._Scripts.FakeHexes;
using Assets.Scripts;
using System.Collections.Generic;


namespace Assets._Scripts.Model
{
    public class ResultWithFruitMap
    {
        public List<IAdditional[,]> fruitMaps;
        public BaseFakeHexType[,] hexMap;

        public ResultWithFruitMap(BaseFakeHexType[,] hexMap, List<IAdditional[,]> fruitMaps)
        {
            this.fruitMaps = fruitMaps;
            this.hexMap = hexMap;
        }
    }
}
