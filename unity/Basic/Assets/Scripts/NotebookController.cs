using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum Page : int { CHARACTER, ITEM, INVENTORY, NOTES, MENU };

public class NotebookController : MonoBehaviour {

    //GameObject tabsLeft;
    GameObject tabsRightChars;
    GameObject tabsEmpty;
    Page currentTab = Page.MENU;
    // Notebook pages
    GameObject charPage;
    GameObject itemPage;
    GameObject invtPage;
    GameObject notePage;
    GameObject menuPage;
    GameObject currentPage;
    GameObject currentTabs;

    // Character clues
    

    // Use this for initialization
    void Start () {
        //tabsLeft = GameObject.Find("LeftTabsPanel");
        tabsRightChars = GameObject.Find("CharacterTabsPanel");
        tabsRightChars.SetActive(false);
        tabsEmpty = GameObject.Find("EmptyPanel");
        // Notebook pages
        charPage = GameObject.Find("CharactersPage");
        charPage.SetActive(false);
        itemPage = GameObject.Find("ItemsPage");
        itemPage.SetActive(false);
        invtPage = GameObject.Find("InventoryPage");
        invtPage.SetActive(false);
        notePage = GameObject.Find("NotesPage");
        notePage.SetActive(false);
        menuPage = GameObject.Find("MenuPage");

        currentPage = menuPage;
        currentTabs = tabsEmpty;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Activate(bool active)
    {

    }

    public void ChangePage(int target)
    {
        if (target != (int)currentTab)
        {
            if (currentTab == Page.CHARACTER)
            {
                tabsRightChars.SetActive(false);
                tabsEmpty.SetActive(true);
            }
            currentPage.SetActive(false);
            switch (target)
            {
                case (int)Page.CHARACTER:
                    tabsEmpty.SetActive(false);
                    charPage.SetActive(true);
                    tabsRightChars.SetActive(true);
                    currentPage = charPage;
                    currentTabs = tabsRightChars;
                    break;
                case (int)Page.INVENTORY:
                    invtPage.SetActive(true);
                    currentPage = invtPage;
                    currentTabs = tabsEmpty;
                    break;
                case (int)Page.ITEM:
                    itemPage.SetActive(true);
                    currentPage = itemPage;
                    currentTabs = tabsEmpty;
                    break;
                case (int)Page.NOTES:
                    notePage.SetActive(true);
                    currentPage = notePage;
                    currentTabs = tabsEmpty;
                    break;
                case (int)Page.MENU:
                    menuPage.SetActive(true);
                    currentPage = menuPage;
                    currentTabs = tabsEmpty;
                    break;
            }
            currentTab = (Page)target;
        }
    }

}
