﻿using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using SimpleJSON;

public class AlexaInput : MonoBehaviour {

    public AIController ai;
    public DialogueScreen dialogue;

    private bool inDialogue;

    IEnumerator DownloadWebService()
    {
        while (true)
        {
            if(inDialogue)
            {
                WWW w = new WWW("http://brass-monkey-watson.herokuapp.com/?command");
                yield return w;

                print("Waiting for webservice\n");

                yield return new WaitForSecondsRealtime(1f);

                print("Received webservice\n");

                ExtractCommand(w.text);

                print("Extracted information");

                WWW y = new WWW("http://brass-monkey-watson.herokuapp.com/?command=empty");
                yield return y;

                print("Cleaned webservice");

                yield return new WaitForSecondsRealtime(5);
            }

            yield return null;
        }       
    }

    void ExtractCommand(string json)
    {
        var jsonstring = JSON.Parse(json);
        string command = jsonstring["command"];
        if (command == null || command == "") { return; }
        else {
            Debug.Log("Command = " + command);
            dialogue.UpdateQuestion(command);
            //ai.Query(command);
        }
    }

    public void StartSession()
    {
        inDialogue = true;
    }

    public void StopSession()
    {
        inDialogue = false;
    }

    void Start ()
    {
        Debug.Log("Started webservice import...\n");

        inDialogue = false;

        StartCoroutine(DownloadWebService());
    }
}