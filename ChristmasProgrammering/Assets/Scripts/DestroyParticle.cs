using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    private ParticleSystem particleSys;
    private void Awake()
    {
        particleSys = GetComponent<ParticleSystem>();  
    }
    void Update()
    {
        if (particleSys.isStopped)
        {
            Destroy(transform.gameObject);
        }
    }
}
