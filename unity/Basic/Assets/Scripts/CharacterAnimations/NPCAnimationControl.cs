using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationControl : MonoBehaviour
{
    public Animator anim;
    private float t1;
    private int random;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        t1 = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.time - t1) > 2)
        {
            random = (int)Mathf.Round(Random.Range(0.0f, 4.0f));
            anim.SetInteger("idle_num", random);
            t1 = Time.time;
        }
    }

    public void Give()
    {
        anim.SetTrigger("start_trade");
        anim.SetBool("is_trading", true);
    }

    public void Take()
    {
        anim.SetBool("is_trading", false);
    }
}
