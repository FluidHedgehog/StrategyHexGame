using System.Collections.Generic;
using UnityEngine;

public static class PathfinderHelper
{
    public static int ComputePathCost(GridManager grid, List<Vector3Int> path, Unit.MovementType moveType)
    {
        if (path == null || path.Count < 2) return 0;
        int cost = 0;
        for (int i = 1; i < path.Count; i++)
        {
            if (!grid.GetMovementCost(path[i], moveType, out int moveCost)) return int.MaxValue;
            cost += moveCost;

        }
        return cost;
    }

    public static List<Vector3Int> TrimPathToBudget(GridManager grid, List<Vector3Int> path, Unit.MovementType moveType, int budget)
    {
        if (path == null || path.Count == 0) return path;

        var trimmedPath = new List<Vector3Int> { path[0] };
        int cost = 0;
        for (int i = 1; i < path.Count; i++)
        {
            if (!grid.GetMovementCost(path[i], moveType, out int moveCost)) break;
            if (cost + moveCost > budget) break;
            cost += moveCost;
            trimmedPath.Add(path[i]);
        }

        return trimmedPath;
    }

    public static List<Vector3Int> GetReachableTiles(GridManager grid, Vector3Int start, Unit.MovementType moveType, int maxCost)
    {
        var reachable = new List<Vector3Int>();
        var frontier = new Queue<Vector3Int>();
        var costSoFar = new Dictionary<Vector3Int, int>();

        frontier.Enqueue(start);
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!grid.GetMovementCost(neighbor, moveType, out int moveCost))
                    continue;

                int newCost = costSoFar[current] + moveCost;
                if (newCost <= maxCost && (!costSoFar.ContainsKey(neighbor) || newCost < costSoFar[neighbor]))
                {
                    costSoFar[neighbor] = newCost;
                    frontier.Enqueue(neighbor);
                    reachable.Add(neighbor);
                }
            }
        }

        return reachable;
    }

    /*public static List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        return new List<Vector3Int>
        {
         //   new Vector3Int(pos.x + 1, pos.y, 0),
        //    new Vector3Int(pos.x - 1, pos.y, 0),
        //    new Vector3Int(pos.x,     pos.y + 1, 0),
        //    new Vector3Int(pos.x,     pos.y - 1, 0),
        //    new Vector3Int(pos.x + 1, pos.y - 1, 0),
        //    new Vector3Int(pos.x - 1, pos.y + 1, 0),
        //    new Vector3Int(pos.x + 1, pos.y + 1, 0),
            new Vector3Int(pos.x - 1, pos.y - 1, 0)
        };
    }

    private static readonly Vector2Int[] axialDirections = new[]
    {
        new Vector2Int(+1,  0),
        new Vector2Int(+1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int(-1,  0),
        new Vector2Int(-1, +1),
        new Vector2Int( 0, +1),
    };

    public static IEnumerable<Vector3Int> GetNeighbors(Vector3Int axialPos)
    {
        foreach (var dir in axialDirections)
        {
            yield return new Vector3Int(axialPos.x + dir.x, axialPos.y + dir.y, 0);
        }
    }*/

    private static readonly Vector3Int[] EvenRowNeighbors = new[]
    {
    new Vector3Int(+1,  0, 0),
    new Vector3Int( 0, +1, 0),
    new Vector3Int(-1, +1, 0),
    new Vector3Int(-1,  0, 0),
    new Vector3Int(-1, -1, 0),
    new Vector3Int( 0, -1, 0)
    };


    private static readonly Vector3Int[] OddRowNeighbors = new[]
    {
    new Vector3Int(+1,  0, 0),
    new Vector3Int(+1, +1, 0),
    new Vector3Int( 0, +1, 0),
    new Vector3Int(-1,  0, 0),
    new Vector3Int( 0, -1, 0),
    new Vector3Int(+1, -1, 0)
    };


    public static List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        bool isOddRow = (pos.y & 1) == 1;
        var offsets = isOddRow ? OddRowNeighbors : EvenRowNeighbors;

        var neighbors = new List<Vector3Int>();
        foreach (var offset in offsets)
            neighbors.Add(pos + offset);

        return neighbors;
    }





}