using Assets._Scripts;
using Assets._Scripts.HP;
using Assets._Scripts.UI2;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UI.Button;

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
            if (nameScene != "LevelSelection")
            {
                if (HPManager.instance.hpBarOnLevelSelect.CountHeartes() <= 0)
                {
                    HPUI.instance.OpenWindow();
                    return;
                }
            }

           /* if (HPManager.instance.hpBarOnLevelSelect.CountHeartes() > 0)
            {*/
                if (loadByIndex)
                {
                    LevelLoad(indexScene+1);
                }
                else
                {
                    LevelLoad(nameScene);
                }
          /*  }
            else
            {
                HPUI.instance.OpenWindow();
            }*/
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
