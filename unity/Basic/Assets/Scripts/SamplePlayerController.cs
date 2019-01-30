using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayerController : MonoBehaviour {

    public float speed;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        speed = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate ()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0.0f, moveY);

        rb.AddForce(movement * speed);
    }
}
