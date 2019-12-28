using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public GameObject waves;
        public Tilemap tilemap;
        public const float UNITS_SIZE = 1.2f;
        public const float WAVES_SIZE = 16f;
        public void SetCamera(Vector2 min,Vector2 max)
        {
            Camera camera = GetComponent<Camera>();
            Vector3 vector = Vector3.Lerp(min,max, 0.5f);
            camera.transform.position = new Vector3(vector.x, vector.y /*- _x / 2 + 1*/, -5);
            float _y = max.y + UNITS_SIZE / 2 - (min.y - UNITS_SIZE / 2);
            float _x = max.x + UNITS_SIZE/2 - (min.x -UNITS_SIZE/2);
           
            if (_x >= _y)
            {
                float coef = (float)Screen.height / (float)Screen.width;
                camera.orthographicSize = (_x+UNITS_SIZE) * coef / 2;
               
            }
            else
            {
                Debug.LogError("_y" + _y + "_" + (_y * UNITS_SIZE + UNITS_SIZE) / 2);
                camera.orthographicSize = (_y + UNITS_SIZE)/ 2;
            }
           
            waves.transform.localScale = new Vector3(camera.orthographicSize*2/WAVES_SIZE, waves.transform.localScale.y, camera.orthographicSize * 2 / WAVES_SIZE);
            waves.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y,
                waves.transform.position.z);

           
        }
     
        public void Awake()
        {
            return;
            Camera camera = GetComponent<Camera>();

            // camera.transform.position = tilemap.localBounds.center;
            Vector3 center = Vector3.Lerp(tilemap.localBounds.max, tilemap.localBounds.min, 0.5f);
            camera.transform.localPosition = center;

            Debug.LogError(tilemap.localBounds.min);
            Debug.LogError(tilemap.localBounds.max);
            Debug.LogError(center);
            Debug.LogError("center " + tilemap.localBounds.center);
            Debug.LogError("center " + tilemap.cellBounds.xMin);
            Debug.LogError("center " + tilemap.cellBounds.xMax);
            Debug.LogError("center " + tilemap.cellBounds.yMin);
            Debug.LogError("center " + tilemap.size.x + "  " + tilemap.size.y);
            var tiles = tilemap.GetTilesBlock(tilemap.cellBounds);
            int minX = 999999;
            int maxX = -999999;
            int minY = 999999;
            int maxY = -999999;
            for (int x = tilemap.cellBounds.xMin; x < tilemap.cellBounds.xMax; x++)
            {
                for (int y = tilemap.cellBounds.yMin; y < tilemap.cellBounds.yMax; y++)
                {
                    if (tilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        if (minX > x)
                        {
                            minX = x;
                        }

                        if (maxX < x)
                        {
                            maxX = x;
                        }

                        if (minY > y)
                        {
                            minY = y;
                        }

                        if (maxY < y)
                        {
                            maxY = y;
                        }
                    }
                }
            }

            var worldMinVector = tilemap.CellToLocal(new Vector3Int(minY, minX, 0));
            var worldMaxVector = tilemap.CellToLocal(new Vector3Int(maxY, maxX, 0));
            Vector3 vector = Vector3.Lerp(worldMinVector, worldMaxVector, 0.5f);
            Debug.LogError(Vector3.Lerp(new Vector3(minX, minY, 0), new Vector3(maxX, maxY, 0), 0.5f));

            float _y = maxX - minX+1;
            float _x = maxY - minY+1;
           
            if (_x >= _y)
            {
                float coef = (float)Screen.height / (float)Screen.width;
                camera.orthographicSize = (_x * UNITS_SIZE+UNITS_SIZE) * coef / 2;
                
            }
            else
            {
                Debug.LogError("_y" + _y +"_"+ (_y * UNITS_SIZE + UNITS_SIZE) / 2);
                camera.orthographicSize = (_y * UNITS_SIZE + UNITS_SIZE) / 2;
            }
            camera.transform.localPosition = new Vector3(vector.y, vector.x /*- _x / 2 + 1*/,-5);
            /* Debug.LogError(_x);
             if (_y == 0)
             {
                 _y = 1;
             }
             Debug.LogError(_y * 2f * Camera.main.pixelHeight / Camera.main.pixelWidth * .5f);*/
           // camera.orthographicSize = _y * 2 * Camera.main.pixelHeight / Camera.main.pixelWidth * .5f;
            
            /*camera.transform.position = new Vector3(vector.x, vector.y - _x / 2 + 1,
                camera.transform.position.z);
            float z = Mathf.Abs(camera.transform.localPosition.z);
            waves.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y + 10,
                waves.transform.position.z);
            if (z / 4 > 1.8f)
            {
                waves.transform.localScale = new Vector3(z / 4, waves.transform.localScale.y, z / 4);
            }
            else
            {
                waves.transform.localScale = new Vector3(1.8f, waves.transform.localScale.y, 1.8f);
            }
            */
            return;
            /* Debug.LogError(_x);
             float angle = camera.fieldOfView / 2 + camera.transform.rotation.x*360/Mathf.PI;
             float b = camera.transform.position.z - tilemap.transform.position.z;
 
           //  camera.fieldOfView = 2.0f * Mathf.Atan(b * 0.5f / _x*tilemap.cellSize.x) * Mathf.Rad2Deg;
             //  a = a * 180 / Mathf.PI;
             float a = b * Mathf.Tan(angle*Mathf.PI/180);
         //    aa = aa * 180 / Mathf.PI;
         if (_x > _y)
         {
             float coef = ((float)Screen.width) / ((float)Screen.height);
                 camera.transform.position = new Vector3( vector.x, vector.y- _x/2+1 , (-_x*1.6f-2) * coef);
         }
         else
         {
          
             camera.transform.position = new Vector3(vector.x ,vector.y - _y /2, -_y*0.9f );
             }
 
         float z = Mathf.Abs(camera.transform.localPosition.z);
         waves.transform.position=new Vector3(camera.transform.position.x,camera.transform.position.y+10, waves.transform.position.z);
         if (z/4 > 1.8f)
         {
             waves.transform.localScale = new Vector3(z / 4, waves.transform.localScale.y, z / 4);
         }
         else
         {
             waves.transform.localScale = new Vector3(1.8f, waves.transform.localScale.y, 1.8f);
             }
         }*/
        }
    }
}
