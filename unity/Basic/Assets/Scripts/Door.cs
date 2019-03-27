using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doors
{
    public class Door : MonoBehaviour
    {
        public bool open = false;
        public float angleOpen = 0;
        public float angleClose = 0;
        public bool locked = false;
        public int direction = 1;
        int step = 3;
        int rounds;
        int counter = 0;
        
        bool activate = false;

        // Use this for initialization
        void Start()
        {
            rounds = 90 / step;
        }

        private void FixedUpdate()
        {
            if (activate)
            {

                if (counter++ < rounds)
                {
                    //float target = open ? angleClose : angleOpen;
                    int sweep = open ? 1 : -1;
                    transform.Rotate(Vector3.up, step * sweep * direction, 0);
                }
                else
                {
                    open = !open;
                    activate = false;
                    counter = 0;
                }

            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate()
        {
            if (!locked)
            {
                activate = true;
            }
        }
    }
}

