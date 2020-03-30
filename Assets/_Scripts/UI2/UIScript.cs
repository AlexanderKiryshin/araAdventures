using System.Collections;
using Assets._Scripts;
using Assets._Scripts.Analytics;
using Assets._Scripts.Model;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UIScript:MonoBehaviour
    {
        public GameObject pauseCanvas;
        public GameObject gameCanvas;
        public GameObject hintAlreadyActivated;
        public GameObject pathNotFound;
        public GameObject levelCompleteCanvas;

        public void Awake()
        {
            LevelManager.WinEvent = null;
            LevelManager.WinEvent += LevelComplete;
            HelpManager.helpAlreadyActivated += ShowHelpActivatedMessage;
            LevelManager.instance.pathNotFoundAction += PathNotFoundMessage;
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

        public void GetHelp()
        {
           HelpManager.helpUse.Invoke();
        }

        public void ShowHelpActivatedMessage()
        {
            StartCoroutine(ShowActivatedMessageCoroutine(hintAlreadyActivated));
        }

        private bool lockCor = false;
        IEnumerator ShowActivatedMessageCoroutine(GameObject messageObject)
        {
            if (!lockCor)
            {
                lockCor = true;
            }
            else
            {
                yield break;
            }
            messageObject.SetActive(true);
            
            var canvasGroup = messageObject.GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
            for (int i = 0; i < 25; i++)
            {
                canvasGroup.alpha = i * 0.04f;
                yield return new WaitForSeconds(0.02f);
            }
            yield return new WaitForSeconds(2.5f);
            for (int i = 0; i < 100; i++)
            {
                canvasGroup.alpha =1- i * 0.01f;
                yield return new WaitForSeconds(0.02f);
            }
            messageObject.SetActive(false);
            lockCor = false;
        }

        public void PathNotFoundMessage()
        {
            StartCoroutine(ShowActivatedMessageCoroutine(pathNotFound));
        }
    }
}
