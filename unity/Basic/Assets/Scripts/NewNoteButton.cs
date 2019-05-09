using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Notebook;

public class NewNoteButton : MonoBehaviour
{

		public Button submitButton;
		public Text inputText;
		public InputField input;
		public NotebookController notebook;

    // Start is called before the first frame update
    void Start()
    {
         submitButton.onClick.AddListener(() => { onClick(); });
         input.onEndEdit.AddListener(delegate {onClick();});
         Debug.Log("Adding Listeners");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
    	Debug.Log("Clicked");
    	notebook.MakeNote(inputText.text);
    	notebook.UpdateNotes();
    	inputText.text = "";
    }
}
