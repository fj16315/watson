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
        itemPage.SetActive(false);
        notePage = GameObject.Find("NotesPage");
        itemPage.SetActive(false);
        menuPage = GameObject.Find("MenuPage");

        //ChangePage((int)Page.MENU);
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
            //pages[(int)currentTab].SetActive(false);
            //tabsR[(int)currentTab].SetActive(false);
            //currentTab = (Page)target;
            //pages[target].SetActive(true);
            //tabsR[target].SetActive(true);
        }
    }
}
