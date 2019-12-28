using UnityEngine;
using System.Collections;
using System;

public class LevelSelectionScreen : MonoBehaviour
{
    private const float RETURN_BUTTON_SIZE = 0.2f;
    private const float BACK_BUTTON_SIZE = 0.2f;
    private const float ARROW_INDENT = 0.15f;
    private float SHEET_MARK_SIZE = 0.04f;
    private float SHEET_MARK_INDENT = 0.03f;
    private float SHEET_MARK_INDENT_LEFT = 0.25f;
    private float SHEET_MARK_INDENT_RIGHT= 0.2f;
    private float SIZE = 80f;
    private float BORDER_INDENT_X = 15f;
    private const float INDENT_X = 15f;
    private const float INDENT_Y = 15f;
    private const float BORDER_INDENT_Y = 0.45f;
    private const float BORDER_INDENT_Y_DOWN = 0.30f;
    private const float BORDER_INDENT_Y_TEXT = 0.33f;
    private const float BORDER_INDENT_X_TEXT = 0.05f;
    public GameObject[] numbers;
    public GameObject rectangle;
    public GameObject activeMark;
    public GameObject unActiveMark;
    public GameObject nextButton;
    public GameObject backButton;
    public GameObject returnButton;
    public GameObject unAvailiblePage;
    private GameObject[] rectArray;
    private GameObject[] marksArray;
    public GameObject canvas;
    public Camera camera;
    float screenWidth;
    float screenHeight;
    int countTrianglesInRow = 0;
    int countTrianglesInColumn = 0;
    float offsetY = 0;
    float offsetX = 0;
    float scale = 1;
    int sheets = 0;
    int sheetMarks = 0;
    GUIStyle numberLevel;
    int activeSheet = 1;
    bool touchLock = false;
    Coroutine checkTouch;
    float x1=0;
    float y1=0;
    string x2="";
    string y2="";
    int click=0;
    float[] coordTouchButtonX;
    float[] coordTouchButtonY;
    float nextButtonTouchX;
    float nextButtonTouchY;
    float prevButtonTouchX;
    float prevButtonTouchY;
    float returnButtonTouchX;
    float returnButtonTouchY;
    float sizeInPixel;
    string hit = "";
    bool clicked = false;
    private void Awake()
    {
        checkTouch=StartCoroutine(CheckTouch());
        numberLevel = GUIGame.GetStyle(Color.white, 20);
        int currentLevels = 0;
        float aspectRatio = ClassAspectRatio.GetFloatAspectRatio();
        screenWidth = Screen.width;  // 4/aspectRatio;
        screenHeight = Screen.height;

        GetCountTriangle();
        sheets = Levels.COUNT_LEVELS / (countTrianglesInColumn * countTrianglesInRow)+1;
        sheetMarks = (int)((screenWidth - SHEET_MARK_INDENT_LEFT - SHEET_MARK_INDENT_RIGHT) / (SHEET_MARK_SIZE + SHEET_MARK_INDENT));
        if (sheetMarks>sheets)
        {
            sheetMarks = sheets;
            offsetX = (screenWidth - 2* SHEET_MARK_INDENT_LEFT-sheetMarks * (SHEET_MARK_SIZE + SHEET_MARK_INDENT)+ SHEET_MARK_INDENT) / 2;
        }
        marksArray = new GameObject[sheetMarks];
        for (int i=0;i<sheetMarks;i++)
        {
            Vector3 position = new Vector3(SHEET_MARK_INDENT_LEFT+SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2 
                - screenWidth / 2 + i * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
                -screenHeight/2+SHEET_MARK_SIZE/2+(BORDER_INDENT_Y_DOWN- SHEET_MARK_SIZE)/2, -2);
            if (activeSheet==i+1)
            {
                marksArray[i] = (GameObject)Instantiate(activeMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                marksArray[i].transform.SetParent(canvas.transform);
            }
            else
            {
                marksArray[i] = (GameObject)Instantiate(unActiveMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                marksArray[i].transform.SetParent(canvas.transform);
            }            
        }
        Vector3 pos = new Vector3(SHEET_MARK_INDENT_LEFT - screenWidth/2+offsetX+ARROW_INDENT+sheetMarks*(SHEET_MARK_INDENT+SHEET_MARK_SIZE)
            -SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2, -screenHeight / 2+ARROW_INDENT, -2f);
        nextButton=Instantiate(nextButton, pos, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
        nextButton.transform.SetParent(canvas.transform);
        pos = new Vector3(-screenWidth / 2 + offsetX + ARROW_INDENT, -screenHeight / 2 + ARROW_INDENT, -2f);
        backButton =Instantiate(backButton, pos, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
        backButton.transform.SetParent(canvas.transform);
        pos = new Vector3(-screenWidth / 2 + RETURN_BUTTON_SIZE, -screenHeight / 2 + ARROW_INDENT+30f, -2f);
        returnButton = Instantiate(returnButton, pos, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
        returnButton.transform.SetParent(canvas.transform);
        BORDER_INDENT_X =(screenWidth- (countTrianglesInRow * SIZE + (countTrianglesInRow - 1) * INDENT_X))/2;
        rectArray = new GameObject[countTrianglesInRow *countTrianglesInColumn];
        for (int j = 0; j < countTrianglesInColumn; j++)
            for (int i = 0; i < countTrianglesInRow; i++)           
            {
                if (currentLevels < Levels.COUNT_LEVELS)
                {
                    currentLevels++;
                    Vector3 position = new Vector3(BORDER_INDENT_X + SIZE / 2/* - screenWidth / 2*/ + i * (INDENT_X + SIZE), 
                        offsetY /*+ screenHeight / 2 */+ SIZE / 2 + j * (SIZE + INDENT_Y) + BORDER_INDENT_Y, -1);
                    rectArray[j * countTrianglesInRow + i] = (GameObject)Instantiate(rectangle, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                    rectArray[j * countTrianglesInRow + i].transform.SetParent(canvas.transform);
                    rectArray[j * countTrianglesInRow + i].transform.localScale = rectArray[j * countTrianglesInRow + i].transform.localScale * scale;
                }
                else
                { return; }
            }
       // CalculateTouchZones();

        // obj.name = number.ToString();
        // obj.transform.parent = gameObj.transform;
        //obj.layer = 9;
        //return obj;
    }
    void OnGUI()
    {
        float x = SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
        + sheetMarks * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) / 2 + offsetX-BORDER_INDENT_X_TEXT;
        float y = +screenHeight - SHEET_MARK_SIZE / 2 - BORDER_INDENT_Y_TEXT;
        GUI.Label(new Rect(x/(screenWidth/100)*Screen.width/100, y / (screenHeight / 100) * Screen.height/100, 100, 30), "1",numberLevel);
        GUI.Label(new Rect(20, 20, 100, 30), x1.ToString());
        GUI.Label(new Rect(20, 70, 100, 30), y1.ToString());
        GUI.Label(new Rect(20, 120, 100, 30),Screen.height+"x"+Screen.width);
        GUI.Label(new Rect(200, 20, 200, 50), x2.ToString());
        GUI.Label(new Rect(200, 70, 200, 50), y2.ToString());
      //  GUI.Label(new Rect(200, 170, 200, 50), hit.ToString());
        GUI.Label(new Rect(200, 20, 200, 50), GetTouchRect(rectArray[0], sizeInPixel).xMin.ToString());
        GUI.Label(new Rect(200, 70, 200, 50), GetTouchRect(rectArray[0], sizeInPixel).xMax.ToString());
        GUI.Label(new Rect(200, 120, 200, 50), GetTouchRect(rectArray[0], sizeInPixel).yMin.ToString());
        GUI.Label(new Rect(200, 170, 200, 50), GetTouchRect(rectArray[0], sizeInPixel).yMax.ToString());
        GUI.Label(new Rect(300, 20, 200, 50), GetTouchRect(rectArray[1], sizeInPixel).xMin.ToString());
        GUI.Label(new Rect(300, 70, 200, 50), GetTouchRect(rectArray[1], sizeInPixel).xMax.ToString());
        GUI.Label(new Rect(300, 120, 200, 50), GetTouchRect(rectArray[1], sizeInPixel).yMin.ToString());
        GUI.Label(new Rect(300, 170, 200, 50), GetTouchRect(rectArray[1], sizeInPixel).yMax.ToString());
        GUI.Label(new Rect(400, 20, 200, 50), GetTouchRect(rectArray[2], sizeInPixel).xMin.ToString());
        GUI.Label(new Rect(400, 70, 200, 50), GetTouchRect(rectArray[2], sizeInPixel).xMax.ToString());
        GUI.Label(new Rect(400, 120, 200, 50), GetTouchRect(rectArray[2], sizeInPixel).yMin.ToString());
        GUI.Label(new Rect(400, 170, 200, 50), GetTouchRect(rectArray[2], sizeInPixel).yMax.ToString());
        GUI.Label(new Rect(500, 170, 200, 50),hit);
        // GUI.Label(new Rect(20, 170, 100, 30), xs);
        // GUI.Label(new Rect(20, 220, 100, 30), ys);
    }

    private IEnumerator CheckTouch()
    {
        for (;;)
        { 
            if (touchLock)
            {
                yield return new WaitForSecondsRealtime(0.01f);
                touchLock = false;
            }
            else
            {
                CheckInput();
                yield return new WaitForSecondsRealtime(0.01f);
            }
            
        }
    }
    public void CheckInput()
    {
        int counter = 0;
        foreach (Touch touch in Input.touches)
        {
            if ((touch.phase == TouchPhase.Began) && (!touchLock))
            {
                float x = touch.position.x;
                float y = touch.position.y;
                x1 = touch.position.x;
                y1 = touch.position.y;
                Rect rect;
                for (int j = 0; j < countTrianglesInColumn; j++)
                    for (int i = 0; i < countTrianglesInRow; i++)
                    {
                        rect= GetTouchRect(rectArray[j * countTrianglesInRow + i],sizeInPixel);
                        if ((x>rect.xMin)&&(x<rect.xMax)&&(y > rect.yMin) && (y < rect.yMax))
                        {
                            hit = (j * countTrianglesInRow + i).ToString();
                            return;
                        }                      
                    }
                rect = GetTouchRect(returnButton, BACK_BUTTON_SIZE / screenHeight * Screen.height);
                if ((x > rect.xMin) && (x < rect.xMax) && (y > rect.yMin) && (y < rect.yMax))
                {
                    hit = "return";
                    return;
                }
                rect = GetTouchRect(nextButton, BACK_BUTTON_SIZE / screenHeight * Screen.height);
                if ((x > rect.xMin) && (x < rect.xMax) && (y > rect.yMin) && (y < rect.yMax))
                {
                    if (activeSheet < sheetMarks)
                    {

                        activeSheet++;
                        Destroy(marksArray[activeSheet - 1]);
                        Vector3 position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
                - screenWidth / 2 + (activeSheet - 1) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
                -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
                        marksArray[activeSheet - 1] = Instantiate(activeMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));

                        Destroy(marksArray[activeSheet - 2]);
                        position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
                - screenWidth / 2 + (activeSheet - 2) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
                -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
                        marksArray[activeSheet - 2] = Instantiate(unActiveMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                        int minCountLevelInPage = Levels.COUNT_LEVELS - (sheetMarks - 1) * countTrianglesInColumn * countTrianglesInRow;
                        if ((activeSheet >= sheetMarks) &&
                            (minCountLevelInPage < countTrianglesInColumn * countTrianglesInRow)
                            && (minCountLevelInPage != 0))
                        {
                            int count = Levels.COUNT_LEVELS - (sheetMarks - 1) * countTrianglesInColumn * countTrianglesInRow;
                            Vector3 pos;
                            for (int i = count; i < countTrianglesInColumn * countTrianglesInRow; i++)
                            {
                                pos = rectArray[i].transform.position;
                                Destroy(rectArray[i]);
                                rectArray[i] = Instantiate(unAvailiblePage, pos, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                                rectArray[i].transform.localScale = rectArray[i].transform.localScale * scale;

                            }
                        }
                    }
                    hit = "next";
                    return;
                }
                rect = GetTouchRect(backButton, BACK_BUTTON_SIZE / screenHeight * Screen.height);
                if ((x > rect.xMin) && (x < rect.xMax) && (y > rect.yMin) && (y < rect.yMax))
                {
                    if (activeSheet > 1)
                    {
                        activeSheet--;
                        Destroy(marksArray[activeSheet - 1]);
                        Vector3 position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
                - screenWidth / 2 + (activeSheet - 1) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
                -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
                        marksArray[activeSheet - 1] = Instantiate(activeMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));

                        Destroy(marksArray[activeSheet - 1 + 1]);
                        position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
                - screenWidth / 2 + (activeSheet) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
                -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
                        marksArray[activeSheet] = Instantiate(unActiveMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                    }

                    if ((activeSheet == sheetMarks - 1) && (Levels.COUNT_LEVELS - (sheetMarks - 1) *
                            countTrianglesInColumn * countTrianglesInRow < countTrianglesInColumn * countTrianglesInRow))
                    {
                        int count = Levels.COUNT_LEVELS - (sheetMarks - 1) * countTrianglesInColumn * countTrianglesInRow;
                        if (count != 0)
                        {
                            for (int i = count - 1; i < countTrianglesInColumn * countTrianglesInRow; i++)
                            {
                                Vector3 position = new Vector3(BORDER_INDENT_X + SIZE / 2 /*- screenWidth / 2*/ + (i) % (countTrianglesInRow) * (INDENT_X + SIZE),
                                            -offsetY /*+ screenHeight / 2*/ - SIZE / 2 - (float)Math.Ceiling((double)(i / countTrianglesInRow)) * (SIZE + INDENT_Y) - BORDER_INDENT_Y, -1);
                                // int index = (int)Math.Ceiling((double)((i % countTrianglesInRow + 1)) + i / countTrianglesInRow);
                                Destroy(rectArray[i]);
                                rectArray[i] = (GameObject)Instantiate(rectangle, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                                rectArray[i].transform.localScale = rectArray[i].transform.localScale * scale;
                                rectArray[i].transform.SetParent(canvas.transform);
                            }
                        }
                    }
                    hit = "back";
                }
                /*     for (int j = 0; j < countTrianglesInColumn; j++)
                 for (int i = 0; i < countTrianglesInRow; i++)
                 {
                     if ((x > coordTouchButtonX[j * countTrianglesInRow + i])
                         && (x < coordTouchButtonX[j * countTrianglesInRow + i] + sizeInPixel)
                         && (y < coordTouchButtonY[j * countTrianglesInRow + i])
                         && (y > coordTouchButtonY[j * countTrianglesInRow + i] + sizeInPixel))
                     {
                         hit = j * countTrianglesInRow + i;
                         break;
                     }
                     touchLock = true;
                     //    yield return new WaitForSeconds(0.01f);
                     break;
                 }*/
            }
            clicked = false;
            //  yield return new WaitForSeconds(0);
        }
    }
    public void Test2()
    {
        if (activeSheet > 1)
        {
            activeSheet--;
            Destroy(marksArray[activeSheet - 1]);
            Vector3 position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
    - screenWidth / 2 + (activeSheet - 1) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
    -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
            marksArray[activeSheet - 1] = Instantiate(activeMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));

            Destroy(marksArray[activeSheet - 1 + 1]);
            position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
    - screenWidth / 2 + (activeSheet) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
    -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
            marksArray[activeSheet] = Instantiate(unActiveMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
        }

        if ((activeSheet == sheetMarks - 1) && (Levels.COUNT_LEVELS - (sheetMarks - 1) *
                countTrianglesInColumn * countTrianglesInRow < countTrianglesInColumn * countTrianglesInRow))
        {
            int count = Levels.COUNT_LEVELS - (sheetMarks - 1) * countTrianglesInColumn * countTrianglesInRow;
            if (count != 0)
            {
                for (int i = count - 1; i < countTrianglesInColumn * countTrianglesInRow; i++)
                {
                    Vector3 position = new Vector3(BORDER_INDENT_X + SIZE / 2 /*- screenWidth / 2*/ + (i) %(countTrianglesInRow) * (INDENT_X + SIZE),
                                -offsetY /*+ screenHeight / 2*/ - SIZE / 2 - (float)Math.Ceiling((double)(i/ countTrianglesInRow)) * (SIZE + INDENT_Y) - BORDER_INDENT_Y, -1);
                   // int index = (int)Math.Ceiling((double)((i % countTrianglesInRow + 1)) + i / countTrianglesInRow);
                    Destroy(rectArray[i]);
                    rectArray[i]= (GameObject)Instantiate(rectangle, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                    rectArray[i].transform.localScale =rectArray[i].transform.localScale * scale;
                    rectArray[i].transform.SetParent(canvas.transform);
                }
            }
        }
    }
    public void Test()
    {
        if (activeSheet < sheetMarks)
        {

            activeSheet++;
            Destroy(marksArray[activeSheet - 1]);
            Vector3 position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
    - screenWidth / 2 + (activeSheet - 1) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
    -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
            marksArray[activeSheet - 1] = Instantiate(activeMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));

            Destroy(marksArray[activeSheet - 2]);
            position = new Vector3(SHEET_MARK_INDENT_LEFT + SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2
    - screenWidth / 2 + (activeSheet - 2) * (SHEET_MARK_INDENT + SHEET_MARK_SIZE) + offsetX,
    -screenHeight / 2 + SHEET_MARK_SIZE / 2 + (BORDER_INDENT_Y_DOWN - SHEET_MARK_SIZE) / 2, -1);
            marksArray[activeSheet - 2] = Instantiate(unActiveMark, position, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
            int minCountLevelInPage = Levels.COUNT_LEVELS - (sheetMarks - 1) * countTrianglesInColumn * countTrianglesInRow;
            if ((activeSheet >= sheetMarks) &&
                (minCountLevelInPage < countTrianglesInColumn * countTrianglesInRow)
                && (minCountLevelInPage != 0))
            {
                int count = Levels.COUNT_LEVELS - (sheetMarks - 1) * countTrianglesInColumn * countTrianglesInRow;
                Vector3 pos;
                for (int i = count; i < countTrianglesInColumn * countTrianglesInRow; i++)
                {
                    pos = rectArray[i].transform.position;
                    Destroy(rectArray[i]);
                    rectArray[i] = Instantiate(unAvailiblePage, pos, Quaternion.AngleAxis(0, new Vector3(0, 0, 1f)));
                    rectArray[i].transform.localScale = rectArray[i].transform.localScale * scale;

                }
            }
        }
    }
    void CalculateTouchZones()
    {
        coordTouchButtonX = new float[countTrianglesInRow * countTrianglesInColumn];
        coordTouchButtonY = new float[countTrianglesInRow * countTrianglesInColumn];
        for (int j = 0; j < countTrianglesInColumn; j++)
        {
            for (int i = 0; i < countTrianglesInRow; i++)
            {
                coordTouchButtonX[j * countTrianglesInRow + i] = (BORDER_INDENT_X - screenWidth / 2 + i * (INDENT_X + SIZE) + screenWidth / 2) / screenWidth * Screen.width;
                coordTouchButtonY[j * countTrianglesInRow + i]=(-offsetY + screenHeight / 2 - j * (SIZE + INDENT_Y) - BORDER_INDENT_Y+ screenHeight/2) /screenHeight*Screen.height;
            }
        }
        sizeInPixel = (SIZE / screenHeight) * Screen.height;
        new Vector3(SHEET_MARK_INDENT_LEFT - screenWidth / 2 + offsetX + ARROW_INDENT + sheetMarks * (SHEET_MARK_INDENT + SHEET_MARK_SIZE)
            - SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2, -screenHeight / 2 + ARROW_INDENT, 1f);

        //TODO
        nextButtonTouchX = (SHEET_MARK_INDENT_LEFT - screenWidth / 2 + offsetX + ARROW_INDENT + sheetMarks * (SHEET_MARK_INDENT + SHEET_MARK_SIZE)
            - SHEET_MARK_INDENT + SHEET_MARK_SIZE / 2) / screenWidth * Screen.width;
    }

    Rect GetTouchRect(GameObject obj,float sizeInPixel)
    {
        return new Rect((obj.transform.position.x+screenWidth/2)/screenWidth*Screen.width - sizeInPixel / 2, 
            (obj.transform.position.y + screenHeight / 2) / screenHeight * Screen.height - sizeInPixel / 2,sizeInPixel,sizeInPixel) ;
    }
   /*  void CheckLevelPress()
    {
        for (int j = 0; j < countTrianglesInColumn; j++)
            for (int i = 0; i < countTrianglesInRow; i++)
            {
                Vector3 position = new Vector3(BORDER_INDENT_X + SIZE / 2 - screenWidth / 2 + i * (INDENT_X + SIZE),
            -offsetY + screenHeight / 2 - SIZE / 2 - j * (SIZE + INDENT_Y) - BORDER_INDENT_Y, -1);
            }
        
    }*/
    void GetCountTriangle()
    {
        GetCountTriangleInRow();
        if(GetCountTriangleInColumn())
        {
            GetCountTriangle();
        }
    }
    bool GetCountTriangleInColumn()
    {
        bool sizeChanged = false;
        countTrianglesInColumn = 0;
        float remain = screenHeight - BORDER_INDENT_Y - BORDER_INDENT_Y_DOWN;
        do
        {
            remain -= INDENT_Y;
            if (remain > SIZE + INDENT_Y)
            {
                countTrianglesInColumn++;
                remain -= SIZE;
            }
        }
        while (remain > SIZE);
        if ((remain>0.8*SIZE)&&(countTrianglesInColumn<8))
        {
            SIZE -= 0.05f;
            sizeChanged = true;
            GetCountTriangleInColumn();
        }
        offsetY = remain;
        return sizeChanged;
       // scale = (float)SIZE / 0.66f;
    }
    bool GetCountTriangleInRow()
    {
        bool sizeChanged = false;
        countTrianglesInRow = 0;
        float remain = screenWidth-BORDER_INDENT_X;
        do
        {
            remain -= INDENT_X;
            if (remain > SIZE + INDENT_X)
            {
                countTrianglesInRow++;
                remain -= SIZE;
            }
        }
        while (remain > SIZE);
        if ((remain > 0.8 * SIZE)&& (countTrianglesInRow <5))
        {
            SIZE -= 0.05f;
            sizeChanged = true;
            GetCountTriangleInRow();
        }
        // scale = (float)SIZE / 0.66f;
        return sizeChanged;
    }
}