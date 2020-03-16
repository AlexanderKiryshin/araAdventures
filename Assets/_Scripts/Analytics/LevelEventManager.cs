using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

namespace Assets._Scripts.Analytics
{
   public class LevelEventManager:MonoBehaviour
    {
        public enum LevelPlayState { InProgress, Won, Lost,Restart, Skip, Quit }

        private Scene thisScene;
        private LevelPlayState state = LevelPlayState.InProgress;
        private int score = 0;
        private float secondsElapsed = 0;
        private int deaths = 0;

        void Awake()
        {
            thisScene = SceneManager.GetActiveScene();
            AnalyticsEvent.LevelStart(thisScene.name);
        }

        public void RestartLevel()
        {
            Dictionary<string, object> customParams = new Dictionary<string, object>();
            if (!PlayerPrefs.HasKey("complete_" + thisScene.name))
            {             
                customParams.Add("seconds_played", secondsElapsed);
                customParams.Add("first_pass", true);
                if (PlayerPrefs.HasKey("level_fails_" + thisScene.name))
                {
                    int fails = PlayerPrefs.GetInt("level_fails_" + thisScene.name);
                    PlayerPrefs.SetInt("level_fails_" + thisScene.name,fails++);
                    float seconds = PlayerPrefs.GetFloat("level_"+thisScene.name);
                    PlayerPrefs.SetFloat("level_" + thisScene.name,seconds+secondsElapsed);
                }
                else
                {
                    PlayerPrefs.SetInt("level_fails_" + thisScene.name, 1);
                    PlayerPrefs.SetFloat("level_" + thisScene.name,secondsElapsed);
                }
            }
            else
            {
                customParams.Add("seconds_played", secondsElapsed);
                customParams.Add("first_pass", false);                
            }
            Debug.LogError("LevelFail");
            state = LevelPlayState.Lost;
            AnalyticsEvent.LevelFail(thisScene.name,thisScene.buildIndex,customParams);
        }

        public void Completelevel()
        {
            if (PlayerPrefs.HasKey("complete_" + thisScene.name))
            {
                return;
            }
            state = LevelPlayState.Won;
            Dictionary<string, object> customParams = new Dictionary<string, object>();
            customParams.Add("seconds_played", secondsElapsed);
            if (PlayerPrefs.HasKey("level_fails_" + thisScene.name))
            {
                int fails = PlayerPrefs.GetInt("level_fails_" + thisScene.name);
                customParams.Add("deaths", fails);
            }
            else
            {
                customParams.Add("deaths", 0);
            }
            PlayerPrefs.SetInt("complete_" + thisScene.name,1);
            Debug.LogError("LevelComplete");
            float seconds = PlayerPrefs.GetFloat("level_" + thisScene.name);
            customParams.Add("totalTime",seconds=secondsElapsed);
            AnalyticsEvent.LevelComplete(thisScene.name,thisScene.buildIndex,customParams);
        }


        void Update()
        {
            secondsElapsed += Time.deltaTime;
        }

        void OnDestroy()
        {            
            switch (this.state)
            {
                case LevelPlayState.InProgress:
                    Dictionary<string, object> customParams = new Dictionary<string, object>();
                    customParams.Add("seconds_played", secondsElapsed);
                    if (PlayerPrefs.HasKey("level_fails_" + thisScene.name))
                    {
                        int fails = PlayerPrefs.GetInt("level_fails_" + thisScene.name);
                        customParams.Add("deaths", fails);
                    }
                    else
                    {
                        customParams.Add("deaths", 0);
                    }
                    Debug.LogError("LevelQUIT");
                    AnalyticsEvent.LevelQuit(thisScene.name,thisScene.buildIndex,customParams);
                    break;
            }
        }
    }
}
