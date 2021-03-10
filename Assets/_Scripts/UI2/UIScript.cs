using System.Collections;
using Assets._Scripts;
using Assets._Scripts.Analytics;
using Assets._Scripts.HP;
//using Assets._Scripts.Devtodev_analytic;
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
        public GameObject notEnoughMoney;
        public GameObject levelCompleteCanvas;
        public GameObject loseCanvas;
        public HPControllerInGame hpController;
        public GameObject hintCanvas;
        public MoneyTaker moneyTaker;
        public MoneyVisualizer moneyVisualizer;
        public MoveHero moveHero;

        public void Awake()
        {
            WinLoseManager.instance.loseEvent += LoseLevel;
            LevelManager.WinEvent = null;
            LevelManager.WinEvent += LevelComplete;
            HelpManager.helpAlreadyActivated += ShowHelpActivatedMessage;
            HelpManager.notEnoughMoney += ShowNotEnoughMoneyMessage;
            LevelManager.instance.pathNotFoundAction += PathNotFoundMessage;
            moveHero = FindObjectOfType<MoveHero>();
        }

        public void OpenPauseScreen()
        {
            pauseCanvas.SetActive(true);
            gameCanvas.SetActive(false);
            moveHero.LockInput();
        }
        public void ClosePauseScreen()
        {
            pauseCanvas.SetActive(false);
            gameCanvas.SetActive(true);
            moveHero.UnlockInput();
        }

        public void OpenLevelSelectionScreen()
        {
            moveHero.LockInput();
            SceneManager.LoadScene("LevelSelection");
            SceneManager.UnloadSceneAsync("UI");
        }

        public void LevelComplete()
        {
           // FindObjectOfType<LevelAnalytics>().Completelevel();
            gameCanvas.SetActive(false);
            levelCompleteCanvas.SetActive(true);
            levelCompleteCanvas.GetComponentInChildren<Animator>().Play("win");
            moveHero.LockInput();
        }

        public void LoseLevel()
        {
            gameCanvas.SetActive(false);
            loseCanvas.SetActive(true);
            loseCanvas.GetComponentInChildren<Animator>().Play("defeat");
            moveHero.LockInput();
        }
        public void LoadNextLevel()
        {
            HelpManager.numberAvailibleTips = 0;
            Advertisments.instance.OnlevelEnd();
            FindObjectOfType<LevelLoader>().LoadNextLevel();
        }

        public void RestartLevelFree()
        {
            HelpManager.numberAvailibleTips = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void RestartLevel()
        {
            HelpManager.numberAvailibleTips = 0;
            if (hpController.LoseHearth())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                LoseLevel();
            }
           // FindObjectOfType<LevelAnalytics>().RestartLevel();
            
        }

        public void GetHelp()
        {
            hintCanvas.SetActive(true);
            moveHero.LockInput();
        }

        public void ShowHelp()
        {
            moveHero.UnlockInput();
            hintCanvas.SetActive(false);
            if (HelpManager.numberAvailibleTips > 0)
            {
                HelpManager.helpAlreadyActivated?.Invoke();
            }
            else
            {
                if (moneyTaker.TrySpendMoney(20))
                {
                    moneyVisualizer.UpdateMoney();
                    HelpManager.helpUse.Invoke();
                }
                else
                {
                    HelpManager.notEnoughMoney?.Invoke();
                }
            }
        }

        public void CloseHelp()
        {
            moveHero.UnlockInput();
            hintCanvas.SetActive(false);
        }
        public void ShowHelpActivatedMessage()
        {
            StartCoroutine(ShowActivatedMessageCoroutine(hintAlreadyActivated));
        }

        public void ShowNotEnoughMoneyMessage()
        {
            StartCoroutine(ShowActivatedMessageCoroutine(notEnoughMoney));
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

        private void OnDestroy()
        {
            HelpManager.helpAlreadyActivated -= ShowHelpActivatedMessage;
            HelpManager.notEnoughMoney -= ShowNotEnoughMoneyMessage;
            if (LevelManager.instance != null)
            {
                LevelManager.instance.pathNotFoundAction -= PathNotFoundMessage;
            }
        }
    }
}
