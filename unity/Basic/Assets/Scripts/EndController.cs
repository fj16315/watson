﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndController : MonoBehaviour {

    public Text score;
    public Text time;

	// Use this for initialization
	void Start () {
        score.text = Stats.Score.ToString();
        time.text = Stats.Time.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
