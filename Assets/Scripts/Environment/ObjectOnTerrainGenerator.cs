using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///     <author>Cyril Dubos</author>
///     Class <c>ObjectOnTerrainGenerator</c> generates some objects on a terrain.
/// </summary>
public class ObjectOnTerrainGenerator : MonoBehaviour
{
    /// <summary>
    ///     <c>prefabs</c> is the list of prefabs that will be generated.
    /// </summary>
    public GameObject[] prefabs;

    /// <summary>
    ///     <c>terrain</c> is the terrain game object on which the objects will be generated.
    /// </summary>
    public GameObject terrain;

    [Space()]

    /// <summary>
    ///     <c>groupCount</c> is the number of group of objects.
    /// </summary>
    [MinAttribute(1)]
    public int groupCount;

    
    /// <summary>
    ///     <c>isUniformGroup</c> is <c>true</c> if groups will be unifom, <c>false</c> otherwise.
    /// </summary>
    public bool isUniformGroup;

    /// <summary>
    ///     <c>isNavMeshObstacle</c> is <c>true</c> if objects are Nav Mesh Obstacle.
    /// </summary>
    public bool isNavMeshObstacle;

    [Space()]

    /// <summary>
    ///     <c>offset</c> is is the offset used to place the game objects.
    /// </summary>
    public Vector3 offset;

    [Header("Size")]

    /// <summary>
    ///     <c>minimumSize</c> is the minimum size of a group.
    /// </summary>
    [MinAttribute(1)]
    public int minimumSize;

    /// <summary>
    ///     <c>maximumSize</c> is the maximum size of a group.
    /// </summary>
    [MinAttribute(1)]
    public int maximumSize;

    [Header("Space")]

    /// <summary>
    ///     <c>minimumSpace</c> is the minimum space between two objects of a same group.
    /// </summary>
    public int minimumSpace;
    
    /// <summary>
    ///     <c>maximumSpace</c> is the maximum space between two objects of a same group.
    /// </summary>
    public int maximumSpace;

    [Header("Scale")]

    /// <summary>
    ///     <c>minimumScale</c> is the minimum scale of a generated object.
    /// </summary>
    public int minimumScale;

    /// <summary>
    ///     <c>maximumScale</c> is the maximum scale of a generated object.
    /// </summary>
    public int maximumScale;

    private int layerMask;

    [HideInInspector]
    public bool done = false;

    public void Start()
    {
        layerMask = LayerMask.GetMask(LayerMask.LayerToName(terrain.layer));

        System.Random random = new System.Random();

        GenerateGroups(random);

        done = true;
    }

    /// <summary>
    ///    This method generates all the groups.
    /// </summary>
    /// <param name="random">a <c>System.Random</c> object</param>
    private void GenerateGroups(System.Random random)
    {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();

        Vector3 position = gameObject.transform.position;
        Vector3 size = boxCollider.size;

        Vector3 origin = new Vector3();
        Vector3 minimumOrigin = position - size / 2;
        Vector3 maximumOrigin = position + size / 2;

        for (int i = 0; i < groupCount; i++)
        {
            RaycastHit hit;

            do {
                origin = Utilities.GetRandomVector3(random, minimumOrigin, maximumOrigin);
                origin.y = maximumOrigin.y;
            } while(!(Physics.Raycast(origin, Vector3.down, out hit) && boxCollider.ClosestPoint(hit.point) == hit.point && hit.collider.gameObject == terrain));

            // Debug.DrawLine(hit.point, hit.point + Vector3.up * 100, Color.blue, 100f);

            GenerateGroup(random, origin);
        }
    }

    /// <summary>
    ///    This method generates all the objects of a group.
    /// </summary>
    /// <param name="random">a <c>System.Random</c> object</param>
    /// <param name="origin">the origin of the group</param>
    private void GenerateGroup(System.Random random, Vector3 origin) {
        GameObject prefab = isUniformGroup ? GetRandomPrefab(random) : null;

        GameObject g = new GameObject(prefab == null ? gameObject.name : prefab.name + "s");

        g.transform.position = origin;
        g.transform.parent = gameObject.transform;

        for (int i = 0; i < random.Next(minimumSize, maximumSize); i++)
        {
            origin = GenerateObject(random, g, origin, prefab);
        }
    }

    /// <summary>
    ///    This method generates a new object.
    /// </summary>
    /// <param name="random">a <c>System.Random</c> object</param>
    /// <param name="parent">the parent of the object (the group object)</param>
    /// <param name="origin">the origin of the object (the previous object position)</param>
    /// <param name="prefab">the prefab of the object that will be used</param>
    private Vector3 GenerateObject(System.Random random, GameObject parent, Vector3 origin, GameObject prefab) {
        if (prefab == null)
            prefab = GetRandomPrefab(random);

        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();

        float space = (float) Utilities.GetRandomDouble(random, minimumSpace, maximumSpace);

        Vector3 minimumOrigin = new Vector3(origin.x - space, origin.y, origin.z - space);
        Vector3 maximumOrigin = new Vector3(origin.x + space, origin.y, origin.z + space);

        RaycastHit hit;

        do {
            origin = Utilities.GetRandomVector3(random, minimumOrigin, maximumOrigin);
        } while(!(Physics.Raycast(origin, Vector3.down, out hit) && boxCollider.ClosestPoint(hit.point) == hit.point && hit.collider.gameObject == terrain));

        // Debug.DrawLine(hit.point, hit.point + Vector3.up * 100, Color.red, 100f);

        GameObject o = Instantiate(prefab);
        o.transform.parent = parent.transform;
        o.transform.position = hit.point + offset;
        o.transform.rotation = Utilities.GetRandomQuaternion(random, false, true, false);
        o.transform.localScale = Utilities.GetRandomVector3(random, minimumScale, maximumScale);

        if (isNavMeshObstacle)
        {
            NavMeshObstacle navMeshObstacle = o.AddComponent<NavMeshObstacle>();
            navMeshObstacle.shape = NavMeshObstacleShape.Capsule;
            navMeshObstacle.carving = true;
        }
    
        return origin;
    }

    /// <summary>
    ///    This method returns a random prefab from the <c>prefabs</c> list.
    /// </summary>
    private GameObject GetRandomPrefab(System.Random random)
    {
        return prefabs[random.Next(0, prefabs.Length)];
    }
}
