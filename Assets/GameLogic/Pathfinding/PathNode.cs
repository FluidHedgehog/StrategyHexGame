using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public Vector3Int GridPosition;

    public int gCost { get; set; }
    public int hCost { get; set; }
    public int fCost => gCost + hCost;

    public PathNode Parent { get; set; }

    public PathNode(Vector3Int gridPosition)
    {
        GridPosition = gridPosition;
    }
}