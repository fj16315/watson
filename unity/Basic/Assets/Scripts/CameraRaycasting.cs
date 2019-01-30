using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC;
using Things;
using Doors;

// (T)ype (o)f (M)essage
enum ToM : int {CHARACTER, THING, DOOR};

public class CameraRaycasting : MonoBehaviour
{

    public float raycastDistance;
    RaycastHit objectHit;
    bool display = false;
    bool converse = false;
    int type;
    Collider entity;
    public GUISkin skin;
    public string stringToEdit = "Who are you?";
    GameObject masterCanvas, speechCanvas;

    // Use this for initialization
    void Start()
    {
        masterCanvas = GameObject.Find("MasterCanvas");
        speechCanvas = GameObject.Find("SpeechCanvas");
        //speechCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(this.transform.position, this.transform.forward * raycastDistance, Color.yellow);

        if (Physics.Raycast(this.transform.position, this.transform.forward, out objectHit, raycastDistance))
        {
            entity = objectHit.collider;

            NPCController npc = entity.GetComponent<NPCController>();
            if (npc != null)
            {
                type = (int)ToM.CHARACTER;
                display = true;

                // Interact with character
                if (Input.GetMouseButtonDown(0))
                {
                    SpeechDialogue();
                }
            }

            Thing obj = entity.GetComponent<Thing>();
            if (obj != null)
            {
                type = (int)ToM.THING;
                display = true;
            }

            Door door = entity.GetComponent<Door>();
            if (door != null)
            {
                type = (int)ToM.DOOR;
                // Interact with door
                if (Input.GetMouseButtonDown(0))
                {
                    door.Activate();
                }
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
        GUI.skin = skin;
        // When conversing
        if (converse)
        {
            // Exit speech
            if (Event.current.isKey && Event.current.keyCode == KeyCode.LeftControl && GUI.GetNameOfFocusedControl() == "TextBox")
            {
                CloseDialogue();
            }
        }
        if (display && !converse)
        {
            string message = "";

            switch (type)
            {
                // NPC
                case (int)ToM.CHARACTER:
                    NPCController npc = entity.GetComponent<NPCController>();
                    message = "Talk to " + npc.charName;
                    break;
                // THING
                case (int)ToM.THING:
                    Thing thing = entity.GetComponent<Thing>();
                    if (thing.CanPickUp())
                    {
                        message = "Pick up ";
                    }
                    message += thing.objName;
                    break;
                // DOOR
                case (int)ToM.DOOR:
                    Door door = entity.GetComponent<Door>();
                    if (door.open)
                    {
                        message = "Close door";
                    }
                    else
                    {
                        message = "Open door";
                    }
                    break;
                // DEFAULT
                default:
                    display = false;
                    break;
            }
            GUI.Box(new Rect(Screen.width / 2 + 20, Screen.height / 2 - 25, 240, 60), message);

        }
    }

    private void Pause(bool pause)
    {
        switch (pause)
        {
            case true:
                Time.timeScale = 0;
                break;
            default:
                Time.timeScale = 1;
                break;
        }
    }

    private void SpeechDialogue()
    {
        masterCanvas.SetActive(false);
        converse = true;
        Pause(true);
        speechCanvas.GetComponent<DialogueScreen>().ShowScreen();
        //speechCanvas.SetActive(true);
    }

    private void CloseDialogue()
    {
        masterCanvas.SetActive(true);
        converse = false;
        Pause(false);
        speechCanvas.GetComponent<DialogueScreen>().HideScreen();
        //GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        //speechCanvas.SetActive(false);
    }
}