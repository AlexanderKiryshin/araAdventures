using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LevelLoader:MonoBehaviour
    {
        public List<string> Scenes;
        private int sceneCounter = 0;
        [HideInInspector]
        public string nameOfScene;

        public void Start()
        {
            DontDestroyOnLoad(this);
           /* LevelManager.WinEvent = null;
            LevelManager.WinEvent += LoadNextLevel;*/
        }

        public void AddLevel(string nameOfScene)
        {
            SceneManager.LoadScene(nameOfScene, LoadSceneMode.Additive);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(Scenes[0]);
            Debug.LogError("Scene "+Scenes[0]);
            //sceneCounter++;          
        }

        /*public IEnumerator LoadLevelCoroutine()
        {
            gameObject.transform.DOLocalRotate(
                new Vector3(gameObject.transform.rotation.x, gameObject.transform.rotation.y,
                    gameObject.transform.rotation.z + angle), 0.5f);
            yield return new WaitForSeconds(0.2f);
        }*/
    }
}
