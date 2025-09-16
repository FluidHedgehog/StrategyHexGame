using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    public GameObject selectedUnit;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private PathGridHelper gridHelper;
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private PathVFX pathVFX;

    private PathFinder pathFinder; // initialized in Awake

    private void Awake()
    {
        // Validate serialized deps
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
        if (gridHelper == null) gridHelper = FindFirstObjectByType<PathGridHelper>();
        if (unitManager == null) unitManager = FindFirstObjectByType<UnitManager>();
        if (pathVFX == null) pathVFX = FindFirstObjectByType<PathVFX>();

        // Construct the service
        pathFinder = new PathFinder(gridManager, gridHelper);
    }

    //------------------------------------------------------------------------------
    // Unit Detection
    //------------------------------------------------------------------------------

    public GameObject DetectUnit(Vector3Int position)
    {
        if (unitManager == null) { Debug.LogError("UnitManager is not set on PathController"); return null; }
        if (!unitManager.unitPositions.TryGetValue(position, out var unit) || unit == null) return null;

        var um = unit.GetComponent<UnitMovement>();
        var ui = unit.GetComponent<UnitInstance>();
        if (um == null) { Debug.LogWarning("Clicked unit has no UnitMovement component"); return null; }
        if (!ui.isActive) return null;
        selectedUnit = unit;

        var reachable = DetectReachableTiles(um);

        if (pathVFX != null)
        {
            pathVFX.ClearHighlights();
            pathVFX.HighlightAvailableTiles(reachable);
        }

        return unit;
    }

    //------------------------------------------------------------------------------
    // Reachable Tile Detection
    //------------------------------------------------------------------------------

    public List<Vector3Int> DetectReachableTiles(UnitMovement unit)
    {
        if (pathFinder == null || gridManager == null || unitManager == null || unit == null || unit.unitInstance == null)
        {
            Debug.LogError("DetectReachableTiles missing dependency or unit/unitObject");
            return new List<Vector3Int>();
        }

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
    // Path Detection
    //------------------------------------------------------------------------------

    public List<Vector3Int> DetectPath(Vector3Int targetTile)
    {
        if (selectedUnit == null) return null;
        var um = selectedUnit.GetComponent<UnitMovement>();
        if (um == null) return null;
        if (pathFinder == null) return null;

        var path = pathFinder.FindPath(
            um.GetCurrentTile(), targetTile,
            um.GetMovementType()
        );
        return path;
    }

    //------------------------------------------------------------------------------
    // Unit Movement
    //------------------------------------------------------------------------------

    public void MoveUnit(Vector3Int targetTile)
    {
        if (selectedUnit == null) return;
        var um = selectedUnit.GetComponent<UnitMovement>();
        if (um == null || um.unitInstance == null) return;

        var path = DetectPath(targetTile);
        if (path == null || path.Count == 0) return;

        int budget = (int)um.unitInstance.currentActionPoints;
        var cost = PathHelper.ComputePathCost(gridHelper, path, um.GetMovementType());

        if (cost > budget)
        {
            path = PathHelper.TrimPathToBudget(gridHelper, path, um.GetMovementType(), budget);
        }

        if (pathVFX != null)
        {
            pathVFX.ClearHighlights();
            pathVFX.HighlightPath(path);
        }

        um.MoveAlongPath(path);

        var finalTile = path[^1];
        unitManager.unitPositions.Remove(um.GetCurrentTile());
        unitManager.unitPositions[finalTile] = selectedUnit;

        selectedUnit = null;
        if (pathVFX != null) pathVFX.ClearHighlights();
        pathVFX.ClearPath();
    }
}
