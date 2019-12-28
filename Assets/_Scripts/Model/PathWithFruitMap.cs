using Assets._Scripts.Additional;
using Assets._Scripts.FakeHexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Model
{
    public class PathWithFruitMap
    {
        public BaseFakeHexType[,] hexMap;
        public Dictionary<IAdditional[,], PathFindResult> pathResults;

        public PathWithFruitMap(BaseFakeHexType[,] hexMap, Dictionary<IAdditional[,], PathFindResult> pathResults)
        {
            this.hexMap = hexMap;
            this.pathResults = pathResults;
        }
    }
}
