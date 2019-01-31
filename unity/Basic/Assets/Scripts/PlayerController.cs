using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Things;

public class PlayerController : MonoBehaviour {

    public List<GameObject> inventory;
    string list = "";
    public GUISkin skin;
    public GUISkin winskin;
    bool won = true;

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
            GUI.Box(new Rect(10, 10, 400, 200), "Inventory:\n\n" + list);
        }
    }

    public void PickUp(GameObject obj)
    {
        inventory.Add(obj);
        obj.SetActive(false);
        GenerateList();
        if (obj.GetComponent<Thing>().objName.Equals("Prize!"))
        {
            won = true;
        }
    }

    private void GenerateList()
    {
        list = "";
        for (int i = 0; i < inventory.Count; i++)
        {
            list += inventory[i].GetComponent<Thing>().objName + '\n';
        }
    }

    public bool Possesses(GameObject obj)
    {
        return inventory.Contains(obj);
    }
}
