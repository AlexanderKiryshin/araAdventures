using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts
{
    public class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
    {

        private static T _instance;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }
                return _instance;
            }
        }

    }
}
