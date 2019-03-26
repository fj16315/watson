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
        int step = 5;
        int rounds;
        int counter = 0;
        
        bool activate = false;

        // Use this for initialization
        void Start()
        {
            rounds = 90 / step;
        }

        // Update is called once per frame
        void Update()
        {
            if (activate)
            {

                //float currentAngle = transform.eulerAngles.y;
                //float target = open ? angleClose : angleOpen;
                //int sweep = open ? 1 : -1;
                //float percentage = Mathf.Abs(currentAngle - target) / 90;
                //if (percentage < 0.2)
                //{
                //    activate = false;
                //    float angle = Mathf.Abs(transform.eulerAngles.y - target) * sweep * direction;
                //    Debug.Log(transform.eulerAngles.y);
                //    Debug.Log(target);
                //    Debug.Log(angle);
                //    transform.Rotate(Vector3.up, angle, 0);
                //    open = !open;
                //} else
                //{
                //    transform.Rotate(Vector3.up, speed * sweep * Time.deltaTime * direction, 0);
                //}

                if (counter++ < rounds)
                {
                    float target = open ? angleClose : angleOpen;
                    int sweep = open ? 1 : -1;
                    transform.Rotate(Vector3.up, step * sweep * direction, 0);
                } else
                {
                    open = !open;
                    activate = false;
                    counter = 0;
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

