using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notebook;

public class PlayerController : MonoBehaviour {

    public List<GameObject> inventory;
    public string list = "";
    public GUISkin skin;
    public GUISkin winskin;
    bool won = false;
    public MasterControl controller;
    public NotebookController notebook;

	// Use this for initialization
	void Start () {
        inventory = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (won)
        {
            GUI.skin = winskin;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "You found my son!");
        }
        else
        {
            GUI.skin = skin;
            //GUI.Box(new Rect(10, 10, 400, 200), "Inventory:\n\n" + list);
        }
    }

    public void PickUp(GameObject obj)
    {
        inventory.Add(obj);
        //obj.SetActive(false);
        obj.transform.Translate(0, -10, 0);
        Interactable t = obj.GetComponent<Interactable>();
        if (t.description != "")
        {
            controller.Pause(true);
            t.InspectObject();
        }
        if (t.propEnum < 9)
        {
            notebook.ownedProps[t.propEnum] = true;
        }
        GenerateList();
    }

    private void GenerateList()
    {
        list = "";
        for (int i = 0; i < inventory.Count; i++)
        {
            string itemName = inventory[i].GetComponent<Interactable>().objName;
            if (itemName != "Notebook")
            {
                list += "- " + itemName +'\n';
            }
            
        }
    }

    public bool Possesses(GameObject obj)
    {
        return inventory.Contains(obj);
    }
}
