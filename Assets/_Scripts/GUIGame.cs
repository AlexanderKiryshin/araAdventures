using UnityEngine;
public class GUIGame : MonoBehaviour
{
    public static int fontSizeTopBorder;
    public delegate void Draw();
    private static GUIStyle guiStyleTextOnScreen = new GUIStyle();
    private static GUIStyle guiStyleTextGameOver = new GUIStyle();
    private static GUIStyle guiStyleScoreGameOver = new GUIStyle();
    private static GUIStyle guiStyleLevelSelection = new GUIStyle();
    private static int topMargin;
    public static Draw draw;
    public static void SetState(Draw draw)
    {
        GUIGame.draw = draw;
    }
    public static void SetTopPanelText()
    {
        if (Screen.width<=320)
        {
            guiStyleTextOnScreen.fontSize = 14;
            topMargin = 6;
        }
        if ((Screen.width>320)&&(Screen.width<=480))
        {
            guiStyleTextOnScreen.fontSize = 20;
            topMargin = 11;
        }
        if ((Screen.width > 480) && (Screen.width <= 600))
        {
            guiStyleTextOnScreen.fontSize = 24;
            topMargin = 12;
        }
        if ((Screen.width > 600) && (Screen.width <= 800))
        {
            guiStyleTextOnScreen.fontSize = 29;
            topMargin = 18;
        }
        if ((Screen.width > 800) && (Screen.width <= 1000))
        {
            guiStyleTextOnScreen.fontSize = 32;
            topMargin = 20;
        }
        if (Screen.width > 1000)
        {
            guiStyleTextOnScreen.fontSize = 38;
            topMargin = 27;
        }
    }
    public static void DrawGUI()
    {
        if (draw!=null)
          draw();
    }
    public static void Game()
    {
       // GUI.Label(new Rect(10, topMargin, 100, 30), "Очки: " + GameData.score, GUIGame.GUIStyleTextOnScreen);
        GUI.Label(new Rect(Screen.width-120, topMargin, 100, 30),PlayerPrefs.GetInt("money").ToString(), GUIGame.GUIStyleTextOnScreen);
    }
    static GUIGame()
    {
        guiStyleTextOnScreen.normal.textColor = Color.yellow;
        guiStyleTextOnScreen.fontSize = 26;
        guiStyleScoreGameOver.normal.textColor = Color.yellow;
        guiStyleScoreGameOver.fontSize = 30;
        guiStyleScoreGameOver.normal.textColor = Color.red;
        guiStyleScoreGameOver.fontSize = 22;
       // guiStyleLevelSelection.normal.textColor = Color.white;
        guiStyleScoreGameOver.fontSize = 14;

    }

    public static GUIStyle GUIStyleTextOnScreen
    { get { return guiStyleTextOnScreen; } }
    public static GUIStyle GUIStyleTextGameOver
    { get { return guiStyleTextGameOver; } }
    public static GUIStyle GUIStyleScoreGameOver
    { get { return guiStyleScoreGameOver; } }
    public static GUIStyle GetStyle(Color color,int size )
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = color;
        style.fontSize = size;
        return style;
    }

}

