using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     <author>Cyril Dubos</author>
///     Class <c>CloudGenerator</c> generates cloud game objects in the scene.
/// </summary>
public class CloudGenerator : MonoBehaviour
{
    /// <summary>
    ///     <c>prefab</c> is cloud prefab that will be generated.
    /// </summary>
    public GameObject prefab;
    
    /// <summary>
    ///     <c>clouds</c> is the list of generated game objects.
    /// </summary>
    private GameObject[] clouds;

    /// <summary>
    ///    This method generates the clouds.
    /// </summary>
    /// <param name="count">the number of generated clouds</param>
    /// <param name="minimumScale">the minimum scale of a cloud</param>
    /// <param name="maximumScale">the maximum scale of a cloud</param>
    public void Generate(int count, Vector3 minimumScale, Vector3 maximumScale)
    {
        clouds = new GameObject[count];

        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();

        Vector3 position = gameObject.transform.position;
        Vector3 size = boxCollider.size;

        Vector3 minimumPosition = position - size / 2;
        Vector3 maximumPosition = position + size / 2;

        System.Random random = new System.Random();

        for (int i = 0; i < clouds.Length; i++)
        {
            clouds[i] = Instantiate(prefab);
            clouds[i].transform.parent = gameObject.transform;
            clouds[i].transform.position = Utilities.GetRandomVector3(random, minimumPosition, maximumPosition);
            clouds[i].transform.localScale = Utilities.GetRandomVector3(random, minimumScale, maximumScale);
        }
    }

    /// <summary>
    ///    This method destroys all the generated clouds.
    /// </summary>
    public void Clear() {
        for (int i = 0; i < clouds.Length; i++)
        {
            Destroy(clouds[i]);
        }

        clouds = new GameObject[0];
    }
}
