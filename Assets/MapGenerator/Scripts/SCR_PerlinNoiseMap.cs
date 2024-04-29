using System.Collections.Generic;
using UnityEngine;

public static class SCR_PerlinNoiseMap
{
    // Create terrain using Perlin noise
    public static List<List<Tile>> GenerateMap(int mapWidth, int mapHeight, int xOffset, int yOffset, int magnification, List<TileType> tileTypes)
    {
        int specialPlaces = 0;
        foreach (TileType tileType in tileTypes)
        {
            if (tileType.isSpecialPlace)
                specialPlaces++;
        }

        List<List<Tile>> mapGrid = new();

        for(int x = 0; x < mapWidth; x++)
        {
            mapGrid.Add(new List<Tile>());

            for (int y = 0; y < mapHeight; y++)
            {
                int tileType = GetTypeUsingPerlin(x, y, xOffset, yOffset, magnification, tileTypes.Count - specialPlaces);
                mapGrid[x].Add(new Tile(x, y, tileTypes[tileType]));
            }
        }
        return mapGrid;
    }

    public static int GetTypeUsingPerlin(float x, float y, int xOffset, int yOffset, int magnification, int tileTypeCount)
    {
        float perlin = Mathf.PerlinNoise((x - xOffset) / magnification, (y - yOffset) / magnification);
        float clampPerlin = Mathf.Clamp(perlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * tileTypeCount;

        if(scalePerlin == tileTypeCount)
        {
            scalePerlin--;
        }

        return Mathf.FloorToInt(scalePerlin);
    }
}
