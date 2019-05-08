using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NPC;
using Notebook;

public class DialogueScreen : MonoBehaviour {

    public GUISkin skin;
    bool show = false;
    public GameState state;
    public string stringToEdit = "";
    private string lastQuestion = "";
    string answer = "";
    string queryResponse = "";
    private AIController ai;
    public Camera cam;
    public GameObject replyBubble;
    public Text answerBox;
    public GameObject saveButton;
    public GameObject nextButton;
    public GameObject skipButton;
    public GameObject textBubble;
    public GameObject player;
    private NPCController currentCharacter;
    public NotebookController notebook;
    public AlexaInput alexa;
    private bool freshReply = true;

    // Character Fonts
    public Font fontDetective;
    public Font fontActress;
    public Font fontCountess;
    public Font fontButler;
    public Font fontColonel;
    public Font fontGangster;
    public Font fontPolice;

    // NPC Profiles
    private NPCProfile profActress, profCountess, profButler, profColonel, profGangster, profPolice;

    // Use this for initialization
    void Start () {
        ai = Object.FindObjectOfType<AIController>();
        replyBubble.SetActive(false);
        textBubble.SetActive(false);
        saveButton.SetActive(false);
        skipButton.SetActive(false);

        // Set profiles
        profActress = new NPCProfile("Actress", fontActress, 50, 1f);
        profCountess = new NPCProfile("Countess", fontCountess, 60, 0.9f);
        profButler = new NPCProfile("Butler", fontButler, 40, 1f);
        profColonel = new NPCProfile("Colonel", fontColonel, 40, 1.3f);
        profGangster = new NPCProfile("Ganster", fontGangster, 40, 1.1f);
        profPolice = new NPCProfile("Police", fontPolice, 45, 1.1f);
    }

    // Update is called once per frame
    void Update () {

    }

