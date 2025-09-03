using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utils;

public class PathfinderAStar
{
    private PathfinderInitializer grid;

    public PathfinderAStar(PathfinderInitializer gridManager)
    {
        grid = gridManager;
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int goal, Unit.MovementType movementType)
    {
        var startNode = new PathNode(start) //Initializing start position
        {
            gCost = 0,
            hCost = PathfinderHelper.GetHexDistance(start, goal) //Get distance from start to goal in hex (smallest possible value)
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
            /*foreach (var node in openSet) //Check each node
            {
                if (node.fCost < currentNode.fCost || (node.fCost == currentNode.fCost && node.hCost < currentNode.hCost)) //Search for lower value
                {
                    currentNode = node; //Assign better cost node
                }
            }*/

            if (currentNode.GridPosition == goal) //Check if the node reached goal
            {
                return ReconstructPath(currentNode); //Terminate loop and reconstruct path
            }

            //openSet.Dequeue(currentNode, currentNode.fCost); //Removed for performance reasons
            closedSet.Add(currentNode.GridPosition); //Add to closed set

            foreach (var neighborPos in PathfinderHelper.GetNeighbors(currentNode.GridPosition)) //Get all neighboring nodes 
            {
                if (closedSet.Contains(neighborPos)) continue; //Skip if already evaluated

                if (!grid.GetMovementCost(neighborPos, movementType, out int cost)) continue; //Skip if no valid movement cost

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
                    neighbor.hCost = PathfinderHelper.GetHexDistance(neighborPos, goal); //Set hCost
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

    // To implement priority queue for openSet i should replace the List with a PriorityQueue, but it does not work since PriorityQueue is not available in all versions of Unity. 
}
