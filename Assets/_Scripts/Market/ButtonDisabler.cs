using UnityEngine;
using UnityEditor;

namespace Assets._Scripts.Market
{
    public class ButtonDisabler : MonoBehaviour
    {
        private void Awake()
        {
            if (PlayerPrefs.HasKey("noads"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}