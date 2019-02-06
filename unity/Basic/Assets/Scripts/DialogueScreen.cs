using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScreen : MonoBehaviour {

    public GUISkin skin;
    bool show = false;
    //bool query = false;
    public string stringToEdit = "";
    string answer = "";
    private AIController ai;

    // Use this for initialization
    void Start () {
        ai = Object.FindObjectOfType<AIController>();
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
            int width = 600;
            int height = 200;
            GUI.SetNextControlName("TextBox");

            int x = Screen.width / 2 + 40;
            int y = Screen.height / 2 + 40;

            stringToEdit = GUI.TextField(new Rect(x, y, width, height), stringToEdit);
            GUI.FocusControl("TextBox");

            GUI.Box(new Rect(x - width - 10, y - height, width, height), answer);

            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "TextBox")
            {
                answer = ai.Query(stringToEdit);
                Debug.Log(answer);
            }
         
        }
    }

    public void ShowScreen()
    {
        show = true;
        answer = "";
    }

    public void HideScreen()
    {
        show = false;
        Cursor.visible = false;
    }

}
