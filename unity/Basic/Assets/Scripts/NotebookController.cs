using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Notebook
{
    enum Page : int { CHARACTER, ITEM, INVENTORY, NOTES, MENU };
    enum Character : int { ACTRESS, BUTLER, COLONEL, COUNTESS, EARL, GANGSTER, POLICE };
    enum Item : int { KEY, BOOK, POISON };

    public class NotebookController : MonoBehaviour
    {

        //GameObject tabsLeft;
        public GameObject tabsRightChars;
        public GameObject tabsEmpty;
        Page currentTab = Page.MENU;
        // Notebook pages
        public GameObject charPage;
        public GameObject itemPage;
        public GameObject invtPage;
        public GameObject notePage;
        public GameObject menuPage;
        GameObject currentPage;
        GameObject currentTabs;

        // Character clues
        List<string> cluesActress = new List<string>();
        List<string> cluesButler = new List<string>();
        List<string> cluesColonel = new List<string>();
        List<string> cluesCountess = new List<string>();
        List<string> cluesEarl = new List<string>();
        List<string> cluesGangster = new List<string>();
        List<string> cluesPolice = new List<string>();
        List<List<string>> cluesDirectory = new List<List<string>>();

        // Use this for initialization
        void Start()
        {
            tabsRightChars.SetActive(false);
            // Notebook pages
            charPage.SetActive(false);
            itemPage.SetActive(false);
            invtPage.SetActive(false);
            notePage.SetActive(false);

            currentPage = menuPage;
            currentTabs = tabsEmpty;

            // Add clue lists
            cluesDirectory.Add(cluesActress);
            cluesDirectory.Add(cluesButler);
            cluesDirectory.Add(cluesColonel);
            cluesDirectory.Add(cluesCountess);
            cluesDirectory.Add(cluesEarl);
            cluesDirectory.Add(cluesGangster);
            cluesDirectory.Add(cluesPolice);
        }

        // Update is called once per frame
        void Update()
        {

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

        public void AddClue(List<int> characters, List<int> items, string clue)
        {
            foreach (int character in characters)
            {
                cluesDirectory[character].Add(clue);
            }
        }

    }
}
