using System.Collections.Generic;
using UnityEngine;
using Utils;

public class Pathfinding : MonoBehaviour
{
    public GridManager gridManager;
    public PathGridHelper pathGridHelper;

    public Dictionary<Vector3Int, int> Dijkstra(GameObject unit)
    {
        var unitMovement = unit.GetComponent<UnitMovement>();
        var tile = unitMovement.GetCurrentTile();
        var moveType = unitMovement.GetMovementType();

        var costs = new Dictionary<Vector3Int, int>();

        var priority = new PriorityQueue<Vector3Int, int>();

        var visited = new HashSet<Vector3Int>();

        costs[tile] = 0;
        priority.Enqueue(tile, 0);

        while (priority.Count > 0)
        {
            var current = priority.Dequeue();
            if (visited.Contains(current)) continue;

            foreach (var neighbor in PathHelper.GetNeighbors(current))
            {
                pathGridHelper.GetMovementCost(neighbor, moveType, out int cost);
                int newCost = costs[current] + cost;

                if (newCost < costs[neighbor])
                {
                    costs[neighbor] = newCost;
                    priority.Enqueue(neighbor, newCost);
                }

                visited.Add(current);
            }
        }

        return costs;
    }

}
