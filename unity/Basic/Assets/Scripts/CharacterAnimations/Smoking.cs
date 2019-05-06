using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoking : MonoBehaviour
{
    public ParticleSystem smoke;

    public void BlowSmoke()
    {
        if (!smoke.isPlaying)
        {
            smoke.Play();
        }
    }
}
