using System.Collections;
using UnityEngine;
public class Levels : MonoBehaviour
{
    public const int COUNT_LEVELS=94;
	public static ArrayList GetLevelSectors(int currentLevel)
    {
        switch (currentLevel)
        {
            case 1: return LevelSectors1();
        }
        return null;
    }
    public static ArrayList LevelSectors1()
    {
        ArrayList sectors = new ArrayList();
        /*sectors.Add(new Sector(null, 0, 0, 35, TypeSector.red));
        sectors.Add(new Sector(null, 0, 35, 20, TypeSector.white));
        sectors.Add(new Sector(null, 0, 55, 35, TypeSector.red));
    }*/
        return sectors;
    }
}
