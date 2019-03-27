using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doors;
using NPC;

public class GameState : MonoBehaviour {

    public enum State : int { TUTORIAL, PLAY, END };
    public int subState = 0; // Define different rules within States
    public State currentState = State.TUTORIAL;

    public List<string> tutorialStrings;
    private int currentString = 0;
    private bool started = false;

    // Useful game things
    public List<Door> entryDoors;
    public NPCController police;
    public DialogueScreen dialogue;
    public CameraRaycasting raycasting;

    // Use this for initialization
    void Start () {
        currentState = State.TUTORIAL;
        subState = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (!started)
        {
            raycasting.SpeechDialogue(police);
            started = true;
        }
	}

    // -------------------- States --------------------

    // ------------------- TUTORIAL -------------------
    // 0 - First instruction, only character bubble

    public string NextString()
    {
        if (currentString < tutorialStrings.Capacity)
        {
            return tutorialStrings[currentString];
        }

        return "";
    }

    public void ContinueTutorial()
    {
        currentString++;
    }

}
