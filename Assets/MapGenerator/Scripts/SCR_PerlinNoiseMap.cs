using System.Collections.Generic;
using UnityEngine;

public class SCR_PerlinNoiseMap : MonoBehaviour
{
    Dictionary<int, GameObject> tileSet;
    Dictionary<int, GameObject> tileGroups;

    [SerializeField] GameObject grass;
    [SerializeField] GameObject desert;
    [SerializeField] GameObject river;
    [SerializeField] GameObject forest;

    public int mapWidth = 160;
    public int mapHeight = 90;

    List<List<int>> noiseGrid = new();
    List<List<GameObject>> tileGrid = new();

    [SerializeField] float magnification = 7;

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

        if(scalePerlin == tileSet.Count)
        {
            scalePerlin--;
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
        tileSet.Add(1, grass);
        tileSet.Add(3, desert);
        tileSet.Add(0, river);
        tileSet.Add(2, forest);
    }

    public GameObject GetTile(int x, int y)
    {
        return tileGrid[x][y];
    }

    public void DestroyTile(int x, int y)
    {
        Destroy(tileGrid[x][y]);
        tileGrid[x][y] = null;
    }
}
