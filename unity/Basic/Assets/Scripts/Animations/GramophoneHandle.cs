using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GramophoneHandle : MonoBehaviour {

    public float RotationSpeed = 100;
    public bool Activated = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Activated)
        {
            transform.Rotate(Vector3.left * (RotationSpeed * Time.deltaTime));
        }

    }
}
