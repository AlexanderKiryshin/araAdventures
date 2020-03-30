using Assets.Scripts;

namespace Assets._Scripts.FakeHexes
{
    public static class BaseOperationWithMap
    {
        public static void DestroyHex(Position position, int layout, IFakeHexType[,] map)
        {
            map[position.x + HelpManager.instance.Offset.x, position.y + HelpManager.instance.Offset.y] =new FakeEmptyHex();           
        }

        public static void ChangeHex(Position position, int layout, IFakeHexType[,] map, IFakeHexType hex)
        {
            map[position.x + HelpManager.instance.Offset.x, position.y + HelpManager.instance.Offset.y] = hex;
        }
    }
}
