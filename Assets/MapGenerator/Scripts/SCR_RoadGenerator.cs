using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class SCR_RoadGenerator
{
    public static PathNode FindBestNode(List<List<Tile>> dataMapGrid, Tile placeA, Tile placeB)
    {
        PathNode node = new PathNode(placeA, placeB, null);
        List<PathNode> openList = new List<PathNode> { node };
        List<Tile> closedList = new List<Tile>{ node.tile };

        while (openList.Count > 0)
        {
            node = GetLowestF(openList);

            if (node == null)
                return null;

            if(node.Equals(placeB))
                return node;

            openList.Remove(node);

            // save neighbour nodes
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

        return null;
    }

    private static PathNode GetLowestF(List<PathNode> openList)
    {
        return openList.OrderBy(node => node.fCost).FirstOrDefault();
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
