using SimpleJSON;
using System.Collections;
using System.Net;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class AlexaInput : MonoBehaviour {

    public AIController ai;
    public DialogueScreen dialogue;
    public GameState state;

    private bool inDialogue;
    private string lastCommand;

    IEnumerator DownloadWebService()
    {
        while (true)
        {
            if(inDialogue && state.currentState == GameState.State.PLAY && state.alexa)
            {
                // TODO: Maybe use using block to ensure destruction of UnityWebRequest object?
                string uri = "http://brass-monkey-watson.herokuapp.com/";
                UnityWebRequest w = UnityWebRequest.Get(uri);
                yield return w.SendWebRequest();

                print("Waiting for webservice\n");

                yield return new WaitForSecondsRealtime(1f);

                print("Received webservice\n");

                ExtractCommand(w.downloadHandler.text);

                print("Extracted information");

                string uriToAppend = "/?command=empty&greeting=false";
                UnityWebRequest y = UnityWebRequest.Get(uri+uriToAppend);
                yield return y.SendWebRequest();

                print("Cleaned webservice");

                yield return new WaitForSecondsRealtime(5);
            }

            yield return null;
        }       
    }

    void ExtractCommand(string json)
    {
        Debug.Log(json);
        var jsonstring = JSON.Parse(json);
        Debug.Log(jsonstring);
        string command = jsonstring["command"];
        if (command == null || command == "") { return; }
        else {
            Debug.Log("Command = " + command);
            if (command != "undefined" && !command.Equals(lastCommand))
            {
                dialogue.UpdateQuestion(command);
                lastCommand = command;
            }
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
