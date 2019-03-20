using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using SimpleJSON;

public class AlexaInput : MonoBehaviour {

    public AIController ai;

    IEnumerator DownloadWebService()
    {
        while (true)
        {
            WWW w = new WWW("http://brass-monkey-alexa.herokuapp.com/?command");
            yield return w;

            print("Waiting for webservice\n");

            yield return new WaitForSeconds(1f);

            print("Received webservice\n");

            ExtractCommand(w.text);

            print("Extracted information");

            WWW y = new WWW("http://brass-monkey-alexa.herokuapp.com/?command=empty");
            yield return y;

            print("Cleaned webservice");

            yield return new WaitForSeconds(5);
        }
    }

    void ExtractCommand(string json)
    {
        var jsonstring = JSON.Parse(json);
        string command = jsonstring["command"];
        print(command);
        if (command == null) { return; }
        else { ai.Query(command); }
    }
}
