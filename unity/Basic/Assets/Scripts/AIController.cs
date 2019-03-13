using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using WatsonAI;
using System;
using System.Linq;
using System.IO;

public class AIController : MonoBehaviour
{

    private Watson watson;

    public AIController() {

        //var altDirectory = Directory.GetCurrentDirectory();
        this.watson = new Watson("Assets/DLLs/");



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