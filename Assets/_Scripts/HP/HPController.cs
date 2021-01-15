using System;
using System.Collections.Generic;
using Assets._Scripts.HP;
using UnityEngine;
using UnityEngine.UIElements;


namespace Assets._Scripts
{
    public class HPController :HPControllerBase
    {
        [SerializeField]
        protected UnityEngine.UI.Button hp1Button;
        [SerializeField]
        protected UnityEngine.UI.Button unlimitedHp1Button;
        [SerializeField]
        protected UnityEngine.UI.Button skipLevelButton;
        [SerializeField]
        protected UnityEngine.UI.Button replayButton;

        private void GameIsAvailibleState()
        {
            hp1Button.gameObject.SetActive(false);
            replayButton.gameObject.SetActive(true);
            unlimitedHp1Button.gameObject.SetActive(false);
            skipLevelButton.gameObject.SetActive(true);
        }

        private void GameIsUnavailibleState()
        {
            hp1Button.gameObject.SetActive(true);
            replayButton.gameObject.SetActive(false);
            unlimitedHp1Button.gameObject.SetActive(true);
            skipLevelButton.gameObject.SetActive(false);
        }

        void Awake()
        {
            SetHearth();
        }

        public void SetHearth()
        {
            if (PlayerPrefs.HasKey("CountHearth"))
            {
                int countHearth = PlayerPrefs.GetInt("CountHearth");
                if (countHearth == 3)
                {
                    isCounter = false;
                }
                else
                {
                    RestoreHealth();
                    isCounter = true;
                }

                for (int i = 0; i < countHearth; i++)
                {
                    hearths[i].SetActive(true);
                }

                for (int i = countHearth; i < 3; i++)
                {
                    hearths[i].SetActive(false);
                }

                if (countHearth > 0)
                {
                    GameIsAvailibleState();
                }
                else
                {
                    GameIsUnavailibleState();
                }
            }
            else
            {
                GameIsAvailibleState();
                PlayerPrefs.SetInt("CountHearth", 3);
                PlayerPrefs.Save();
            }
        }

        public void AddHearth()
        {
            if (PlayerPrefs.HasKey("CountHearth"))
            {
                int hearthes = PlayerPrefs.GetInt("CountHearth");
                if (hearthes<3)
                {
                    PlayerPrefs.SetInt("CountHearth", hearthes + 1);
                }
            }
        }

        protected override void RestoreHealth()
        {
            int seconds = GetSeconds();
            while (seconds >= SECOND_TO_RESTORE)
            {
                SaveDate();
                int hearthes = CountHeartes();
                GameIsAvailibleState();
                if (hearthes < 3)
                {
                    PlayerPrefs.SetInt("CountHearth", hearthes + 1);
                    PlayerPrefs.Save();
                    seconds -= SECOND_TO_RESTORE;
                }
                else
                {
                    PlayerPrefs.DeleteKey("timeRestoreHealth");
                    isCounter = false;
                    break;
                }
                for (int i = 0; i < hearthes + 1; i++)
                {
                    hearths[i].SetActive(true);
                }
            }
        }

        public bool LoseHearth()
        {
            if (PlayerPrefs.HasKey("CountHearth"))
            {
                int countHearth = PlayerPrefs.GetInt("CountHearth");

                if (countHearth > 0)
                {
                    countHearth--;
                    PlayerPrefs.SetInt("CountHearth", countHearth);
                    PlayerPrefs.Save();
                    SaveDate();
                    if (countHearth == 0)
                    {
                        return false;
                    }
                }
                else
                {
                    GameIsUnavailibleState();
                    return false;
                }
            }
            else
            {
                PlayerPrefs.SetInt("CountHearth", 2);
            }

            return true;
        }
    }
}
