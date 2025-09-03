using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathfinderAStar
{
    private GridManager grid;

    public PathfinderAStar(GridManager gridManager)
    {
        grid = gridManager;
    }



    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal, Unit.MovementType movementType)
    {
        var openSet = new List<PathNode>();
        var closedSet = new HashSet<Vector3Int>();

        var startNode = new PathNode(start)
        {
            gCost = 0,
            hCost = GetHexDistance(start, goal)
        };

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            PathNode current = openSet[0];
            foreach (var node in openSet)
            {
                if (node.fCost < current.fCost ||
                   (node.fCost == current.fCost && node.hCost < current.hCost))
                {
                    current = node;
                }
            }

            if (current.GridPosition == goal)
            {
                return ReconstructPath(current);
            }

            openSet.Remove(current);
            closedSet.Add(current.GridPosition);

            foreach (var neighborPos in PathfinderHelper.GetNeighbors(current.GridPosition))
            {
                if (closedSet.Contains(neighborPos)) continue;

                if (!grid.GetMovementCost(neighborPos, movementType, out int moveCost))
                    continue;

                int tentativeGCost = current.gCost + moveCost;

                PathNode neighbor = openSet.Find(n => n.GridPosition == neighborPos);
                if (neighbor == null)
                {
                    neighbor = new PathNode(neighborPos);
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = GetHexDistance(neighborPos, goal);
                    neighbor.Parent = current;
                    openSet.Add(neighbor);
                }
                else if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.Parent = current;
                }
            }
        }

        return null;
    }

    private List<Vector3Int> ReconstructPath(PathNode node)
    {
        var path = new List<Vector3Int>();
        while (node != null)
        {
            path.Add(node.GridPosition);
            node = node.Parent;
        }
        path.Reverse();
        if (path.Count > 0)
            path.RemoveAt(0);
        return path;
    }

    private int GetHexDistance(Vector3Int a, Vector3Int b)
    {
        var ac = HexToCube(a);
        var bc = HexToCube(b);

        return Mathf.Max(
            Mathf.Abs(ac.x - bc.x),
            Mathf.Abs(ac.y - bc.y),
            Mathf.Abs(ac.z - bc.z)
        );
    }

    private (int x, int y, int z) HexToCube(Vector3Int hex)
    {
        int q = hex.x;
        int r = hex.y;
        int x = q;
        int z = r;
        int y = -x - z;
        return (x, y, z);
    }




}


/*
    The pathfinder script gets terrain information in this part of the code:
    - GridManager: Provides information about the grid, including terrain types and movement costs. The code is here:
    - PathfinderHelper: Contains utility functions for pathfinding, such as getting neighboring tiles.

    Pathfinding:

    Initialization of path finding algorithm (List<Vector3Int> FindPath). Core logic:
        1. Initialization of:
            a. openSet
            b. closedSet
            c. startNode:
                - gCost = 0
                - hCost = GetHexDistance(start, goal)
        2. Adding startNode to openSet
        3. Loop while openSet is not empty:
            a. Find node with lowest fCost in openSet:
                - Initialization of PathNode current = openSet[0];
                - Loop through openSet to find the node with the lowest fCost:
                    - If node.fCost < current.fCost || (node.fCost == current.fCost && node.hCost < current.hCost)
                        - current = node;
            b. If current.GridPosition == goal:
                - Reconstruct the path from start to goal
                // ReconstructPath logic
            c. If current.GridPosition is not walkable:
                - Remove current from openSet
                - Add current.GridPosition to closedSet
            d. Explore neighbors of current:
                - Loop through each neighbor position:
                    - Foreach var neighborPos in GetNeighbors(current.GridPosition):
                        - Get Neighbors:


*/

















