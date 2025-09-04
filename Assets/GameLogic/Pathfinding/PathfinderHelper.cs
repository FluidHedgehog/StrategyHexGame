using System.Collections.Generic;
using UnityEngine;

public static class PathfinderHelper
{
    public static int ComputePathCost(PathfinderInitializer grid, List<Vector3Int> path, Unit.MovementType moveType)
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

    public static List<Vector3Int> TrimPathToBudget(PathfinderInitializer grid, List<Vector3Int> path, Unit.MovementType moveType, int budget)
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

    public static bool DoesTileHaveUnit(PathfinderInitializer grid, Vector3Int position)
    {
        return grid.unitPositions.ContainsKey(position);
    }

    public static List<Vector3Int> GetReachableTiles(PathfinderInitializer grid, Vector3Int start, Unit.MovementType moveType, int maxCost)
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

                if (DoesTileHaveUnit(grid, neighbor))
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

    public static int GetHexDistance(Vector3Int start, Vector3Int goal)
    {
        int dx = goal.x - start.x;
        int dy = goal.y - start.y;
        int dz = -(dx + dy);

        return (Mathf.Abs(dx) + Mathf.Abs(dy) + Mathf.Abs(dz)) / 2;
    }

    public static int CalculateMovementCost(PathfinderInitializer grid, Vector3Int from, Vector3Int to, Unit.MovementType moveType)
    {
        if (!grid.GetMovementCost(to, moveType, out int moveCost))
            return int.MaxValue; // Impassable

        return moveCost;
    }

    public static List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        bool isOddRow = (pos.y & 1) == 1;
        var offsets = isOddRow ? OddRowNeighbors : EvenRowNeighbors;

        var neighbors = new List<Vector3Int>();
        foreach (var offset in offsets)
            neighbors.Add(pos + offset);

        return neighbors;
    }

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

}