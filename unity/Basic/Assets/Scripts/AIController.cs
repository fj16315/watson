using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WatsonAI;
using System;
using System.Linq;
using System.IO;
using UnityEditor;

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

    public string Run(string input) => watson.Run(input);


}