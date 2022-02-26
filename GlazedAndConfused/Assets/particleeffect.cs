using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleeffect : MonoBehaviour
{
    public ParticleSystem karmaParticle;

    public void playParticle()
    {
        karmaParticle.transform.position = transform.position;
        karmaParticle.Play();
    }
}
