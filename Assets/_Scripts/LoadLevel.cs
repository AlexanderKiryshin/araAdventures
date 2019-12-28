using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LoadLevel:MonoBehaviour
    {
        public int indexScene;
        public string nameScene;
        public bool loadByIndex=true;

        public void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            if (loadByIndex)
            {
                LevelLoad(indexScene);
            }
            else
            {
                LevelLoad(nameScene);
            }
        }
        public void LevelLoad(int numberScene)
        {
            SceneManager.LoadScene(numberScene);
          //  SceneManager.LoadScene(numberScene, LoadSceneMode.Additive);
            SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        }
        public void LevelLoad(string nameScene)
        {
            SceneManager.LoadScene(nameScene);
        }
    }
}
