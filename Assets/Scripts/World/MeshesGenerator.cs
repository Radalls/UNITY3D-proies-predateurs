
using UnityEngine;


/* Méthode de génération procédurale inspirée par la série de tutoriels de Sebastian Lague :
 * https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
 */

/// <summary>
/// author : Anis Koraichi
/// Classe permettant de générer les mesh relatifs au monde
/// </summary>
public static class MeshGenerator
{

    /// <summary>
    /// Génération des toutes les informations nécessaires à la création des mesh de la carte. La méthode ne renvoie pas directement le mesh afin d'éviter d'éventuelles baisses de performances lors de la génération du monde.
    /// </summary>
    /// <param name="heightMap"></param>
    /// <param name="heightMultiplier"></param>
    /// <param name="_heightCurve"></param>
    /// <param name="levelOfDetail"></param>
    /// <returns></returns>
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        //Permet de centrer les mesh
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;
        int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
        int vertexIndex = 0;

        // Création de tous les vertices du monde
        for (int y = 0; y < height; y += meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x += meshSimplificationIncrement)
            {

                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y) ;
                meshData.uv[vertexIndex] = new Vector2(x / (float)width, y / (float)height); // Affecte une position à l'UV du vertex actuel

                //Condition permettant d'éviter de créer des triangles là où ce n'est pas nécessaire
                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices; // Tableau contenant tous les vertex de notre monde
    public int[] triangles; // Tableau contenant tous les triangles pouvant être représentés dans le monde
    public Vector2[] uv; // Tableau contenant un UV par vertex, permet d'appliquer une texture à chaque mesh

    int index;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uv = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }


    /// <summary>
    /// Création d'un triangle composé de 3 vertex
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    public void AddTriangle(int a, int b, int c)
    {
        triangles[index] = a;
        triangles[index + 1] = b;
        triangles[index + 2] = c;
        index += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals(); // Permet d'éviter d'éventuels problèmes relatifs à la simulation de lumière
        Physics.BakeMesh(mesh.GetInstanceID(), false);
        return mesh;
    }
}
