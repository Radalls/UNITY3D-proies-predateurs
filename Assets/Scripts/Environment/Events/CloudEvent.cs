using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudEvent : Event
{
    public CloudGenerator cloudGenerator;

    public override void Initialize() { 
        cloudGenerator.Generate(20, new Vector3(30, 15, 30), new Vector3(75, 25, 75));
    }

    public override void Play() { }

    public override void Stop() { 
        cloudGenerator.Clear();
    }
}
