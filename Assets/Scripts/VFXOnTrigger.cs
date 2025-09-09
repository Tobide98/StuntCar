using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXOnTrigger : MonoBehaviour
{
    public ParticleSystem particle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            particle.Play();
        }
    }
}
