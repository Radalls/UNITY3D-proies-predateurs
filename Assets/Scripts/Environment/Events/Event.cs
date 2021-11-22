using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    public float duration;
    public NextEvent[] nextEvents;

    public abstract void Initialize();
    public abstract void Play();
    public abstract void Stop();
    

    public Event GetRandomEvent()
    {
        float random = Random.value, sum = 0f;

        foreach (NextEvent ne in nextEvents)
        {
            if (random <= ne.p + sum)
                return ne.e;

            sum += ne.p;
        }

        return null;
    }
}

[System.Serializable]
public struct NextEvent {
    public float p;
    public Event e;
}