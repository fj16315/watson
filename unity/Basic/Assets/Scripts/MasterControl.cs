using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class MasterControl : MonoBehaviour {

    GameObject notebook, masterCanvas;
    bool paused = false;
    RigidbodyFirstPersonController fpc;

    // Use this for initialization
    void Start () {
        notebook = GameObject.Find("NotebookCanvas");
        fpc = Object.FindObjectOfType<RigidbodyFirstPersonController>();
        //Pause(true);
        notebook.SetActive(false);
        masterCanvas = GameObject.Find("MasterCanvas");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            notebook.SetActive(paused);
            Pause(paused);
            if (!paused)
            {
                Cursor.visible = false;
            }
        }
    }

    private void Pause(bool pause)
    {
        switch (pause)
        {
            case true:
                masterCanvas.SetActive(false);
                Time.timeScale = 0;
                //fpc.mouseLook.SetCursorLock(false);
                //fpc.mouseLook.lockCursor = false;
                fpc.mouseLook.SetCursorLock(false);
                Cursor.visible = true;
                break;
            default:
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
