using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Assets._Scripts
{
#if (UNITY_EDITOR)
    [CustomEditor(typeof(LevelManager))]
    public class Button : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Open File"))
            {
               //action
            }
        }

    }
#endif
}
