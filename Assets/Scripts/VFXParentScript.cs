using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXParentScript : MonoBehaviour
{
    public List<ParticleSystem> particles;
    
    public void StopAllParticles()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }
}
