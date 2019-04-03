using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPC;

namespace Things
{
    public class Thing : MonoBehaviour
    {
        public NPCController owner;
        public bool pickup;
        private bool show = false;
        public string objName;
        public int category;
        public string description;
        public GUISkin skin;

        public enum Category : int {BOOK, KEY, CONTAINER, OBJECT};

        // Use this for initialization
        void Start()
        {

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
                GUI.Box(new Rect(Screen.width, Screen.height, Screen.width/2, Screen.height/2), description);

                //show until escape pressed
                if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
                {
                    show = false;
                }
            }
        }

        public void InspectObject()
        {
            show = true;
        }
    }
}
