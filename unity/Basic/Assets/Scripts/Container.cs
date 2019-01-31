using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Things;

namespace Containers
{
    public abstract class Container : Thing
    {

        public Key key;
        public bool locked;
        public bool open;

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

