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
    string answer = "";
    string queryResponse = "";
    private AIController ai;
    public GameObject replyBubble;
    public Text answerBox;
    public GameObject saveButton;
    public GameObject nextButton;
    public GameObject textBubble;
    private NPCController currentCharacter;
    public NotebookController notebook;
    public AlexaInput alexa;

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

        // Set profiles
        profActress = new NPCProfile("Actress", fontActress, 50, 1f);
        profCountess = new NPCProfile("Countess", fontCountess, 60, 0.9f);
        profButler = new NPCProfile("Butler", fontButler, 40, 1f);
        profColonel = new NPCProfile("Colonel", fontColonel, 40, 1.3f);
        profGangster = new NPCProfile("Ganster", fontGangster, 50, 1.1f);
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
            //Cursor.visible = true;
            int width = 750;
            int height = 215;
            GUI.SetNextControlName("TextBox");

            int x = Screen.width/2 + 116;
            int y = Screen.height/2 + 164;

            //if (!(state.currentState == GameState.State.TUTORIAL && state.subState == 0))
            //{
            stringToEdit = GUI.TextField(new Rect(x, y, width, height), stringToEdit);
            GUI.FocusControl("TextBox");
            //}

            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "TextBox" && state.currentState == GameState.State.PLAY)
            {
                QueryAi();
            }

            //if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
            //{
            //    ShowScreen(currentCharacter);
            //}

        }
    }

    public void ShowScreen(NPCController character)
    {
        show = true;
        replyBubble.SetActive(true);
        currentCharacter = character;
        textBubble.SetActive(true);
        saveButton.SetActive(true);
        if (!(state.currentState == GameState.State.TUTORIAL))
        {
            alexa.StartSession();
            ai.StartSession();
            UpdateReply("");
        } else
        {
            nextButton.SetActive(true);
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
        answerBox.text = "";
        alexa.StopSession();
        Cursor.visible = false;
    }

    private void QueryAi()
    {
        queryResponse = ai.Run(stringToEdit);
        UpdateReply(queryResponse);
        Debug.Log(answer);
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
        notebook.LogResponse(currentCharacter, queryResponse);
    }

    public void NextButton()
    {
        state.ContinueTutorial();
        UpdateReply(state.NextString());
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