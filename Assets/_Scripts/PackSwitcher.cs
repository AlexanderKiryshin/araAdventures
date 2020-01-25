using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets._Scripts
{
    public class PackSwitcher:SerializedMonoBehaviour
    {
        public Sprite inactiveButton;
        public Sprite activeButton;
        public List<GameObject> packs;
        public List<GameObject> buttons;
        public int sizeInactiveButton;
        public int sizeActiveButton;
        public void EnablePack(int index)
        {
            foreach (var pack in packs)
            {
                pack.SetActive(false);
            }

            foreach (var button in buttons)
            {
                button.GetComponent<Image>().sprite = inactiveButton;
                button.GetComponent<RectTransform>().sizeDelta= new Vector2(sizeInactiveButton,sizeInactiveButton);
            }

            buttons[index].GetComponent<Image>().sprite = activeButton;
            packs[index].SetActive(true);
            buttons[index].GetComponent<RectTransform>().sizeDelta = new Vector2(sizeActiveButton, sizeActiveButton);
        }
        
    }
}
