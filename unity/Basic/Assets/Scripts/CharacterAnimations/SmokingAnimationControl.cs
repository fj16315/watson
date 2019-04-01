using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokingAnimationControl : MonoBehaviour
{
    public ParticleSystem smoke;
    public Animator anim;
    private float t1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        t1 = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle2"))
        {
            if (!smoke.isPlaying)
            {
                smoke.Play();
            }
        }
    }
}
