using UnityEngine;
using System.Collections;
public enum AspectRatio
{
    ar16_9,
    ar16_10,
    ar4_3,
    ar5_4,
    ar5_3,
    other
}
public enum Density
{
    TVDPI,
    LDPI,
    MDPI,
    HDPI,
    XHDPI,
    other
}
public static class ClassAspectRatio {
    private const int ASPECT_RATIO_COUNT = 6;
    public static float GetFloatAspectRatio()
    {
        return (float)Screen.height / (float)Screen.width;
    }
    public static AspectRatio GetAspectRatio()
    {
        int width = Screen.width;
        int height = Screen.height;
        if (CheckAspectRatio16_9(height, width))
        {
            return AspectRatio.ar16_9;
        }
        if (CheckAspectRatio16_10(height, width))
        {
            return AspectRatio.ar16_10;
        }
        if (CheckAspectRatio4_3(height, width))
        {
            return AspectRatio.ar4_3;
        }
        if (CheckAspectRatio5_4(height, width))
        {
            return AspectRatio.ar5_4;
        }
        if (CheckAspectRatio5_3(height, width))
        {
            return AspectRatio.ar5_3;
        }
        return AspectRatio.other;
    }
    public static bool CheckAspectRatio16_9(int height, int width)
    {
        return (height / 16 == width / 9);
    }
    public static bool CheckAspectRatio16_10(int height, int width)
    {
        return (height / 16 == width / 10);
    }
    public static bool CheckAspectRatio4_3(int height, int width)
    {
        return (height / 4 == width / 3);
    }
    public static bool CheckAspectRatio5_4(int height, int width)
    {
        return (height / 5 == width / 4);
    }
    public static bool CheckAspectRatio5_3(int height, int width)
    {       
        return (height / 5 == width / 3);
    }
    public static Density GetDensityMode()
    {
        switch ((int)Screen.dpi)
        {
            case 96:return Density.TVDPI;
            case 120: return Density.LDPI; 
            case 160: return Density.MDPI;
            case 240: return Density.HDPI;
            case 320: return Density.XHDPI;
        }       
        return Density.other;
    }
}
