using System.Collections.Generic;
using UnityEngine;

public class SCR_MapGeneratorManager : MonoBehaviour
{
    List<List<GameObject>> tileMapGrid = new(); // Final map of GameObjects tiles
    List<List<Tile>> dataMapGrid = new();       // Map of structs tiles

    List<TileType> tileTypes = new();

    
    Dictionary<int, GameObject> DirToPrefab;   // Assign direction value to prefab

    #region Prefabs for tiles
    [SerializeField] GameObject river;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject forest;
    [SerializeField] GameObject desert;

    [SerializeField] GameObject castle;

    // D = Down
    // U = Up
    // L = Left
    // R = Right
    [SerializeField] GameObject roadDL;
    [SerializeField] GameObject roadDR;
    [SerializeField] GameObject roadLR;
    [SerializeField] GameObject roadTD;
    [SerializeField] GameObject roadTL;
    [SerializeField] GameObject roadTR;
    [SerializeField] GameObject roadTU;
    [SerializeField] GameObject roadUD;
    [SerializeField] GameObject roadUL;
    [SerializeField] GameObject roadUR;
    [SerializeField] GameObject roadX;
    #endregion

    #region Variables for special places
    [SerializeField] int randomPlacesToAdd = 20;
    List<Tile> specialPlaces = new();
    #endregion

    #region Variables for whole map generation
    public int mapWidth = 160;
    public int mapHeight = 90;

    public int magnification = 14;

    // Used to set offset for Perlin noise
    public int xOffset = 0;    // to move left decrease to move right increase
    public int yOffset = 0;    // to move left decrease to move right increase
    #endregion


    void Start()
    {
        CreateTileTypes();

        // Terrain data grid
        dataMapGrid = SCR_PerlinNoiseMap.GenerateDataGrid(mapWidth, mapHeight, xOffset, yOffset, magnification, tileTypes);


        // Special places
        if (randomPlacesToAdd != 0)
            specialPlaces.AddRange(SCR_PlacesHelper.AddRandomPlaces(randomPlacesToAdd, mapWidth, mapHeight, tileTypes));

        dataMapGrid = SCR_PlacesHelper.PlacePlaces(specialPlaces, dataMapGrid);

        // Road
        SetRoadConnections();

        // Set Dictionary used in placing roads
        SetDirToPrefab();

        // Final map of GameObjects
        GenerateMap();
    }

    private void SetRoadConnections() // between special places
    {
        if (specialPlaces.Count < 2) return;

        List<int> connected = new();

        for (int i = 0; i < specialPlaces.Count; i++)
        {
            if (connected.Contains(i)) continue;

            int connection = -1;
            for (int j = i + 1; j < specialPlaces.Count; j++)
            {
                if (connected.Contains(j)) continue;

                connection = j; break;
            }

            if (connection == -1) continue;

            connected.Add(connection);

            PutPathOnDataGrid(SCR_RoadGenerator.FindBestNode(dataMapGrid, specialPlaces[i], specialPlaces[connection]));
        }
    }

    private void PutPathOnDataGrid(PathNode node)
    {
        if (node.cameFromNode == null)
            return;
        
        if(!dataMapGrid[node.tile.X][node.tile.Y].type.isSpecialPlace)
            dataMapGrid[node.tile.X][node.tile.Y] = new Tile(node.tile.X, node.tile.Y, tileTypes[tileTypes.Count - 1]);

        PutPathOnDataGrid(node.cameFromNode);
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
        GameObject prefab = null;

        bool isARoad = singleTile.type.Equals(tileTypes[tileTypes.Count - 1]);
        if (isARoad)
        {
            prefab = ChooseRoadTile(singleTile);
        }
        else
        {
            prefab = singleTile.type.prefab;
        }

        GameObject createdTile = Instantiate(prefab, singleTile.type.group.transform);

        createdTile.name = string.Format("tile_x({0})_y({1})", singleTile.X, singleTile.Y);
        createdTile.transform.localPosition = new Vector3(singleTile.X, singleTile.Y, 0);

        tileMapGrid[singleTile.X].Add(createdTile);
    }

    private GameObject ChooseRoadTile(Tile singleTile)
    {
        // in 4 bit variable
        // if road up set 2^0 to 1
        // if road down set 2^1 to 1
        // if road left set 2^2 to 1
        // if road right set 2^3 to 1

        int dir = 0;

        TileType neighbourType;

        if (singleTile.Y + 1 < dataMapGrid[singleTile.X].Count)
        {
            neighbourType = dataMapGrid[singleTile.X][singleTile.Y + 1].type;
            if (singleTile.Y + 1 < dataMapGrid[singleTile.X].Count &&
                neighbourType.Equals(singleTile.type) ||
                neighbourType.isSpecialPlace)
            {
                dir += 1;
            }
        }

        if (singleTile.Y - 1 >= 0)
        {
            neighbourType = dataMapGrid[singleTile.X][singleTile.Y - 1].type;
            if (singleTile.Y - 1 >= 0 &&
                neighbourType.Equals(singleTile.type) ||
                neighbourType.isSpecialPlace)
            {
                dir += 2;
            }
        }

        if (singleTile.X - 1 >= 0)
        {
            neighbourType = dataMapGrid[singleTile.X - 1][singleTile.Y].type;
            if (singleTile.X - 1 >= 0 &&
                neighbourType.Equals(singleTile.type) ||
                neighbourType.isSpecialPlace)
            {
                dir += 4;
            }
        }

        if (singleTile.X + 1 < dataMapGrid.Count)
        {
            neighbourType = dataMapGrid[singleTile.X + 1][singleTile.Y].type;
            if (singleTile.X + 1 < dataMapGrid.Count &&
                neighbourType.Equals(singleTile.type) ||
                neighbourType.isSpecialPlace)
            {
                dir += 8;
            }
        }

        return DirToPrefab[dir];
    }

    private void SetDirToPrefab()
    {
        DirToPrefab = new()
        {
            { 1, roadUD },{ 2, roadUD },{ 3, roadUD },
            { 4, roadLR },{ 5, roadUL },{ 6, roadDL },
            { 7, roadTL },{ 8, roadLR },{ 9, roadUR },
            { 10, roadDR },{ 11, roadTR },{ 12, roadLR },
            { 13, roadTU },{ 14, roadTD },{ 15, roadX },
        };
    }

    private void CreateTileTypes() 
    {
        tileTypes = new();
        tileTypes.Add(new TileType(new GameObject("River"), 200, river, false));
        tileTypes.Add(new TileType(new GameObject("Grass"), 30, grass, false));
        tileTypes.Add(new TileType(new GameObject("Forest"), 60, forest, false));
        tileTypes.Add(new TileType(new GameObject("Desert"), 90, desert, false));
        tileTypes.Add(new TileType(new GameObject("Castle"), 1, castle, true));
        tileTypes.Add(new TileType(new GameObject("Road"), 1, roadLR, true));     // Road has to be the last one

        foreach(TileType tileType in tileTypes)
            tileType.group.transform.parent = this.transform;
    }
}
