using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Utils;

public class PathFinder
{
    // Plain C# service. Dependencies are injected via constructor.
    //private readonly GridManager grid;
    private readonly PathGridHelper pathGridHelper;
    private readonly PathUnitHelper pathUnitHelper;

    public PathFinder(GridManager gridManager, PathGridHelper pathGridHelper, PathUnitHelper pathUnitHelper)
    {
        //this.grid = gridManager;
        this.pathGridHelper = pathGridHelper;
        this.pathUnitHelper = pathUnitHelper;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal, MovementType movementType)
    {
        var startNode = new PathNode(start) //Initializing start position
        {
            gCost = 0,
            hCost = PathHelper.GetHexDistance(start, goal) //Get distance from start to goal in hex (smallest possible value)
        };

        var openSet = new PriorityQueue<PathNode, int>(); //Nodes to be evaluated
        var openSetDict = new Dictionary<Vector3Int, PathNode>(); //For quick lookup

        openSet.Enqueue(startNode, startNode.fCost);
        openSetDict[startNode.GridPosition] = startNode;

        var closedSet = new List<Vector3Int>(); //Nodes checked

        while (openSet.Count > 0) //Loop until no more nodes
        {
            PathNode currentNode = openSet.Dequeue(); //Assign first node as current
            openSetDict.Remove(currentNode.GridPosition);

            if (currentNode.GridPosition == goal) //Check if the node reached goal
            {
                return ReconstructPath(currentNode); //Terminate loop and reconstruct path
            }

            closedSet.Add(currentNode.GridPosition); //Add to closed set

            foreach (var neighborPos in PathHelper.GetNeighbors(currentNode.GridPosition)) //Get all neighboring nodes 
            {
                if (closedSet.Contains(neighborPos)) continue; //Skip if already evaluated

                if (!pathGridHelper.GetMovementCost(neighborPos, movementType, out int cost)) continue; //Skip if no valid movement cost

                int tentativeGCost = currentNode.gCost + cost; //Calculate gCost

                if (openSetDict.TryGetValue(neighborPos, out PathNode neighbor))
                {
                    if (tentativeGCost < neighbor.gCost) //If a better path is found
                    {
                        neighbor.gCost = tentativeGCost; //Set gCost
                        neighbor.Parent = currentNode; //Set parent

                        openSet.Enqueue(neighbor, neighbor.fCost); //Reinsert to update priority
                    }
                }
                else //If a better path is found
                {
                    neighbor = new PathNode(neighborPos); //Create a new node
                    neighbor.gCost = tentativeGCost; //Set gCost
                    neighbor.hCost = PathHelper.GetHexDistance(neighborPos, goal); //Set hCost
                    neighbor.Parent = currentNode; //Set parent

                    openSet.Enqueue(neighbor, neighbor.fCost); //Add to open set
                    openSetDict[neighborPos] = neighbor;
                }
            }
        }
        Debug.Log("No valid path found");
        return null; //No valid path found
    }

    private List<Vector3Int> ReconstructPath(PathNode node) //Reconstruct the path from the end node to the start
    {
        var path = new List<Vector3Int>(); //Create a new list to hold the path
        while (node != null) //While there is a valid node
        {
            path.Add(node.GridPosition); //Add the current node's position to the path
            node = node.Parent; //Move to the parent node
        }
        path.Reverse(); //Reverse the path to get the correct order
        if (path.Count > 0) //If the path is not empty
            path.RemoveAt(0); //Remove the first node (start node)

        return path; //Return the final path
    }








    public List<Vector3Int> GetReachable(UnitMovement unit)
    {
        var reachable = new List<Vector3Int>();
        var frontier = new Queue<Vector3Int>();
        var costSoFar = new Dictionary<Vector3Int, int>();

        var start = unit.GetCurrentTile();
        var moveType = unit.GetMovementType();
        var maxCost = unit.GetActionPoints();



        frontier.Enqueue(start);
        costSoFar[start] = 0;






        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            foreach (var tile in PathHelper.GetNeighbors(current))
            {
                if (pathUnitHelper.DoesTileHaveUnit(tile)) continue;
                if (!pathGridHelper.GetMovementCost(tile, moveType, out int moveCost)) continue;

                int newCost = costSoFar[current] + moveCost;

                if (newCost <= maxCost && (!costSoFar.ContainsKey(tile) || newCost < costSoFar[tile]))
                {
                    costSoFar[tile] = newCost;
                    frontier.Enqueue(tile);
                    reachable.Add(tile);
                }
            }
        }

        return reachable;
    }
}