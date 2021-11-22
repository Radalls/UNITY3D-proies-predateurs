using UnityEngine;

/// <summary>
///     <author>Cyril Dubos</author>
///     Class <c>Sightable</c> allows game objects to be sightable by the agents.
/// </summary>

public class Sightable : MonoBehaviour
{
    public void OnTriggerEnter(Collider collider)
    {
        SightManager manager = collider.GetComponent<SightManager>();

        if (manager && collider == manager.sphereCollider)
            if (manager.IsSightable(gameObject))
                manager.Add(gameObject);
    }

    public void OnTriggerStay(Collider collider)
    {
        SightManager manager = collider.GetComponent<SightManager>();

        if (manager && collider == manager.sphereCollider)
            if (manager.IsSightable(gameObject))
                manager.Add(gameObject);
            else
                manager.Remove(gameObject);
    }
    public void OnTriggerExit(Collider collider)
    {
        SightManager manager = collider.GetComponent<SightManager>();

        if (manager && collider == manager.sphereCollider)
            manager.Remove(gameObject);
    }
}
