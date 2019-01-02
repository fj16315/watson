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
        public bool single = true;
        public bool locked = false;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Activate()
        {
            if (single)
            {
                float currentAngle = transform.eulerAngles.y;
                Debug.Log(currentAngle);
                int sweep = open ? 1 : -1;
                transform.Rotate(Vector3.up, 90 * sweep, 0);
                open = !open;
            }
        }
    }
}

