using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public Event currentEvent;
    public float remainingTime;

    public void Start()
    {
        remainingTime = currentEvent.duration;
    }

    public void Update()
    {
        if (remainingTime > 0)
        {
            currentEvent.Play();
            remainingTime -= Time.deltaTime;
        }
        else
        {
            Event nextEvent = currentEvent.GetRandomEvent();

            if (nextEvent != currentEvent) {
                currentEvent.Stop();
                nextEvent.Initialize();
            }
                
            currentEvent = nextEvent;
            remainingTime = currentEvent.duration;
        }
    }
}