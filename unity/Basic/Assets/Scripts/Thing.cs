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
        public string objName;
        public int category;

        public enum Category : int {BOOK};

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
    }
}
