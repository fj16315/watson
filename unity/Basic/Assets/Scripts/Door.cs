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
        bool activate = false;

        Transform openTransform;
        Transform closedTransform;
        public float speedF = 1.0F;
        float startTime;
        float journeyLength;


        // Use this for initialization
        void Start()
        {
            openTransform = transform;
            closedTransform = transform;
            //openTransform.Rotate(Vector3.up, 90, 0);
            journeyLength = Vector3.Distance(openTransform.position, closedTransform.position);
        }

        // Update is called once per frame
        void Update()
        {
            if (activate)
            {
                float currentAngle = transform.eulerAngles.y;
                float target = open ? angleClose : angleOpen;
                int sweep = open ? 1 : -1;
                //Debug.Log(currentAngle);
                //if (Mathf.Abs(currentAngle - target) > 1)
                //{
                //    transform.Rotate(Vector3.up, speed * sweep * Time.deltaTime, 0);
                //}

                //if ()
                //{
                //    activate = false;
                //    open = !open;
                //}
            }

            //if (activate)
            //{

            //}

        }

        public void Activate()
        {
            if (!locked)
            {
                activate = true;
                startTime = Time.time;
                
            }
        }
    }
}

