using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttackObject : BaseAttackObject
{
    ParticleSystem ps;
    List<ParticleSystem.Particle> insideList = new List<ParticleSystem.Particle>();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ps = GetComponent<ParticleSystem>();

        return true;
    }

    private void OnParticleTrigger()
    {
        ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, insideList);

        foreach(ParticleSystem.Particle particle in insideList)
        {
            Debug.Log(particle);
            // particle.
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
    }
}
