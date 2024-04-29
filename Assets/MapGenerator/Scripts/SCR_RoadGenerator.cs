using System.Collections.Generic;
using UnityEngine;


public class SCR_RoadGenerator : MonoBehaviour
{
    [SerializeField] GameObject thing;

    public List<List<Tile>> GenerateRoad(List<List<Tile>> dataMapGrid, Tile placeA, Tile placeB, List<TileType> tileTypes)
    {
        PathNode startNode = new PathNode(placeA, placeB, null);
        List<PathNode> openList = new List<PathNode> { startNode };
        List<Tile> closedList = new List<Tile>{ startNode.tile };

        while (openList.Count > 0)
        {
            PathNode node = GetLowestF(openList);
            GameObject temporaryPath = Instantiate(thing, this.transform);
            temporaryPath.transform.position = new Vector3(node.tile.X, node.tile.Y, 0);

            if (node == null)
                return dataMapGrid;

            if(node.Equals(placeB))
            {
                return CreatePath(dataMapGrid, node, tileTypes);
            }

            openList.Remove(node);
            if (node.tile.X + 1 < dataMapGrid.Count)
            {
                Tile tile = dataMapGrid[node.tile.X + 1][node.tile.Y];
                if (!closedList.Contains(tile))
                {
                    openList.Add(new PathNode(tile, placeB, node));
                    closedList.Add(tile);
                }
            }
            if (node.tile.X - 1 >= 0)
            {
                Tile tile = dataMapGrid[node.tile.X - 1][node.tile.Y];
                if (!closedList.Contains(tile))
                {
                    openList.Add(new PathNode(tile, placeB, node));
                    closedList.Add(tile);
                }
            }
            if (node.tile.Y + 1 < dataMapGrid[node.tile.X].Count)
            {
                Tile tile = dataMapGrid[node.tile.X][node.tile.Y + 1];
                if (!closedList.Contains(tile))
                {
                    openList.Add(new PathNode(tile, placeB, node));
                    closedList.Add(tile);
                }
            }
            if (node.tile.Y - 1 >= 0)
            {
                Tile tile = dataMapGrid[node.tile.X][node.tile.Y - 1];
                if (!closedList.Contains(tile))
                {
                    openList.Add(new PathNode(tile, placeB, node));
                    closedList.Add(tile);
                }
            }
            closedList.Add(node.tile);
        }

        return dataMapGrid;
    }

    private List<List<Tile>> CreatePath(List<List<Tile>> dataMapGrid, PathNode node, List<TileType> tileTypes)
    {
        
        return dataMapGrid;
    }

    private List<PathNode> CheckNeighbours(List<List<Tile>> dataMapGrid, List<PathNode> openList, List<Tile> closedList, PathNode node, Tile placeB)
    {
        
        return openList;
    }

    private PathNode GetLowestF(List<PathNode> openList)
    {
        int lowestFValue = int.MaxValue;
        PathNode bestNode = null;

        foreach(PathNode node in openList)
        {
            if(node.fCost < lowestFValue)
            {
                lowestFValue = node.fCost;
                bestNode = node;
            }
        }
        return bestNode;
    }

}

public class PathNode
{
    public Tile tile;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;

    public PathNode(Tile placeA, Tile placeB, PathNode cameFromNode)
    {
        this.tile = placeA;

        Vector2 A = new Vector2(placeA.X, placeA.Y);
        Vector2 B = new Vector2(placeB.X, placeB.Y);

        this.hCost = Mathf.RoundToInt(Vector2.Distance(A, B) * 10);

        this.cameFromNode = cameFromNode;

        if (cameFromNode != null)
        {
            gCost = cameFromNode.gCost + tile.type.weight;
        }
        else
        {
            gCost = tile.type.weight;
        }

        this.fCost = gCost + hCost;
    }

    public bool Equals(Tile tile)
    {
        if (!(this.tile.X == tile.X && this.tile.Y == tile.Y)) return false;
        else return true;
    }
}
