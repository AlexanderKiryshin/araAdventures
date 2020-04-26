using DevToDev;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Assets._Scripts.Analytics.LevelEventManager;

namespace Assets._Scripts.Devtodev_analytic
{
    public class LevelAnalytics:MonoBehaviour
    {
        private LevelPlayState state = LevelPlayState.InProgress;
        private bool levelCompleted ;
        private Scene thisScene;
        private List<Position> path;

        public void Awake()
        {
            StartLevel();
        }
        public void StartLevel()
        {
            levelCompleted = false;
            thisScene = SceneManager.GetActiveScene();
            if (PlayerPrefs.HasKey("complete_" + thisScene.name))
            {
                return;
            }
            ProgressionEventParams eventParams = new ProgressionEventParams();
           // eventParams.SetSource(thisScene.name);
            Debug.LogError("<color=yellow>[SessionAnalytic]</color>level_started " + thisScene.name);
            DevToDev.Analytics.StartProgressionEvent(thisScene.name, eventParams);          
        }

        public void Completelevel()
        {
            if (PlayerPrefs.HasKey("complete_" + thisScene.name))
            {
                return;
            }
            state = LevelPlayState.Won;
            var locationParams = new ProgressionEventParams();
            locationParams.SetDuration((int)secondsElapsed);
            Dictionary<string, int> spent = new Dictionary<string, int>();
            if (PlayerPrefs.HasKey("level_fails_" + thisScene.name))
            {
                int fails = PlayerPrefs.GetInt("level_fails_" + thisScene.name);
                spent["deaths"]= fails;
            }
            else
            {
                spent["deaths"] = 0;
            }
            PlayerPrefs.SetInt("complete_" + thisScene.name, 1);
            float seconds = PlayerPrefs.GetFloat("level_" + thisScene.name);
            spent["total_seconds"] = (int)(seconds + secondsElapsed);
            locationParams.SetSpent(spent);
            locationParams.SetSuccessfulCompletion(true);
            DevToDev.Analytics.EndProgressionEvent(thisScene.name, locationParams);
            DevToDev.Analytics.LevelUp(thisScene.buildIndex);
            Debug.LogError("<color=yellow>[PlayerAnalytic]</color>complete "+thisScene.name+",deaths="+ spent["deaths"]+",total_seconds="+ spent["total_seconds"]);
        }

        public void RestartLevel()
        {
            CustomEventParams customParams = new CustomEventParams();
            if (path != null && path.Count > 0)
            {
                string stringPath = String.Empty;
                foreach (var point in path)
                {
                    stringPath += "(" + point.x + "," + point.y + ")";
                }

                customParams.AddParam("path", stringPath);
            }

            customParams.AddParam("time",secondsElapsed);

            Debug.LogError("<color=yellow>[PlayerAnalytic]</color>restart");
            DevToDev.Analytics.CustomEvent("restart",customParams);

            if (PlayerPrefs.HasKey("level_fails_" + thisScene.name))
            {
                int fails = PlayerPrefs.GetInt("level_fails_" + thisScene.name);
                PlayerPrefs.SetInt("level_fails_" + thisScene.name, fails++);
                float seconds = PlayerPrefs.GetFloat("level_" + thisScene.name);
                PlayerPrefs.SetFloat("level_" + thisScene.name, seconds + secondsElapsed);
            }
            else
            {
                PlayerPrefs.SetInt("level_fails_" + thisScene.name, 1);
                PlayerPrefs.SetFloat("level_" + thisScene.name, secondsElapsed);
            }            
        }

        private float secondsElapsed = 0;

        void Update()
        {
            secondsElapsed += Time.deltaTime;
        }

        void OnDestroy()
        {
            switch (this.state)
            {
                case LevelPlayState.InProgress:
                    var locationParams = new ProgressionEventParams();
                    locationParams.SetDuration((int)secondsElapsed);

                    Dictionary<string, int> spent = new Dictionary<string, int>();
                    if (PlayerPrefs.HasKey("level_fails_" + thisScene.name))
                    {
                        int fails = PlayerPrefs.GetInt("level_fails_" + thisScene.name);
                        spent["deaths"] = fails;
                    }
                    else
                    {
                        spent["deaths"] = 0;
                    }

                    float seconds = PlayerPrefs.GetFloat("level_" + thisScene.name);
                    spent["total_seconds"] = (int)(seconds + secondsElapsed);
                    locationParams.SetSpent(spent);
                    locationParams.SetSuccessfulCompletion(false);
                    DevToDev.Analytics.EndProgressionEvent(thisScene.name, locationParams);
                    Debug.LogError("<color=yellow>[PlayerAnalytic]</color>failed " + thisScene.name + ",deaths=" + spent["deaths"] + ",total_seconds=" + spent["total_seconds"]);
                    break;
            }
        }
    }
}
