using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.FakeHexes
{
#if (UNITY_EDITOR)
    public static class BaseOperationWithMap
    {
        public static void DestroyHex(Position position, int layout, IFakeHexType[,] map)
        {
            map[position.x,position.y]=new FakeEmptyHex();           
        }

        public static void ChangeHex(Position position, int layout, IFakeHexType[,] map, IFakeHexType hex)
        {
            map[position.x, position.y] = hex;
        }
    }
#endif
}
