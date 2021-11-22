using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* Méthode de génération procédurale inspirée par la série de tutoriels de Sebastian Lague :
 * https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
 */

public class MapDisplay : MonoBehaviour
{
    public PhysicMaterial physicMaterial;
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    /// <summary>
    /// author : Anis Koraichi
    /// Application des textures dans les cas de drawMode égal à NoiseMap ou ColourMap
    /// </summary>
    /// <param name="texture"></param>
    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    /// <summary>
    /// author : Anis Koraichi
    /// Application des textures dans les cas de drawMode égal à Mesh
    /// </summary>
    /// <param name="texture"></param>
    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        if (meshFilter.gameObject.GetComponent<MeshCollider>() != null)
        {
            DestroyImmediate(meshFilter.gameObject.GetComponent<MeshCollider>());
        }
        Physics.BakeMesh(meshFilter.sharedMesh.GetInstanceID(), false);
        MeshCollider meshCollider = meshFilter.gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMaterial = physicMaterial;
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
