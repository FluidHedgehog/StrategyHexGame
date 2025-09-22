using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Utils;

public class Pathfinding
{
    //------------------------------------------------------------------------------
    // class Initialization
    //------------------------------------------------------------------------------

    private readonly GridManager manager;

    public Pathfinding(GridManager gridManager)
    {
        manager = gridManager;
    }

    //------------------------------------------------------------------------------
    // A* Algorythm
    //------------------------------------------------------------------------------

    public (Queue<Vector3Int>, int pathCost) AStar(GameObject unit, Vector3Int goal)
    {
        var um = unit.GetComponent<UnitMovement>();
        var moveType = um.GetMovementType();
        var start = um.GetCurrentTile();
        var budget = um.GetActionPoints();

        var nextTileToGoal = new Dictionary<Vector3Int, Vector3Int>();
        var costToReachTile = new Dictionary<Vector3Int, int>();
        var frontier = new PriorityQueue<Vector3Int, int>();

        costToReachTile[goal] = 0;
        frontier.Enqueue(goal, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!GetMovementCost(start, neighbor, moveType, out int cost)) continue;
                
                    var newCost = costToReachTile[current] + cost;

                if (newCost > budget) continue;

                if (!costToReachTile.ContainsKey(neighbor) || newCost < costToReachTile[neighbor])
                {
                    costToReachTile[neighbor] = newCost;
                    int priority = newCost + GetHexDistance(neighbor, start);
                    frontier.Enqueue(neighbor, priority);
                    nextTileToGoal[neighbor] = current;

                }
            }
        }



        Queue<Vector3Int> path = new Queue<Vector3Int>();
        Vector3Int currentPathTile = start;
        int pathCost = 0;

        while (currentPathTile != goal)
        {
            Vector3Int newStep = nextTileToGoal[currentPathTile];
            if (GetMovementCost(start, newStep, moveType, out int stepCost)) pathCost += stepCost;
            currentPathTile = newStep;
            path.Enqueue(currentPathTile);
        }
        if (!nextTileToGoal.ContainsKey(start)) return (path, pathCost);
        return (path, pathCost);
    }

    //------------------------------------------------------------------------------
    // Dijkstra Algorythm
    //------------------------------------------------------------------------------

    public Dictionary<Vector3Int, Vector3Int> Dijkstra(GameObject unit)
    {
        var um = unit.GetComponent<UnitMovement>();
        var moveType = um.GetMovementType();
        var start = um.GetCurrentTile();
        var budget = um.GetActionPoints();

        var nextTileToGoal = new Dictionary<Vector3Int, Vector3Int>();
        var costToReachTile = new Dictionary<Vector3Int, int>();
        var frontier = new PriorityQueue<Vector3Int, int>();

        costToReachTile[start] = 0;
        frontier.Enqueue(start, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!GetMovementCost(start, neighbor, moveType, out int cost)) continue;
                var newCost = costToReachTile[current] + cost;

                if (newCost > budget) continue;

                if (!costToReachTile.ContainsKey(neighbor) || newCost < costToReachTile[neighbor])
                {
                    costToReachTile[neighbor] = newCost;
                    int priority = newCost;
                    frontier.Enqueue(neighbor, priority);
                    nextTileToGoal[neighbor] = current;

                }
            }
        }

        if (!nextTileToGoal.ContainsKey(start)) return nextTileToGoal;

        return nextTileToGoal;
    }

    public List<Vector3Int> ReturnPathDijkstra(Vector3Int start, Vector3Int goal, Dictionary<Vector3Int, Vector3Int> tilesToGoal)
    {

        var path = new List<Vector3Int>();
        if (!tilesToGoal.ContainsKey(goal)) return path;

        var current = goal;

        while (current != start)
        {
            path.Add(current);
            current = tilesToGoal[current];
        }
        path.Reverse();

        return path;
    }

    //------------------------------------------------------------------------------
    // Additional Core Help Functions
    //------------------------------------------------------------------------------


    public List<Vector3Int> GetNeighbors(Vector3Int pos)
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

    public TileData GetTileData(Vector3Int gridPosition)
    {
        var tile = manager.tilemap.GetTile(gridPosition);
        if (tile != null && manager.dataFromTerrain.ContainsKey(tile))
        {
            return manager.dataFromTerrain[tile];
        }
        return null;
    }

    public bool GetMovementCost(Vector3Int unitPosition, Vector3Int gridPosition, MovementType movementType, out int cost)
    {
        var tileData = GetTileData(gridPosition);
        if (tileData == null)
        {
            cost = int.MaxValue;
            return false;
        }
        if (DetectUnit(gridPosition, unitPosition))
        {
            cost = int.MaxValue;
            return false;
        }

        switch (movementType)
        {
            case MovementType.Walk:
                cost = tileData.walkCost;
                return tileData.walkable;
            case MovementType.Swim:
                cost = tileData.swimCost;
                return tileData.swimable;
            case MovementType.Fly:
                cost = tileData.flyCost;
                return tileData.flyable;
            default:
                cost = int.MaxValue;
                return false;
        }
    }

    public static int GetHexDistance(Vector3Int start, Vector3Int goal)
    {
        int dx = goal.x - start.x;
        int dy = goal.y - start.y;
        int dz = -(dx + dy);

        return (Mathf.Abs(dx) + Mathf.Abs(dy) + Mathf.Abs(dz)) / 2;
    }
    
    public bool DetectUnit(Vector3Int checkedPosition, Vector3Int unitPosition)
    {

        if (checkedPosition == unitPosition) return false;
        return manager.unitPositions.ContainsKey(checkedPosition);
    }
}
