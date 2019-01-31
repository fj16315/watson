using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Containers;

public class Box : Container {

    Transform lid;
    PlayerController player;

	// Use this for initialization
	void Start () {
        lid = transform.Find("lid");
        player = Object.FindObjectOfType<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Open()
    {
        if (Unlockable())
        {
            lid.gameObject.SetActive(false);
            open = true;
            locked = false;
        }
        
    }

    public override void Close()
    {
        lid.gameObject.SetActive(true);
        open = false;
    }

    public override bool Unlockable()
    {
        return player.Possesses(this.key.gameObject);
    }

    public override void Activate()
    {
        if (this.open)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

}
