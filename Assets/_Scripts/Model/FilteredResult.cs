using Assets._Scripts.Additional;
using Assets._Scripts.FakeHexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Model
{
    public class FilteredResult
    {
        public BaseFakeHexType[,] hexMap;
      //  public Dictionary<IAdditional[,], PathFindResult> pathResults;
        public IAdditional[,] fruits;
        // public List<PathFindDataWithDifficulty> pathes;
       
        public PathFindData pathFindData;

        public FilteredResult(BaseFakeHexType[,] hexMap, PathFindData pathFindData, IAdditional[,] fruits)
        {
            this.hexMap = hexMap;
            this.pathFindData = pathFindData;
            this.fruits = fruits;

        }
    }
}
