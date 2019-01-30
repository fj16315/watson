using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScreen : MonoBehaviour {

    public GUISkin skin;
    bool show = false;
    //bool query = false;
    public string stringToEdit = "";

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {

    }

    void OnGUI()
    {
        GUI.skin = skin;
        if (show)
        {
            int width = 600;
            int height = 200;
            GUI.SetNextControlName("TextBox");

            stringToEdit = GUI.TextField(new Rect(Screen.width / 2 + 40, Screen.height / 2 + 40, width, height), stringToEdit);
            GUI.FocusControl("TextBox");

            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "TextBox")
            {
                Debug.Log("Query: " + stringToEdit);
            }
         
        }
    }

    public void ShowScreen()
    {
        show = true;
    }

    public void HideScreen()
    {
        show = false;
    }

}
