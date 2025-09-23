using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PathController : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Unit used for pathfinding
    //------------------------------------------------------------------------------ 

    //------------------------------------------------------------------------------
    // Core references
    //------------------------------------------------------------------------------    
    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private PathVFX pathVFX;
    [SerializeField] private Pathfinding pathfinding;
    [SerializeField] private TaskManager taskManager;
    
    //------------------------------------------------------------------------------
    // Initialization
    //------------------------------------------------------------------------------ 

    private void Awake()
    {
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
        if (unitManager == null) unitManager = FindFirstObjectByType<UnitManager>();
        if (pathVFX == null) pathVFX = FindFirstObjectByType<PathVFX>();
        if (taskManager == null) taskManager = FindFirstObjectByType<TaskManager>();

        pathfinding = new Pathfinding(gridManager, taskManager);
    }

    //------------------------------------------------------------------------------
    // Detects a unit
    //------------------------------------------------------------------------------

    public GameObject DetectUnit(Vector3Int position)
    {
        if (!gridManager.unitPositions.TryGetValue(position, out var unit) || unit == null) return null;
        else return unit;
    }

    //------------------------------------------------------------------------------
    // Validates and selects unit
    //------------------------------------------------------------------------------

    public GameObject ValidateUnit(GameObject unit)
    {
        var ui = unit.GetComponent<UnitInstance>();
        if (!ui.isActive) return null;
        unitManager.selectedUnit = unit;
        return unit;
    }

    //------------------------------------------------------------------------------
    // Tell PathFinder to detect reachable tiles
    //------------------------------------------------------------------------------

    public List<Vector3Int> DetectReachableTiles(GameObject unit)
    {
        var reachable = pathfinding.Dijkstra(unit);
    
        return new List<Vector3Int>(reachable.Keys);
    }

    //------------------------------------------------------------------------------
    // Tell PathVFX to highlight reachable tiles
    //------------------------------------------------------------------------------
    
    public void HighlightReachableTiles(List<Vector3Int> reachable)
    {
        pathVFX.ClearHighlights();
        pathVFX.HighlightAvailableTiles(reachable);
    }

    //--------------------------------------------------------------------------------------------------------------------------------

    //------------------------------------------------------------------------------
    // Path Detection
    //------------------------------------------------------------------------------

    public (Queue<Vector3Int>, int) DetectPath(Vector3Int targetTile)
    {
        var (path, pathCost) = pathfinding.AStar(unitManager.selectedUnit, targetTile);
        return (path, pathCost);
    }

    public void HighlightPathTiles(Queue<Vector3Int> path)
    {
        var pathNew = path.ToList();
        pathVFX.ClearPath();
        pathVFX.HighlightPath(pathNew);
    }

    public void ClearTiles()
    {
        pathVFX.ClearPath();
        pathVFX.ClearHighlights();
    }
    //------------------------------------------------------------------------------
    // Unit Movement
    //------------------------------------------------------------------------------

    public void MoveUnit(Queue<Vector3Int> path, int pathCost)
    {
        var um = unitManager.selectedUnit.GetComponent<UnitMovement>();

        gridManager.unitPositions.Remove(um.GetCurrentTile());

        um.StartCoroutine(um.FollowPath(path, pathCost));

        

        unitManager.previouslySelectedUnit = unitManager.selectedUnit;
        unitManager.selectedUnit = null;

        pathVFX.ClearHighlights();
        pathVFX.ClearPath();
    }
}
