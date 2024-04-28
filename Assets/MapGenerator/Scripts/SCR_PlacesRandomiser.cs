using System.Collections.Generic;
using UnityEngine;

public static class SCR_PlacesRandomiser
{
    public static List<Tile> AddRandomPlaces(int randomPlacesToAdd, int mapWidth, int mapHeight, List<TileType> tileTypes)
    {
        List<TileType> specialTypes = new();
        foreach(TileType tileType in tileTypes)
        {
            if(tileType.isSpectialPlace)
                specialTypes.Add(tileType);
        }

        List<Tile> specialPlaces = new();
        for (int i = 0; i < randomPlacesToAdd; i++)
        {
            specialPlaces.Add(new Tile(Random.Range(0, mapWidth),
                                       Random.Range(0, mapHeight),
                                       specialTypes[Random.Range(0, specialTypes.Count - 1)]));
        }
        return specialPlaces;
    }

    public static List<List<Tile>> PlacePlaces(List<Tile> places, List<List<Tile>> dataTileMap)
    {
        foreach (Tile place in places)
        {
            dataTileMap[place.X][place.Y] = place;
        }

        return dataTileMap;
    }
}
