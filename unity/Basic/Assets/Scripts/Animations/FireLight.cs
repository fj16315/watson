using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLight : MonoBehaviour {

    Light l;
    System.Random rand;
    int counter = 1;
    public GameObject player;
    float distance;
    float hop = 0.02F;
    float diff;

	// Use this for initialization
	void Start () {
        rand = new System.Random();
        l = transform.gameObject.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if ((counter++ % 3 ) == 0)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < 12)
            {
                diff = (float)(rand.Next(110, 140));
                l.intensity = diff / 100.0F;
            }
        }
        if (counter % 8 == 0)
        {
            if (rand.Next(0, 10) >= 5)
            {
                l.transform.Translate(0, hop, 0);
                counter = 1;
                hop = hop * -1;
            }
        }
    }
}
