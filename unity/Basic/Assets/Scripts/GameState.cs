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

    private List<string> storyCharacters = new List<string>
            { "his wife the Countess", "an old friend the Colonel",
              "a young Actress", "his loyal Butler", "a no-good ruffian Gangster"
            };
    private NPCController currentCharacter;
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
        if ( currentString == 2)
        {
            int charIndex = GetIndexFromCharacter();
            Debug.Log("charIndex = " + charIndex);
            List<string> stringsToUse = GetStringsToUse(charIndex);
            storyStrings[2] = GenerateResponse(stringsToUse);
        }
        else if( currentString >= storyStrings.Capacity )
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

    public void SetCharacter(NPCController character)
    {
        currentCharacter = character;
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

    private int GetIndexFromCharacter()
    {
        switch(currentCharacter.charName.ToLower())
        {
            case "countess":
                return 0;
            case "colonel":
                return 1;
            case "actress":
                return 2;
            case "butler":
                return 3;
            case "gangster":
                return 4;
            default:
                return -1;
        }
    }

    private List<string> GetStringsToUse(int charIndex)
    {
        List<string> stringsToUse = new List<string>();
        int length = storyCharacters.Count;

        if (charIndex == 0)
        {
            stringsToUse.AddRange(storyCharacters.GetRange(1,length-1));
        }
        else if (charIndex == length-1)
        {
            stringsToUse.AddRange(storyCharacters.GetRange(0,length-1));
        }
        else
        {
            // GetRange(int startIndex, int count)
            stringsToUse.AddRange(storyCharacters.GetRange(0,charIndex));
            stringsToUse.AddRange(storyCharacters.GetRange(charIndex+1,length-charIndex-1));
        }

        return stringsToUse;
    }

    private string GenerateResponse(List<string> stringsToUse)
    {
        string response = "They included ";

        for(int i=0; i<stringsToUse.Count-2; i++)
        {
            response += stringsToUse[i] + ", ";
        }
        response += stringsToUse[stringsToUse.Count-2] + " and ";
        response += stringsToUse[stringsToUse.Count-1] + ".";

        return response;
    }
}
