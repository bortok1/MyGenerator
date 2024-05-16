using UnityEngine;

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
    public bool isSpecialPlace;

    public TileType(GameObject group, int weight, GameObject prefab, bool isSpecialPlace)
    {
        this.group = group;
        this.weight = weight;
        this.prefab = prefab;
        this.isSpecialPlace = isSpecialPlace;
    }

    public bool Equals(TileType tile)
    {
        return this.group.Equals(tile.group);
    }
}
