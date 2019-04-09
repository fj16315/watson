using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Math;

public class EndController : MonoBehaviour {

    public Text score;
    public Text time;

	// Use this for initialization
	void Start () {

        score.text = Stats.Score.ToString() + "%";

        int minutes = (int)(Stats.Score / 60);
        int seconds = Stats.Score - (minutes * 60);
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
