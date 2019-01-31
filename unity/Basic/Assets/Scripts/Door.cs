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
        public float speed = 100;
        bool activate = false;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (activate && single)
            {
                float currentAngle = transform.eulerAngles.y;
                float target = open ? angleClose : angleOpen;
                int sweep = open ? 1 : -1;
                Debug.Log(currentAngle);
                if (Mathf.Abs(currentAngle - target) > 1)
                {
                    transform.Rotate(Vector3.up, speed * sweep * Time.deltaTime, 0);
                }
                else
                {
                    activate = false;
                    open = !open;
                }
            }
        }

        public void Activate()
        {
            if (single && !locked)
            {
                activate = true;
            }
        }
    }
}

