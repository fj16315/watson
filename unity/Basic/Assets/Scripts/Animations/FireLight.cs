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
    public float flickerDistance = 12;
    public bool candle = false;

	// Use this for initialization
	void Start () {
        rand = new System.Random();
        l = transform.gameObject.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if ((counter++ % 3 ) == 0)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < flickerDistance)
            {
                diff = (float)(rand.Next(110, 160));
                if (candle) diff = diff * 0.4f;
                l.intensity = diff / 100.0F;
            }
        }
        if (counter % 8 == 0)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (rand.Next(0, 10) >= 5 && distance < flickerDistance)
            {
                l.transform.Translate(0, hop, 0);
                counter = 1;
                hop = hop * -1;
            }
        }
    }
}
