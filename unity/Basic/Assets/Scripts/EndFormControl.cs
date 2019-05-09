using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFormControl : MonoBehaviour
{
    
    public GameObject form;
    public GameState state;
    public CameraRaycasting raycast;

    // Start is called before the first frame update
    void Start()
    {
        firstTimeHide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void firstTimeHide()
    {
        form.SetActive(false);
    }

    public void ShowForm()
    {
        form.SetActive(true);
        state.finalForm = true;
    }

    public void HideForm()
    {
        form.SetActive(false);
        state.finalForm = false;
        //unpause Game!
        raycast.CloseDialogue();
    }
}
