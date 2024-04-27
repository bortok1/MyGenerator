using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpecialPlace
{
    public int X;
    public int Y;
    public int type;

    public SpecialPlace(int coordX, int coordY, int dictionaryNr)
    {
        X = coordX;
        Y = coordY;
        type = dictionaryNr;
    }
}

public class SCR_PlacesRandomiser : MonoBehaviour
{
    [SerializeField] GameObject castle;

    [SerializeField] bool bJustAddSomePlacesForMe = true;
    [SerializeField] public List<SpecialPlace> places = new();

    Dictionary<int, GameObject> tileSet;

    SCR_PerlinNoiseMap perlinMap;
    int mapWidth;
    int mapHeight;

    void Start()
    {
        perlinMap = GetComponent<SCR_PerlinNoiseMap>();
        mapWidth = perlinMap.mapWidth;
        mapHeight = perlinMap.mapHeight;

        CreateTileset();

        if (bJustAddSomePlacesForMe)
            AddPlaces();

        PlacePlaces();
    }

    void AddPlaces()
    {
        places.Add(new SpecialPlace(Random.Range(0, mapWidth),
                                    Random.Range(0, mapHeight),
                                    0));
        places.Add(new SpecialPlace(Random.Range(0, mapWidth),
                                    Random.Range(0, mapHeight),
                                    0));
        places.Add(new SpecialPlace(Random.Range(0, mapWidth),
                                    Random.Range(0, mapHeight),
                                    0));
        /*places.Add(new SpecialPlace(1,
                                    1,
                                    0));
        places.Add(new SpecialPlace(1,
                                    1,
                                    0));*/
    }

    void PlacePlaces()
    {
        foreach (SpecialPlace place in places)
        {
            GameObject tile = perlinMap.GetTile(place.X, place.Y);
            if (tile == null)
            {
                Debug.LogWarning($"Given location for special place is incorrect. {place.X} | {place.Y} | {tileSet[place.type]}");
                continue;
            }

            GameObject newTile = Instantiate(tileSet[place.type], this.transform);

            newTile.name = string.Format("tile_x({0})_y({1})", place.X, place.Y);
            newTile.transform.localPosition = tile.transform.localPosition;

            perlinMap.DestroyTile(place.X, place.Y);
        }
    }

    void CreateTileset()
    {
        tileSet = new Dictionary<int, GameObject>();
        tileSet.Add(0, castle);
    }
}
