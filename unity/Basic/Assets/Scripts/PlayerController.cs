using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Notebook;

public class PlayerController : MonoBehaviour {

    public List<GameObject> inventory;
    public GameObject[] propObjects = new GameObject[9];
    public bool[] ownedProps = new bool[9];
    public string list = "";
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

    }

    public void PickUp(GameObject obj)
    {
        inventory.Add(obj);
        obj.transform.Translate(0, -10, 0);
        Interactable t = obj.GetComponent<Interactable>();
        if (t.description != "")
        {
            controller.Pause(true);
            t.InspectObject();
            controller.inspect = true;
        }
        if (t.propEnum < 9)
        {
            ownedProps[t.propEnum] = true;
        }
    }

    public bool Possesses(GameObject obj)
    {
        return inventory.Contains(obj);
    }
}
