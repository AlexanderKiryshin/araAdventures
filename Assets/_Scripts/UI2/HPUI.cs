using Assets._Scripts.HP;
using Assets.Scripts;
using TMPro;
using UnityEngine;

namespace Assets._Scripts.UI2
{
    public class HPUI:Singleton<HPUI>
    {
        [SerializeField]
        private GameObject hpWindow;
        [SerializeField]
        private HPController hpController;
        [SerializeField]
        private TextMeshProUGUI text;
        public void OpenWindow()
        {
            hpWindow.SetActive(true);
            hpController.SetHearth();
        }

        public void CloseWindow()
        {
            hpWindow.SetActive(false);
        }

        void Update()
        {
            if (HPManager.instance.hpBarOnLevelSelect.CountHeartes() == 3)
            {
                hpWindow.SetActive(false);
            }
            else
            {
                text.text = "+1 сердце через " + HPManager.instance.hpBarOnLevelSelect.GetTime();
            }
            
        }
    }
}
