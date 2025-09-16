using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathController : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Unit used for pathfinding
    //------------------------------------------------------------------------------ 

    public GameObject selectedUnit;

    //------------------------------------------------------------------------------
    // Core references
    //------------------------------------------------------------------------------    
    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PathGridHelper gridHelper;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private PathVFX pathVFX;
    [SerializeField] private PathFinder pathFinder;
    
    //------------------------------------------------------------------------------
    // Initialization
    //------------------------------------------------------------------------------ 

    private void Awake()
    {
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
        if (gridHelper == null) gridHelper = FindFirstObjectByType<PathGridHelper>();
        if (unitManager == null) unitManager = FindFirstObjectByType<UnitManager>();
        if (pathVFX == null) pathVFX = FindFirstObjectByType<PathVFX>();

        pathFinder = new PathFinder(gridManager, gridHelper);
    }

    //------------------------------------------------------------------------------
    // Detects a unit
    //------------------------------------------------------------------------------

    public GameObject DetectUnit(Vector3Int position)
    {
        if (!unitManager.unitPositions.TryGetValue(position, out var unit) || unit == null) return null;
        else return unit;
    }

    //------------------------------------------------------------------------------
    // Validates and selects unit
    //------------------------------------------------------------------------------

    public GameObject ValidateUnit(GameObject unit)
    {
        var ui = unit.GetComponent<UnitInstance>();
        if (!ui.isActive) return null;
        selectedUnit = unit;
        return unit;
    }

    //------------------------------------------------------------------------------
    // Tell PathFinder to detect reachable tiles
    //------------------------------------------------------------------------------

    public List<Vector3Int> DetectReachableTiles(UnitMovement unit)
    {
        var reachable = pathFinder.GetReachableTiles(
            gridManager,
            unitManager,
            unit.GetCurrentTile(),
            (MovementType)unit.GetMovementType(),
            (int)unit.unitInstance.currentActionPoints
        );
        return reachable;
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

    public List<Vector3Int> DetectPath(Vector3Int targetTile)
    {
        var um = selectedUnit.GetComponent<UnitMovement>();

        var path = pathFinder.FindPath(
            um.GetCurrentTile(), targetTile,
            um.GetMovementType()
        );
        return path;
    }
    public List<Vector3Int> ValidatePath(List<Vector3Int> path, Vector3Int targetTile)
    {
        if (unitManager.unitPositions.ContainsKey(targetTile))
        {
            path.Remove(targetTile);
            return path;
        }
        

        if (!path.Contains(targetTile))
        {
            var um = selectedUnit.GetComponent<UnitMovement>();
            int budget = (int)um.unitInstance.currentActionPoints;
            int cost = PathHelper.ComputePathCost(gridHelper, path, um.GetMovementType());
            if (cost > budget)
            {
                path = PathHelper.TrimPathToBudget(gridHelper, path, um.GetMovementType(), budget);
            }

        }
        return path;
    }

    public void HighlightPathTiles(List<Vector3Int> path)
    {
        pathVFX.ClearPath();
        pathVFX.HighlightPath(path);
    }

    //------------------------------------------------------------------------------
    // Unit Movement
    //------------------------------------------------------------------------------

    public void MoveUnit(Vector3Int targetTile, List<Vector3Int> path)
    {
        var um = selectedUnit.GetComponent<UnitMovement>();

        um.MoveAlongPath(path);

        var finalTile = path[^1];
        unitManager.unitPositions.Remove(um.GetCurrentTile());
        unitManager.unitPositions[finalTile] = selectedUnit;

        selectedUnit = null;

        pathVFX.ClearHighlights();
        pathVFX.ClearPath();
    }
}
