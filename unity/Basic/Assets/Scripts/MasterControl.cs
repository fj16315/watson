using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;
using Notebook;
using NPC;

public class MasterControl : MonoBehaviour {

    public GameObject masterCanvas;
    public NotebookController notebook;
    public RigidbodyFirstPersonController fpc;
    public GameState state;
    public InputField notesInput;

    // State variables
    public bool paused = false;
    private float launch;
    private float end;
    public bool finalForm = false;

    // Use this for initialization
    void Start () {
        launch = Time.realtimeSinceStartup;
        Debug.Log("Launch: " + launch.ToString());
        notebook.Activate(false);
    }

    void OpenNotebook(bool status)
    {
        notebook.Activate(status);
        Pause(status);
        if (status)
        {
            state.OpenNotebook();
            Cursor.visible = status;
        }
    }

	// Update is called once per frame
	void Update () {
        // TODO replace with switch
        if (!state.finalForm)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenNotebook(!paused);
                notebook.ChangePage((int)Page.MENU);
            }
            if (Input.GetKeyDown(KeyCode.C) && !notesInput.isFocused)
            {
                OpenNotebook(true);
                notebook.ChangePage((int)Page.CHARACTER);
            }
            if (Input.GetKeyDown(KeyCode.I) && !notesInput.isFocused)
            {
                OpenNotebook(true);
                notebook.ChangePage((int)Page.INVENTORY);
            }
            if (Input.GetKeyDown(KeyCode.N) && !notesInput.isFocused)
            {
                OpenNotebook(true);
                notebook.ChangePage((int)Page.NOTES);
            }
            if (Input.GetKeyDown(KeyCode.M) && !notesInput.isFocused)
            {
                OpenNotebook(true);
                notebook.ChangePage((int)Page.MAP);
            }
            if (Input.GetKeyDown(KeyCode.Tab) && !notesInput.isFocused)
            {
                OpenNotebook(!paused);
                notebook.ChangePage((int)notebook.currentPageEnum);
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

    public void EndGame(int score)
    {
        end = Time.realtimeSinceStartup;
        Debug.Log("End: " + end.ToString());
        Stats.Time = end - launch;
        Stats.Score = score;
        SceneManager.LoadScene("End_Scene", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }

    public void TogglePaused()
    {
        Pause(!paused);
    }
}
