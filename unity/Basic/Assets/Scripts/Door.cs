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
        public float speed = 100;
        public int direction = 1;
        
        bool activate = false;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (activate)
            {
                float currentAngle = transform.eulerAngles.y;
                float target = open ? angleClose : angleOpen;
                int sweep = open ? 1 : -1;
                float percentage = Mathf.Abs(currentAngle - target) / 90;
                if (percentage < 0.04)
                {
                    activate = false;
                    transform.Rotate(Vector3.up, Mathf.Abs(currentAngle - target) * sweep * direction, 0);
                    open = !open;
                } else
                {
                    transform.Rotate(Vector3.up, speed * sweep * Time.deltaTime * direction, 0);
                }
            }

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

