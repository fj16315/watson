using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using Notebook;
using NPC;

public class MasterControl : MonoBehaviour {

    public GameObject masterCanvas;
    public NotebookController notebook;
    public bool paused = false;
    public RigidbodyFirstPersonController fpc;

    // Scoring variables
    public NPCController who;
    public GameObject what;

    // Use this for initialization
    void Start () {
        notebook.Activate(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            notebook.Activate(!paused);
            Pause(!paused);
            if (!paused)
            {
                Cursor.visible = false;
            }
        }
    }

    public void Pause(bool pause)
    {
        switch (pause)
        {
            case true:
                paused = true;
                masterCanvas.SetActive(false);
                Time.timeScale = 0;
                //fpc.mouseLook.SetCursorLock(false);
                //fpc.mouseLook.lockCursor = false;
                fpc.mouseLook.SetCursorLock(false);
                Cursor.visible = true;
                break;
            default:
                paused = false;
                masterCanvas.SetActive(true);
                Time.timeScale = 1;
                //fpc.mouseLook.SetCursorLock(true);
                //fpc.mouseLook.lockCursor = true;
                fpc.mouseLook.SetCursorLock(true);
                Cursor.visible = false;
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
