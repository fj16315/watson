using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MasterControl : MonoBehaviour {

    GameObject notebook;
    bool paused = false;
    RigidbodyFirstPersonController fpc;

    // Use this for initialization
    void Start () {
        notebook = GameObject.Find("NotebookCanvas");
        notebook.SetActive(false);
        fpc = Object.FindObjectOfType<RigidbodyFirstPersonController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            notebook.SetActive(paused);
            Pause(paused);
        }
    }

    private void Pause(bool pause)
    {
        switch (pause)
        {
            case true:
                Time.timeScale = 0;
                //fpc.mouseLook.lockCursor = false;
                fpc.mouseLook.SetCursorLock(false);
                break;
            default:
                Time.timeScale = 1;
                //fpc.mouseLook.lockCursor = true;
                fpc.mouseLook.SetCursorLock(true);
                break;
        }
    }
}
