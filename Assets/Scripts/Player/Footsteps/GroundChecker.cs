using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Used to get the texture the player is currently walking on
/// </summary>
public class GroundChecker
{

    /// <summary>
    /// Finds the texture mix of the terrain at a certain position
    /// </summary>
    /// <param name="playerPos">The position</param>
    /// <param name="terrain">The Terrain</param>
    /// <returns>The texture mix</returns>
    float[] GetTextureMix(Vector3 position, Terrain terrain)
    {
        Vector3 terrainPos = terrain.transform.position;
        TerrainData terrainData = terrain.terrainData;

        int mapX = Mathf.RoundToInt((position.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((position.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);
        float[,,] alphaMaps = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellMix = new float[alphaMaps.GetUpperBound(2) + 1];
        for (int i = 0; i < cellMix.Length; i++)
        {
            cellMix[i] = alphaMaps[0, 0, i];
        }
        return cellMix;
    }

    /// <summary>
    /// Finds the dominant texture's name on the terrain at a certain position
    /// </summary>
    /// <param name="playerPos">The position</param>
    /// <param name="terrain">The Terrain</param>
    /// <returns>The dominant's layer name</returns>
    public string GetLayerName(Vector3 position, Terrain terrain)
    {
        float[] cellMix = GetTextureMix(position, terrain);
        float strongest = 0;
        int maxIndex = 0;
        for (int i = 0; i < cellMix.Length; i++)
        {
            if (cellMix[i] > strongest)
            {
                maxIndex = i;
                strongest = cellMix[i];
            }
        }
        return terrain.terrainData.terrainLayers[maxIndex].name;
    }
}
