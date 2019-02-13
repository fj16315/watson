using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Tab : int { CHARACTER, ITEM, INVENTORY, NOTES, MENU };

public class NotebookController : MonoBehaviour {

    GameObject tabsLeft;
    GameObject tabsRightChars;
    GameObject tabsEmpty;
    Tab currentTab = Tab.MENU;

    // Use this for initialization
    void Start () {
        tabsLeft = transform.Find("Container/LeftTabsPanel").gameObject;
        tabsRightChars = transform.Find("Container/CharacterTabsPanel").gameObject;
        tabsEmpty = transform.Find("Container/EmptyPanel").gameObject;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowScreen()
    {
    }

    public void HideScreen()
    {
    }

    public void ChangeLeftTab(int target)
    {
        currentTab = (Tab)target;
        switch (target)
        {
            case (int)Tab.CHARACTER:
                tabsEmpty.SetActive(false);
                tabsRightChars.SetActive(true);
                break;
        }
    }
}
