using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC;
using Doors;
using Containers;
using UnityStandardAssets.Characters.FirstPerson;

// (T)ype (o)f (M)essage
enum ToM : int {CHARACTER, INTERACTABLE, DOOR};

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
    public GameObject masterCanvas, speechCanvas;
    PlayerController player;
    public MasterControl controller;
    public GameState state;

    // Use this for initialization
    void Start()
    {
        player = Object.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Physics.Raycast(this.transform.position, this.transform.forward, out objectHit, raycastDistance))
        {
            entity = objectHit.collider;
            NPCController npc = entity.GetComponent<NPCController>();
            Door door = entity.GetComponent<Door>();
            Interactable obj = entity.GetComponent<Interactable>();

            if (npc != null)
            {
                type = (int)ToM.CHARACTER;
                display = true;

                // Interact with character
                if (Input.GetMouseButtonDown(0) && !controller.paused)
                {
                    SpeechDialogue(npc);
                }
            }       
            else if (obj != null)
            {
                type = (int)ToM.INTERACTABLE;
                Container container = entity.GetComponent<Container>();
                if (Input.GetMouseButtonDown(0) && !controller.paused)
                {
                    if (obj.CanPickUp())
                    {
                        state.PickUp(entity.gameObject);
                        player.PickUp(entity.gameObject);
                        display = false;
                    }
                    else if (container != null)
                    {
                        container.Activate();
                    }
                }
                display = true;
            }
            else if (door != null)
            {
                type = (int)ToM.DOOR;
                // Interact with door
                if (Input.GetMouseButtonDown(0) && !controller.paused)
                {
                    door.Activate();
                }
                display = true;
            }
            else
            {
                display = false;
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
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
            {
                state.ExitTutorial();
                CloseDialogue();
            }
        }
        if (display && !converse && !controller.paused)
        {
            string message = "";

            switch (type)
            {
                // NPC
                case (int)ToM.CHARACTER:
                    NPCController npc = entity.GetComponent<NPCController>();
                    if (npc != null)
                    {
                        message = "Talk to " + npc.charName;
                    }
                    break;
                // Interactable
                case (int)ToM.INTERACTABLE:
                    Interactable interactable = entity.GetComponent<Interactable>();
                    Container container = entity.GetComponent<Container>();
                    if (container != null)
                    {
                        if (!container.locked && !container.open)
                        {
                            message = "Open ";
                        }
                        else if (!container.locked && container.open)
                        {
                            message = "Close ";
                        }
                        else if (container.locked && container.Unlockable())
                        {
                            message = "Unlock ";
                        }
                        else
                        {
                            message = "Locked ";
                        }
                    }
                    if (interactable != null)
                    {
                        if (interactable.CanPickUp())
                        {
                            message = "Pick up ";
                        }
                        message += interactable.objName;
                    }
                    break;
                // DOOR
                case (int)ToM.DOOR:
                    Door door = entity.GetComponent<Door>();
                    if (door != null && door.locked)
                    {
                        message = "Locked " + door.doorName + " door";   
                    }
                    else if (door != null)
                    {
                        if (door.open)
                        {
                            message = "Close " + door.doorName + " door";
                        }
                        else
                        {
                            message = "Open " + door.doorName + " door";
                        }
                    }
                    break;
                default:
                    display = false;
                    
                    break;
            }
            GUI.Box(new Rect(Screen.width / 2 + 20, Screen.height / 2 - 25, 240, 60), message);

        }
    }

    public void SpeechDialogue(NPCController character)
    {
        converse = true;
        controller.Pause(true);
        speechCanvas.GetComponent<DialogueScreen>().ShowScreen(character);
    }

    public void CloseDialogue()
    {
        converse = false;
        controller.Pause(false);
        speechCanvas.GetComponent<DialogueScreen>().HideScreen();
    }
}