using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    public NPCController owner;
    public bool pickup;
    private bool show = false;
    public string objName;
    public int category;
    public string description;
    public Image image = null;
    public GUISkin skin;
    public GameObject hover = null;
    //private Material mat;
    //public Color colourGlowing;

    public enum Category : int {BOOK, KEY, CONTAINER, OBJECT};

    // Use this for initialization
    void Start()
    {
        //if (hover)
        //{
        //    mat = hover.GetComponent<Renderer>().material;
        //}
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public bool CanPickUp()
    {
        return pickup;
    }

    //once per GUI update
    private void OnGUI()
    {
        GUI.skin = skin;
        if (show)
        {
            //Draw the GUI layer
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), description);

            //show until escape pressed
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
            {
                if (image)
                {
                    image.gameObject.SetActive(false);
                }
                show = false;
            }
        }
    }

    public void InspectObject()
    {
        show = true;
        if (image)
        {
            image.gameObject.SetActive(true);
        }
    }

    public void Glow(bool status)
    {
        if (hover)
        {
            hover.SetActive(status);
        }
    }

    private void OnMouseOver()
    {
        if (hover)
        {
            hover.SetActive(true);
        }
        
    }

    private void OnMouseExit()
    {
        if (hover)
        {
            hover.SetActive(false);
        }
        
    }
}

