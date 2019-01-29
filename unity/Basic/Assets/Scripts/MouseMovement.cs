using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour {

    public Vector3 pos;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnGUI()
    {
        pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = (pos);
    }
}
