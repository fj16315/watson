using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC;

public class CameraRaycasting : MonoBehaviour
{

    public float raycastDistance;
    RaycastHit objectHit;
    bool display = false;
    string charName;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.forward * raycastDistance, Color.yellow);

        if (Physics.Raycast(this.transform.position, this.transform.forward, out objectHit, raycastDistance))
        {
            NPCController npc = objectHit.collider.GetComponent<NPCController>();
            if (npc != null)
            {
                //Debug.Log("true");
                charName = npc.charName;
                display = true;
            }
        }
        else
        {
            display = false;
        }

    }

    void OnGUI()
    {
        if (display == true)
        {
            //Debug.Log("textbox activated");
            string message = "Press T to talk to " + charName;
            GUI.Box(new Rect((Screen.width - 150) / 2, (Screen.height - 50) / 2, 150, 50), message);

        }
    }
}