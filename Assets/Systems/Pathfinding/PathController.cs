using System.Collections.Generic;
using System.Globalization;
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
    [SerializeField] private PathGridHelper gridHelper;
    [SerializeField] private PathUnitHelper unitHelper;
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

        pathFinder = new PathFinder(gridManager, gridHelper, unitHelper);
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
        unitManager.selectedUnit = unit;
        return unit;
    }

    //------------------------------------------------------------------------------
    // Tell PathFinder to detect reachable tiles
    //------------------------------------------------------------------------------

    public List<Vector3Int> DetectReachableTiles(UnitMovement unit)
    {
        var reachable = pathFinder.GetReachable(unit);
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
        var um = unitManager.selectedUnit.GetComponent<UnitMovement>();

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
            var um = unitManager.selectedUnit.GetComponent<UnitMovement>();
            int budget = (int)um.unitInstance.currentActionPoints;
            int cost = PathHelper.ComputePathCost(gridHelper, path, um.GetMovementType());
            if (cost > budget)
            {
                path = PathHelper.TrimPathToBudget(gridHelper, path, um.GetMovementType(), budget);
            }
            if (budget <= 0)
            {
                return null;
            }

        }
        return path;
    }

    public void HighlightPathTiles(List<Vector3Int> path)
    {
        pathVFX.ClearPath();
        pathVFX.HighlightPath(path);
    }


    public void ClearTiles()
    {
        pathVFX.ClearPath();
        pathVFX.ClearHighlights();
    }
    //------------------------------------------------------------------------------
    // Unit Movement
    //------------------------------------------------------------------------------

    public void MoveUnit(List<Vector3Int> path)
    {
        if (path == null || path.Count < 2) return;

        var um = unitManager.selectedUnit.GetComponent<UnitMovement>();

        int cost = PathHelper.ComputePathCost(gridHelper, path, um.GetMovementType());

        if (cost > um.unitInstance.currentActionPoints) return;

        um.MoveAlongPath(path);

        var finalTile = path[^1];
        unitManager.unitPositions.Remove(um.GetCurrentTile());
        unitManager.unitPositions[finalTile] = unitManager.selectedUnit;

        unitManager.previouslySelectedUnit = unitManager.selectedUnit;
        unitManager.selectedUnit = null;

        pathVFX.ClearHighlights();
        pathVFX.ClearPath();
    }
}
