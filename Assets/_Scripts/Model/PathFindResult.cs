using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Model
{
    public class PathFindResult
    {
        public int[,] ispassedHexes;
        public List<PathFindData> results;
    }
}
