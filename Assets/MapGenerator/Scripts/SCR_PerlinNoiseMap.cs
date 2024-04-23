using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileSet;
    Dictionary<int, GameObject> tileGroups;

    public GameObject grass;
    public GameObject desert;
    public GameObject river;
    public GameObject forest;

    int mapWidth = 160;
    int mapHeight = 90;

    List<List<int>> noiseGrid = new List<List<int>>();
    List<List<GameObject>> tileGrid = new List<List<GameObject>>();

    public float magnification = 7;

    int xOffset = 0;    // <- +>
    int yOffset = 0;    // v- +^

    void Start()
    {
        CreateTileset();
        CreateTileGroups();
        GenerateMap();
    }

    private void GenerateMap()
    {
        for(int x = 0; x < mapWidth; x++)
        {
            noiseGrid.Add(new List<int>());
            tileGrid.Add(new List<GameObject>());

            for (int y = 0; y < mapHeight; y++)
            {
                int tileID = GetIDUsingPerlin(x, y);
                noiseGrid[x].Add(tileID);
                CreateTile(tileID, x, y);
            }
        }
    }

    private void CreateTile(int tileID, int x, int y)
    {
        GameObject tilePrefab = tileSet[tileID];
        GameObject tileGroup = tileGroups[tileID];
        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.name = string.Format("tile_x({0})_y({1})", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tileGrid[x].Add(tile);
    }

    private int GetIDUsingPerlin(int x, int y)
    {
        float perlin = Mathf.PerlinNoise((x - xOffset) / magnification, (y - yOffset) / magnification);
        float clampPerlin = Mathf.Clamp(perlin, 0.0f, 1.0f);
        float scalePerlin = clampPerlin * tileSet.Count;

        if(scalePerlin == 4)
        {
            scalePerlin = 3.0f;
        }

        return Mathf.FloorToInt(scalePerlin);
    }

    private void CreateTileGroups()
    {
        tileGroups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> tilePair in tileSet)
        {
            GameObject tileGroup = new GameObject(tilePair.Value.name);
            tileGroup.transform.parent = gameObject.transform;
            tileGroup.transform.localPosition = new Vector3(0, 0, 0);
            tileGroups.Add(tilePair.Key, tileGroup);
        }
    }

    void CreateTileset()
    {
        tileSet = new Dictionary<int, GameObject>();
        tileSet.Add(0, grass);
        tileSet.Add(1, desert);
        tileSet.Add(2, river);
        tileSet.Add(3, forest);
    }
}
