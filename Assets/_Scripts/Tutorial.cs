using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject pages;
    public GameObject nextButton;
    public GameObject backButton;
    private int currentPage;

    private int pageCount;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MoveHero>().LockInput();
        currentPage = 0;
        pageCount = pages.transform.childCount;
        for (int i = 1; i < pageCount; i++)
        {
            pages.transform.GetChild(i).gameObject.SetActive(false);
        }
        pages.transform.GetChild(0).gameObject.SetActive(true);
        backButton.GetComponent<Button>().interactable = false;
        if (pageCount < 2)
        {
            nextButton.GetComponent<Button>().interactable = false;
        }
    }

    public void NextPage()
    {
        if (currentPage < pageCount - 1)
        {
            backButton.GetComponent<Button>().interactable = true;
            pages.transform.GetChild(currentPage).gameObject.SetActive(false);
            currentPage++;
            pages.transform.GetChild(currentPage).gameObject.SetActive(true);
        }

        if (currentPage == pageCount - 1)
        {
            nextButton.GetComponent<Button>().interactable=false;
        }
    }

   public void BackPage()
   {
       if (currentPage > 0)
       {
           nextButton.GetComponent<Button>().interactable = true;
            pages.transform.GetChild(currentPage).gameObject.SetActive(false);
           currentPage--;
           pages.transform.GetChild(currentPage).gameObject.SetActive(true);
        }
       if (currentPage == 0)
       {
           backButton.GetComponent<Button>().interactable = false;
       }
    }

   public void Close()
   {
       gameObject.SetActive(false);
       FindObjectOfType<MoveHero>().UnlockInput();
    }
}
