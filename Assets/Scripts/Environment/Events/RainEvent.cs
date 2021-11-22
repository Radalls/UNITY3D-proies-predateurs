using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEvent : Event
{
    public CloudGenerator cloudGenerator;

#if UNITY_EDITOR
    public new ParticleSystem particleSystem;
#else
    public ParticleSystem particleSystem;
#endif

    public void Start()
    {
        particleSystem.Stop();
    }

    public override void Initialize() { 
        cloudGenerator.Generate(50, new Vector3(50, 15, 50), new Vector3(100, 25, 100));
    }

    public override void Play()
    {
        if (particleSystem.isStopped)
            particleSystem.Play();
    }

    public override void Stop()
    {
        cloudGenerator.Clear();

        if (particleSystem.isPlaying)
            particleSystem.Stop();
    }
}
