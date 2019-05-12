using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour {

    public Text score;
    public Text time;

	// Use this for initialization
	void Start () {


        score.text = Math.Max(Stats.Score, 0).ToString() + "%";

        int minutes = (int)(Stats.Time / 60);
        int seconds = (int)Stats.Time%60;
        time.text = minutes.ToString() + ":" + seconds.ToString();
        
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }
}
