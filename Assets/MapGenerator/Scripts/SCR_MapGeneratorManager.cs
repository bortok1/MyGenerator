using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tile
{
    public int X;
    public int Y;
    public TileType type;

    public Tile(int coordX, int coordY, TileType tileType)
    {
        X = coordX;
        Y = coordY;
        type = tileType;
    }
}

public struct TileType
{
    public GameObject group;
    public int weight;
    public GameObject prefab;
    public bool isSpectialPlace;

    public TileType(GameObject group, int weight, GameObject prefab, bool isSpecialPlace)
    {
        this.group = group;
        this.weight = weight;
        this.prefab = prefab;
        this.isSpectialPlace = isSpecialPlace;
    }
}


public class SCR_MapGeneratorManager : MonoBehaviour
{
    List<List<GameObject>> tileMapGrid = new(); // Final map of GameObjects tiles
    List<List<Tile>> dataMapGrid = new();       // Map of structs tiles

    List<TileType> tileTypes;

    // --------------------------------------------
    
    [SerializeField] GameObject river;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject forest;
    [SerializeField] GameObject desert;
    
    [SerializeField] GameObject castle;

    // --------------------------------------------

    [SerializeField] int randomPlacesToAdd = 5;
    [SerializeField] List<Tile> specialPlaces = new();

    // --------------------------------------------

    public int mapWidth = 160;
    public int mapHeight = 90;

    public int magnification = 7;

    public int xOffset = 0;    // <- +>
    public int yOffset = 0;    // v- +^

    // --------------------------------------------


    void Start()
    {
        CreateTileTypes(); // Create groups for every tile type in scene hierarchy

        dataMapGrid = SCR_PerlinNoiseMap.GenerateMap(mapWidth, mapHeight, xOffset, yOffset, magnification, tileTypes);

        if (randomPlacesToAdd != 0)
            specialPlaces.AddRange(SCR_PlacesRandomiser.AddRandomPlaces(randomPlacesToAdd, mapWidth, mapHeight, tileTypes));

        dataMapGrid = SCR_PlacesRandomiser.PlacePlaces(specialPlaces, dataMapGrid);

        GenerateMap();
    }

    void GenerateMap()
    {
        foreach(List<Tile> tileList in dataMapGrid)
        {
            tileMapGrid.Add(new List<GameObject>());
            foreach (Tile tile in tileList)
            {
                CreateSingleTile(tile);
            }
        }
    }
    private void CreateSingleTile(Tile singleTile)
    {
        GameObject createdTile = Instantiate(singleTile.type.prefab, singleTile.type.group.transform);

        createdTile.name = string.Format("tile_x({0})_y({1})", singleTile.X, singleTile.Y);
        createdTile.transform.localPosition = new Vector3(singleTile.X, singleTile.Y, 0);

        tileMapGrid[singleTile.X].Add(createdTile);
    }

    private void CreateTileTypes() 
    {
        tileTypes = new();
        tileTypes.Add(new TileType(new GameObject("River"), 50, river, false));
        tileTypes.Add(new TileType(new GameObject("Grass"), 10, grass, false));
        tileTypes.Add(new TileType(new GameObject("Forest"), 20, forest, false));
        tileTypes.Add(new TileType(new GameObject("Desert"), 30, desert, false));
        tileTypes.Add(new TileType(new GameObject("Castle"), 1, castle, true));
    }
}
