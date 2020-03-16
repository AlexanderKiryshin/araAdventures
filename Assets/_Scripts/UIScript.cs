using Assets._Scripts.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UIScript:MonoBehaviour
    {
        public GameObject pauseCanvas;
        public GameObject gameCanvas;
        public GameObject levelCompleteCanvas;

        public void Awake()
        {
            LevelManager.WinEvent = null;
            LevelManager.WinEvent += LevelComplete;
        }

        public void OpenPauseScreen()
        {
            pauseCanvas.SetActive(true);
            gameCanvas.SetActive(false);
        }
        public void ClosePauseScreen()
        {
            pauseCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        }

        public void OpenLevelSelectionScreen()
        {
            SceneManager.LoadScene("LevelSelection");
            SceneManager.UnloadSceneAsync("UI");
        }

        public void LevelComplete()
        {
            FindObjectOfType<LevelEventManager>().Completelevel();
            gameCanvas.SetActive(false);
            levelCompleteCanvas.SetActive(true);      
        }

        public void LoadNextLevel()
        {
            FindObjectOfType<LevelLoader>().LoadNextLevel();
        }
        public void RestartLevel()
        {
            FindObjectOfType<LevelEventManager>().RestartLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
