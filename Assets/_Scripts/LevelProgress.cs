using UnityEngine;
using System.Collections.Generic;
using Assets._Scripts.Model;
using UnityEngine.UI;
using TMPro;

namespace Assets._Scripts
{
    public class LevelProgress : MonoBehaviour
    {
        public Sprite unlockedButton;
        [SerializeField]
        public List<GameObject> levels;
        [SerializeField]
        private List<GameObject> packs;
        [SerializeField]
        private List<TextMeshProUGUI> texts;
        void Awake()
        {
            int numberPack;
            if (PlayerPrefs.HasKey("progress"))
            {
                var currentLevel = PlayerPrefs.GetInt("progress");
                numberPack = currentLevel / 15;
                for (int i = 0; i < numberPack; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        levels[i * 15 + j].transform.GetChild(0).gameObject.SetActive(true);
                        levels[i * 15 + j].GetComponent<Image>().sprite = unlockedButton;
                        levels[i * 15 + j].GetComponent<UnityEngine.UI.Button>().interactable = true;
                    }
                }
                int remainingLevels = currentLevel - numberPack * 15;
                if (numberPack < 2)
                {
                    for (int i = 0; i < remainingLevels + 1; i++)
                    {
                        levels[numberPack * 15 + i].transform.GetChild(0).gameObject.SetActive(true);
                        levels[numberPack * 15 + i].GetComponent<Image>().sprite = unlockedButton;
                        levels[numberPack * 15 + i].GetComponent<UnityEngine.UI.Button>().interactable = true;
                    }
                }
                if (numberPack > packs.Count)
                {
                    return;
                }
                for (int i = 1; i < numberPack+1; i++)
                {
                    packs[i].GetComponent<Image>().sprite = unlockedButton;
                    texts[i].enabled = true;
                    packs[i].GetComponent<UnityEngine.UI.Button>().interactable = true;
                }
            }
            else
            {
                Debug.LogError("noprogress");
                levels[0].transform.GetChild(0).gameObject.SetActive(true);
                levels[0].GetComponent<Image>().sprite = unlockedButton;
                levels[0].GetComponent<UnityEngine.UI.Button>().interactable = true;
            }

        }
    }
}