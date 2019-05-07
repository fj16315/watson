using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Containers
{
    public abstract class Container : Interactable
    {

        public Key key;
        public bool locked;
        public bool open;
        public GameObject contents;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public abstract void Open();

        public abstract void Close();

        public abstract bool Unlockable();

        public abstract void Activate();
    }
}

