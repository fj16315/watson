using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WatsonAI;
using System;
using System.Linq;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;


public class AIController : MonoBehaviour
{

    private Watson watson;

    public AIController()
    {
      #if UNITY_EDITOR
        this.watson = new Watson("Assets/StreamingAssets/");
      #endif

      #if UNITY_STANDALONE
        this.watson = new Watson("Watson_Data/StreamingAssets/");
      #endif
  }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string Run(string input) 
    {
        SaveFile(input);

        return watson.Run(input); 
    }

    public void SaveFile(string data)
    {
        string path = Path.Combine(Application.persistentDataPath, "inputs.txt");
        Debug.Log(path);

       
        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(data);
            }
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(data);
        }
    }

   

}


