using UnityEngine;
using UnityEditor;

namespace Assets._Scripts
{
    public class TilemapMarker : MonoBehaviour
    {
        public Vector2Int position;

        private void OnDrawGizmos()
        {
            int x1 =Mathf.Abs(position.x % 10);
            int x2 = Mathf.Abs(position.x / 10 % 10);
            int y1 = Mathf.Abs(position.y % 10);
            int y2 = Mathf.Abs(position.y / 10 % 10);
            Vector2 x1pos = transform.position;
            Vector2 x2pos = transform.position;
            Vector2 y1pos = transform.position;
            Vector2 y2pos = transform.position;
            x1pos.x -= 0.2f;
            x2pos.x -=0.4f;
            y1pos.x += 0.4f;
            y2pos.x += 0.2f;
            Debug.LogError(position.x+"  "+x1 + "  " + x2 /*+ "  " + y1 + "  " + y2*/);
            Gizmos.DrawIcon(x1pos, x1.ToString(), false);
            Gizmos.DrawIcon(x2pos, x2.ToString(), false);
            Gizmos.DrawIcon(y1pos, y1.ToString(), false);
            Gizmos.DrawIcon(y2pos, y2.ToString(), false);
            if (position.x % 10<0|| position.x / 10 % 10<0)
            {
                Vector2 x3pos = transform.position;
                x3pos.x -= 0.5f;
                Gizmos.DrawIcon(x3pos, "minus", false);
            }
            if (position.y % 10 < 0 || position.y / 10 % 10 < 0)
            {
                Vector2 y3pos = transform.position;
                y3pos.x += 0.1f;
                Gizmos.DrawIcon(y3pos, "minus", false);
            }
        }  
    }   
}