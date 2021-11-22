using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Méthode de génération procédurale inspirée par la série de tutoriels de Sebastian Lague :
 * https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3
 */

public static class TextureGenerator
{

    /// <summary>
    /// author : Anis Koraichi
    /// Application d'une couleur à la colourMap liée
    /// </summary>
    /// <param name="colourMap"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode =  TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// author : Anis Koraichi
    /// Création d'une colourMap à partir de la noiseMap liée puis application de leurs couleurs
    /// </summary>
    /// <param name="noiseMap"></param>
    /// <returns></returns>
    public static Texture2D TextureFromHeightMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Color[] colourMap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        return TextureFromColourMap(colourMap, width, height);
    }
}
