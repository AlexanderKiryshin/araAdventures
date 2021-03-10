using UnityEngine;

namespace Assets._Scripts.HP
{
    public class HPControllerInGame:HPControllerBase
    {
        void Awake()
        {
            WinLoseManager.instance.hpController = this;
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
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    hearths[i].SetActive(true);
                }
                PlayerPrefs.SetInt("CountHearth", 3);
                PlayerPrefs.Save();
            }
        }

        protected override void RestoreHealth()
        {
            int seconds = GetSeconds();
            while (seconds > SECOND_TO_RESTORE)
            {
                SaveDate();
                int hearthes = CountHeartes();
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
            if (Test.instance.isDisabled)
            {
                return true;
            }
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
