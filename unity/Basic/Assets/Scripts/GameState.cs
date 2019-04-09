using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doors;
using NPC;

public class GameState : MonoBehaviour {

    public enum State : int { TUTORIAL, STORY, PLAY, END };
    public State currentState = State.TUTORIAL;

    public List<string> tutorialStrings;
    public List<string> storyStrings;
    private int currentString = 0;
    private bool started = false;
    public bool alexa = false;

    // Useful game things
    public List<Door> entryDoors;
    public NPCController police;
    public DialogueScreen dialogue;
    public CameraRaycasting raycasting;
    public Interactable notebookInteractable;
    public MasterControl controller;

    // Tutorial variables
    bool saved = false;
    bool exited = false;
    bool pickup = false;
    bool notebook = false;

    // Solution variables
    public Toggle who;
    public Toggle why;
    public Toggle how;

    public bool who_check = false;
    public bool why_check = false;
    public bool how_check = false;
    public List<Toggle> checkboxes;

    private int score = 0;
    private int incorrect_check = 0;

    // Use this for initialization
    void Start () {
        currentState = State.TUTORIAL;
        currentString = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!started)
        {
            raycasting.SpeechDialogue(police);
            started = true;
        }
	}

    public string NextString()
    {
        Debug.Log("currentString = " + currentString);
        if( currentState == State.TUTORIAL )
        {
            if (currentString < tutorialStrings.Capacity)
            {
                return tutorialStrings[currentString];
            }
        }
        else if( currentState == State.STORY )
        {
            if (currentString < storyStrings.Capacity)
            {
                return storyStrings[currentString];
            }
        }

        return "";
    }

    public void ContinueTutorial()
    {
        if (RuleSatisfied(currentString))
        {
            currentString++;
            if (currentString == 2)
            {
                notebookInteractable.pickup = true;
            }
            else if (currentString == 10)
            {
                EndTutorial();
            }
        }
    }

    public void ContinueStory()
    {
        currentString++;
        if( currentString >= storyStrings.Capacity )
        {
            currentState = State.PLAY;
            raycasting.CloseDialogue();
        }
    }

    public void EndTutorial()
    {
        currentState = State.STORY;
        currentString = 0;
        entryDoors[0].locked = false;
        entryDoors[1].locked = false;
        entryDoors[0].Activate();
        entryDoors[1].Activate();
        raycasting.CloseDialogue();
    }

    private bool RuleSatisfied(int stage)
    {
        switch (currentState)
        {
            case State.TUTORIAL:
                
                switch (stage)
                {
                    case 2:
                        return exited;
                    case 4:
                        return pickup;
                    case 5:
                        return notebook;
                    case 6:
                        return saved;
                    default:
                        return true;
                }
            case State.STORY:
                break;
            case State.PLAY:
                break;
            case State.END:
                break;
        }
        return false;
    }

    public void PickUp(GameObject obj)
    {
        if (obj.GetComponent<Interactable>().objName.Equals("Notebook"))
        {
            if (!pickup)
            {
                currentString++;
            }
            pickup = true;
        }
    }

    public void ExitTutorial()
    {
        if (!exited)
        {
            currentString++;
            exited = true;
        }
    }

    public void SaveClue()
    {
        if (!saved && exited && pickup && notebook)
        {
            currentString++;
            saved = true;
        }
    }

    public void OpenNotebook()
    {
        if (!notebook && exited && pickup)
        {
            notebook = true;
            currentString++;
        }
    }

    public void UseAlexa()
    {
        alexa = !alexa;
    }

    public void CheckBoxes()
    {
        foreach (Toggle clue in checkboxes)
        {
            if (clue.isOn && clue == who)
            {
                who_check = true;
            }
            else if (clue.isOn && clue == why)
            {
                why_check = true;
            }
            else if (clue.isOn && clue == how)
            {
                how_check = true;
            }
            else if (clue.isOn)
            {
                who_check = false;
                why_check = false;
                how_check = false;
                incorrect_check++;
            }
        }
        if (who_check && why_check && how_check)
        {
            score = CalculateScore();
            controller.EndGame(score);
        }
    }

    private int CalculateScore()
    {
        return 100 - incorrect_check * 10;
    }
}
