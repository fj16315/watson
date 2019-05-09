using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFormControl : MonoBehaviour
{
    
    public GameObject form;
    public GameState state;
    public CameraRaycasting raycast;
    public DialogueScreen dialogue;

    // Start is called before the first frame update
    void Start()
    {
        FirstTimeHide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FirstTimeHide()
    {
        form.SetActive(false);
    }

    public void HideForm()
    {
        form.SetActive(false);
        state.finalForm = false;
    }

    public void ShowForm()
    {
        form.SetActive(true);
        state.finalForm = true;
    }

    public void EndingSubmit()
    {
        HideForm();
        if (!state.CheckBoxes())
        { 
            dialogue.ShowFailScreen();
        }
    }

    public void OpenForm()
    {
        ShowForm();
    }

    public void CloseForm()
    {
        form.SetActive(false);
        state.finalForm = false;
        //unpause Game!
        raycast.CloseDialogue();
    }
}
