using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScreen : MonoBehaviour {

    public GUISkin skin;
    bool show = false;
    bool query = false;
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
            //DisableKey(KeyCode.Return);
            stringToEdit = GUI.TextField(new Rect(Screen.width / 2 + 40, Screen.height / 2 + 40, width, height), stringToEdit);
          
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("Query");
            }

            GUI.FocusControl("TextBox");
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

    //static void DisableKey(KeyCode key)
    //{
    //    DisableKeys(new KeyCode[] { key });
    //}

    //static void DisableKeys(KeyCode[] keys)
    //{
    //    if (!Event.current.isKey)
    //    {
    //        return;
    //    }

    //    foreach (KeyCode key in keys)
    //    {
    //        if (Event.current.keyCode == key)
    //        {
    //            //Event.current.Use();
    //        }
    //    }
    //}
}
