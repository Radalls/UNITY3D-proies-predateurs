using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Méthode de génération procédurale inspirée par la série de tutoriels de Sebastian Lague :
 * https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
 */

public static class Noise
{
    private static NoiseSettings noiseSettings;

    /// <summary>
    /// author : Anis Koraichi
    /// Génération de la NoiseMap finale. Elle renvoie un tableau de float contenant les valeurs représentant les différents points de hauteurs de la carte
    /// </summary>
    /// <param name="noiseSettings"></param>
    /// <returns></returns>
    public static float[,] GenerateNoiseMap(NoiseSettings noiseSettings)
    {
        Noise.noiseSettings = noiseSettings;

        float[,] noiseMap = new float[noiseSettings.width, noiseSettings.height];
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        System.Random prng = new System.Random(noiseSettings.seed);
        Vector2[] octaveOffsets = new Vector2[noiseSettings.octaves];

        for (int i = 0; i < noiseSettings.octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + noiseSettings.offset.x;
            float offsetY = prng.Next(-100000, 100000) + noiseSettings.offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }


        if(noiseSettings.scale <= 0)
        {
            noiseSettings.scale = 0.0001f;
        }

        setNoiseMap(ref noiseMap, octaveOffsets, ref maxNoiseHeight, ref minNoiseHeight);

        for (int y = 0; y < noiseSettings.height; y++)
        {
            for (int x = 0; x < noiseSettings.width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
    /// <summary>
    /// author : Anis Koraichi
    /// Création de la noiseMap 
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <param name="octaveOffsets"></param>
    /// <param name="maxNoiseHeight"></param>
    /// <param name="minNoiseHeight"></param>
    private static void setNoiseMap(ref float[,] noiseMap, Vector2[] octaveOffsets, ref float maxNoiseHeight, ref float minNoiseHeight)
    {

        float halfWidth = noiseSettings.width / 2f;
        float halfHeight = noiseSettings.height / 2f;

        for (int y = 0; y < noiseSettings.height; y++)
        {
            for (int x = 0; x < noiseSettings.width; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //Génération d'une NoiseMap par octave
                for (int i = 0; i < noiseSettings.octaves; i++)
                {
                    float sampleX = (x - halfWidth) / noiseSettings.scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / noiseSettings.scale * frequency + octaveOffsets[i].y;

                    float perlin = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; //Affecte aux coordonnées sampleX et sampleY une valeur de Perlin comprise entre 0.0 et 1.0 représentant la couleur du point
                    noiseHeight += perlin * amplitude;
                    amplitude *= noiseSettings.persistance;
                    frequency *= noiseSettings.lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }
    }
}



public struct NoiseSettings
{
    public int width;
    public int height;
    public int seed;
    public float scale;
    public int octaves; //Nombre de NoiseMap à générer, permet d'influencer le niveau de détails de la NoiseMap finale
    public float persistance; //Contrôle la diminution de l'amplitude
    public float lacunarity; //Contrôle l'augmentation de la fréquence des octaves
    public Vector2 offset;
}
