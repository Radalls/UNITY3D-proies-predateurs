using UnityEngine;
using UnityEngine.AI;

/* Méthode de génération procédurale inspirée par la série de tutoriels de Sebastian Lague :
 * https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
 */

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode { NoiseMap, ColourMap, Mesh } // Nous pouvons afficher notre monde de 3 différentes manières : sous forme de bruit de Perlin (NoiseMap), sous forme de carte en 2D (ColourMap) et sous forme de modèle complet en 3D (Mesh)
    public DrawMode drawMode;

    public const int mapChunkSize = 241; // Taille d'un chunk, c'est à dire d'une subdivision du monde. Actuellement, notre monde ne contient qu'un seul chunk cependant, il est possible de lui en affecter un nombre illimité.
    [Range(0,6)]
    public int LOD; // LOD = Level of Detail, nombre de vertex 
    public float noiseScale; 

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier; //Valeur permettant de modifier la hauteur des reliefs
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;
    private MapData mapData;
    public float[,] HeightMap { get
        {
            return mapData.heightMap;
        } }

    public float[] GetSpawnable
    {
        get
        {
            float[] spawnable = new float[2];
            spawnable[0] = -1;
            spawnable[1] = -1;

            foreach (TerrainType terrain in regions)
            {
                if (spawnable[0] == -1 && terrain.name.ToLower().Contains("grass"))
                    spawnable[0] = terrain.terrainHeight;
                else if (spawnable[0] != -1 && !terrain.name.ToLower().Contains("grass"))
                {
                    spawnable[1] = terrain.terrainHeight;
                    break ;
                }
            }
            if (spawnable[0] == -1 && spawnable[1] == -1)
                return new float[]{ 0, 1 };
            return spawnable;
        }
    }

    public static MapGenerator instance;

    public NavMeshSurface surface;

    /// <summary>
    /// author : Anis Koraichi
    /// Permet de charger le monde et ses textures lors du lancement
    /// </summary>
    private void Awake()
    {
        Environment.Parameters parameters = Menu.EditAction.parameters ?? Environment.Parameters.Load();
        seed = parameters.parameters["seed"].value;

        instance = this;
        DrawMapInEditor();
    }

    private void Start()
    {
        surface.BuildNavMesh();
    }

    /// <summary>
    /// author : Anis Koraichi
    /// Permet de visualiser le monde créer sur l'éditeur d'Unity
    /// </summary>
    public void DrawMapInEditor()
    {
        mapData = GenerateMapData();
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, LOD), TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
    }

    /// <summary>
    /// author : Anis Koraichi
    /// Méthode centrale de la génération de monde. Elle permet de créer un élèment de type MapData contenant toutes les informations nécessaires à la création du modèle du monde
    /// </summary>
    /// <returns></returns>
    MapData GenerateMapData()
    {
        NoiseSettings noiseSettings;
        noiseSettings.width = mapChunkSize;
        noiseSettings.height = mapChunkSize;
        noiseSettings.seed = seed;
        noiseSettings.scale = noiseScale;
        noiseSettings.octaves = octaves;
        noiseSettings.persistance = persistance;
        noiseSettings.lacunarity = lacunarity;
        noiseSettings.offset = offset;

        float[,] noiseMap = Noise.GenerateNoiseMap(noiseSettings);

        Color[] colourMap = new Color[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].terrainHeight)
                    {
                        colourMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colourMap);
    }

    /// <summary>
    /// author : Anis Koraichi
    /// Méthode permettant d'éviter d'entrer des données incohérentes
    /// </summary>
    private void OnValidate()
    {
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves < 0)
        {
            octaves = 0;
        }
    }

    public static Vector3 GetRandomPointOnMesh(Vector3 pos, float walkRadius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

        NavMesh.SamplePosition(pos + randomDirection, out NavMeshHit hit, walkRadius, 1);
        return hit.position;
    }
}


/// <summary>
/// author : Anis Koraichi
/// Structure contenant les informations définissant un type de terrain, c'est à dire, son nom, la hauteur maximale qu'il peut atteindre et la couleur qui lui est liée
/// </summary>
[System.Serializable]
public struct TerrainType
{
    public string name;
    public float terrainHeight;
    public Color colour;
}


/// <summary>
/// Structure contenant toutes les informations nécessaires à la génération d'un monde
/// </summary>
public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colourMap;

    public MapData(float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}