    void OnGUI()
    {
        GUI.skin = skin;
        if (show)
        {
            int width = 750;
            int height = 215;
            GUI.SetNextControlName("TextBox");

            int x = Screen.width/2 + 116;
            int y = Screen.height/2 + 164;

            stringToEdit = GUI.TextField(new Rect(x, y, width, height), stringToEdit);
            GUI.FocusControl("TextBox");

            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "TextBox" && state.currentState == GameState.State.PLAY)
            {
                // Highlight dialogue box
                GUI.SetNextControlName("TextBox");
                stringToEdit = GUI.TextField(new Rect(x, y, width, height), stringToEdit);
                GUI.FocusControl("TextBox");
                QueryAi();
            }

        }
    }

    public void ShowScreen(NPCController character)
    {
        show = true;
        replyBubble.SetActive(true);
        currentCharacter = character;
        textBubble.SetActive(true);
        saveButton.SetActive(true);
        stringToEdit = "";
        lastQuestion = "";

        PositionCamera(currentCharacter);
        /* If not in tutorial and talking to Policeman, and not in story-dump, launch AI session.*/
        if (!((state.currentState == GameState.State.TUTORIAL &&
              currentCharacter.charName == "Police") ||
              (state.currentState == GameState.State.STORY)))
        {
            // If players choose to skip getting the story dump from the butler
            // then just change the state to PLAY.
            if(state.currentState == GameState.State.STORY)
            {
                state.currentState = GameState.State.PLAY;
            }
            alexa.StartSession();
            ai.StartSession(currentCharacter);
            UpdateReply("");
        }
        else if (state.currentState == GameState.State.STORY &&
                 currentCharacter.charName == "Police")
        {
            UpdateReply("Go and speak to one of the suspects to find out more about what happened.");
        }
        else
        {
            nextButton.SetActive(true);
            if (Application.isEditor && state.currentState == GameState.State.TUTORIAL)
            {
                skipButton.SetActive(true);
            }
            if (state.currentState == GameState.State.STORY)
            {
                state.SetCharacter(currentCharacter);
            }
            UpdateReply(state.NextString());
        }
        
    }

    public void HideScreen()
    {
        show = false;
        replyBubble.SetActive(false);
        textBubble.SetActive(false);
        saveButton.SetActive(false);
        nextButton.SetActive(false);
        skipButton.SetActive(false);
        answerBox.text = "";
        alexa.StopSession();
        Cursor.visible = false;
    }

    private void PositionCamera(NPCController character)
    {
        Transform face = character.gameObject.transform.Find("Face");
        
        Vector3 desiredPosition = new Vector3(0.25f*cam.pixelWidth, 0.33f*cam.pixelHeight,1);
        float desiredDistance = 0.8544f; // Calculated through trial and error
        
        Vector3 facePosition = face.position;
        Vector3 playerPosition = player.transform.position;

        Debug.Log("facePosition: " + facePosition);
        Debug.Log("playerPosition: " + playerPosition);
        
        Vector3 playerRotation = player.transform.rotation.eulerAngles;
        Vector3 faceRotation = face.rotation.eulerAngles;
        Vector3 halfTurn = new Vector3(0,180.0f,0);
        
        // Rotate the character to face the player
        character.transform.Rotate(playerRotation - faceRotation + halfTurn, Space.World);
        
        // Centre the camera on the face, then move the player forwards to desired distance.
        cam.transform.LookAt(facePosition);
        Ray fromCharacter = new Ray(facePosition, face.forward);
        Vector3 desiredPoint = fromCharacter.GetPoint(desiredDistance);

        player.transform.position = new Vector3(desiredPoint.x, playerPosition.y, desiredPoint.z);

        cam.transform.LookAt(cam.ScreenToWorldPoint(desiredPosition));

        // HS: position in code is in world coordinates, position in editor is relative coordinates
    }

    private void QueryAi()
    {
        if (stringToEdit != lastQuestion)
        {
            queryResponse = ai.Run(stringToEdit, 2);
            UpdateReply(queryResponse);
            freshReply = true;
            Debug.Log(answer);
            lastQuestion = stringToEdit;
        }
    }

    private void UpdateReply(string extra)
    {
        NPCProfile prof = GetProfile(currentCharacter.name);
        answer = "<b>" + prof.name + ":</b>\n" + extra;
        answerBox.fontSize = prof.size;
        answerBox.lineSpacing = prof.lineSpacing;
        answerBox.font = prof.font;
        answerBox.text = answer;
    }

    public void UpdateQuestion(string question)
    {
        stringToEdit = question;
        QueryAi();
    }

    private NPCProfile GetProfile(string name)
    {
        string val = name.ToLower();
        switch (val)
        {
            case "actress":
                return profActress;
            case "butler":
                return profButler;
            case "colonel":
                return profColonel;
            case "countess":
                return profCountess;
            case "gangster":
                return profGangster;
            case "police":
                return profPolice;

        }
        return new NPCProfile("", fontDetective, 0, 0);
    }

    public void SaveButton()
    {
        if (freshReply)
        {
            if (queryResponse == "" && state.currentState == GameState.State.TUTORIAL)
            {
                queryResponse = "My first clue!";
            }
            if (queryResponse != "")
            {
                notebook.LogResponse(currentCharacter, stringToEdit, queryResponse);
            }
            if (state.currentState == GameState.State.TUTORIAL)
            {
                state.SaveClue();
                UpdateReply(state.NextString());
            }
            freshReply = false;
        }
    }

    public void NextButton()
    {
        switch(state.currentState)
        {
            case GameState.State.TUTORIAL:
                state.ContinueTutorial();
                break;
            case GameState.State.STORY:
                state.ContinueStory();
                break;
        }
        UpdateReply(state.NextString());
    }

    public void SkipButton()
    {
        state.EndTutorial();
    }

}

public class NPCProfile
{
    public string name;
    public Font font;
    public int size;
    public float lineSpacing;

    public NPCProfile(string n, Font f, int s, float l)
    {
        name = n;
        font = f;
        size = s;
        lineSpacing = l;
    }
}