/*    public List<Vector3Int> FindPath(Vector3Int startTile, Vector3Int goalTile, Unit.MovementType movementType)
    {
        var toSearch = new List<PathNode>();
        var searched = new HashSet<Vector3Int>();

        var startNode = new PathNode(startTile)
        {
            gCost = 0,
            hCost = GetHDistance(startTile, goalTile)
        };

        while (toSearch.Count > 0)
        {
            PathNode current = toSearch[0];

            foreach (var tile in toSearch)
            {
                if (tile.fCost < current.fCost || (tile.fCost == current.fCost && tile.hCost < current.hCost))
                {
                    current = tile;
                }
            }

            if (current.gridPosition == goalTile)
            {
                return ReconstructPath(current);
            }

            toSearch.Remove(current);
            searched.Add(current.gridPosition);

            foreach (var neighborPos in PathfinderHelper.GetNeighbors(current.gridPosition))
            {
                if (searched.Contains(neighborPos)) continue;

                if (!grid.GetMovementCost(neighborPos, movementType, out int moveCost))
                    continue;

                int tentativeGCost = current.gCost + moveCost;

                PathNode neighbor = toSearch.Find(n => n.gridPosition == neighborPos);
                if (neighbor == null)
                {
                    neighbor = new PathNode(neighborPos);
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = GetHDistance(neighborPos, goalTile);
                    neighbor.parent = current;
                    toSearch.Add(neighbor);
                }
                else if (tentativeGCost < neighbor.gCost)
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.parent = current;
                }
            }
        }

        return null;
    }

        private List<Vector3Int> ReconstructPath(PathNode node)
        {
            var path = new List<Vector3Int>();
            while (node != null)
            {
                path.Add(node.gridPosition);
                node = node.parent;
            }
            path.Reverse();
            return path;
        }
        
        private int GetHDistance(Vector3Int a, Vector3Int b)
    {
        var ac = HexToCube(a);
        var bc = HexToCube(b);

        return Mathf.Max(
            Mathf.Abs(ac.x - bc.x),
            Mathf.Abs(ac.y - bc.y),
            Mathf.Abs(ac.z - bc.z)
        );
    }

        private (int x, int y, int z) HexToCube(Vector3Int hex)
        {
            int q = hex.x;
            int r = hex.y;
            int x = q;
            int z = r;
            int y = -x - z;
            return (x, y, z);
        }
            


        }*/

        /*
                To search for node that has the lowest fCost I should:

                    1. Initialize a variable to keep track of the lowest fCost found
                    2. Iterate through the toSearch list
                    3. For each node, calculate its fCost
                    4. If this fCost is lower than the lowest fCost found, update the lowest fCost and the current node

                    So in code it would look like this:

                    float lowestFCost = float.MaxValue;
                    PathNode currentNode = null;

                    foreach (var tile in toSearch)
                    {
                        float fCost = CalculateFCost(tile, goalTile);
                        if (fCost < lowestFCost)
                        {
                            lowestFCost = fCost;
                            currentNode = new PathNode(tile);
                        }
                    }

                    And CalculateFCost would look like this:
                        float CalculateFCost(Vector3Int from, Vector3Int to)
                        {
                            float gCost = GetDistance(from, to);
                            float hCost = GetHeuristicDistance(from, to);
                            return gCost + hCost;
                        }
            */

    /* 
        HashSet can be explained as:
        A HashSet is a collection that contains no duplicate elements, and it is based on the hash table data structure. It is used to store unique items and provides fast lookups, additions, and removals.
        Main difference between HashSet and List is that HashSet does not allow duplicate values, while List does.


        For the most basic A* code structure we can use this code as a starting point:

        public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal, Unit.MovementType movementType)
        {
            var openSet = new List<PathNode>();
            var closedSet = new HashSet<Vector3Int>();

            var startNode = new PathNode(start)
            {
                gCost = 0,
                hCost = GetHexDistance(start, goal)
            };

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                // Pick node with lowest fCost
                PathNode current = openSet[0];
                foreach (var node in openSet)
                {
                    if (node.fCost < current.fCost ||
                       (node.fCost == current.fCost && node.hCost < current.hCost))
                    {
                        current = node;
                    }
                }
            }
       Firstly, we need to implement the A* pathfinding algorithm itself. Algorithm contains:
           - Cost calculation (gCost, hCost, fCost)
               - gCost: Distance from start to this node
               - hCost: Distance from this node to the goal
               - fCost: Total cost (gCost + hCost)
               - openSet and closedSet management
                   - OpenSet: Nodes to be evaluated
                   - ClosedSet: Nodes already evaluated
               Firstly the algorithm checks which node has the lowest fCost in the openSet. If 2 or more nodes have
               the same fCost, it breaks ties using the hCost, if the hCost is also the same then it explores its neighbors, updating their costs and parents as necessary.

           So the full algorithm can be summarized as follows:
               1. Initialize the openSet with the start node.
               2. While the openSet is not empty:
                   a. Find the node with the lowest fCost in the openSet
                   b. If this node is the goal, reconstruct the path and return it
                   c. Move the current node from the openSet to the closedSet
                   d. Explore each neighbor of the current node
                       i. If the neighbor is in the closedSet, skip it
                       ii. Calculate the tentative gCost for the neighbor
                       iii. If the neighbor is not in the openSet, add it
                       iv. If the tentative gCost is lower than the neighbor's gCost, update it

           Additional functionality:
               - Support for different movement types
               - Cost calculation based on movement type
               - Handling obstacles (terrain or unit on the tile)

           Support for different movement types:
               - Implement different movement costs for various terrain types (e.g., grass, water, mountains)
               - Allow units to have different movement capabilities (e.g., flying units can move over obstacles)
               Code:


           - Path reconstruction
               - Reconstruct the path from the goal node to the start node using the parent references

    */
































 