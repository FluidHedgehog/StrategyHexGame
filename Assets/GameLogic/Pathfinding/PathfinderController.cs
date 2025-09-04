using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfinderController : MonoBehaviour
{
    private PathfinderInitializer gridManager;
    private InputManager inputManager;

    private PathfinderAStar pathfinder;
    private PathfinderVFX pathfinderVFX;

    //private HashSet<Vector3Int> _reachable;

    [SerializeField] GameObject selectedUnit;

    private void Awake()
    {
        gridManager = FindFirstObjectByType<PathfinderInitializer>();
        pathfinderVFX = FindFirstObjectByType<PathfinderVFX>();
        inputManager = FindFirstObjectByType<InputManager>();
        pathfinder = new PathfinderAStar(gridManager);
    }

    private void Update()
    {
        if (selectedUnit != null)
        {
            var path = DetectPath(inputManager.gridPosition);
            if (path != null && path.Count > 0)
            {
                pathfinderVFX.ClearPath();
                pathfinderVFX.HighlightPath(path);
            }

        }
        else return;
    }

    public void Controller(Vector3Int gridPosition)
    {
        if (selectedUnit == null)
        {
            DetectUnit(gridPosition);
        }
        else
        {
            MoveUnit(gridPosition);
        }
    }

    public GameObject DetectUnit(Vector3Int position)
    {
        gridManager.unitPositions.TryGetValue(position, out var unit);
        if (unit == null) return null;

        selectedUnit = unit;

        var reachable = DetectReachableTiles(unit.GetComponent<UnitMovement>());
        //_reachable = new HashSet<Vector3Int>(reachable);

        pathfinderVFX.ClearHighlights();
        pathfinderVFX.HighlightAvailableTiles(reachable);

        return unit;
    }

    public List<Vector3Int> DetectReachableTiles(UnitMovement unit)
    {
        var reachable = PathfinderHelper.GetReachableTiles(
            gridManager, 
            unit.GetCurrentTile(),
            unit.GetMovementType(),
            (int)unit.unitObject.currentActionPoints
        );
        return reachable;
    }

    public List<Vector3Int> DetectPath(Vector3Int targetTile)
    {
        var path = pathfinder.FindPath(
            selectedUnit.GetComponent<UnitMovement>().GetCurrentTile(), targetTile,
            selectedUnit.GetComponent<UnitMovement>().GetMovementType()
        );
        if (path == null) return null;
        else return path;
    }

    public void MoveUnit(Vector3Int targetTile)
    {
        var um = selectedUnit.GetComponent<UnitMovement>();
        var path = DetectPath(targetTile);
        if (path == null) return;

        int budget = (int)um.unitObject.currentActionPoints;
        var cost = PathfinderHelper.ComputePathCost(gridManager, path, um.GetMovementType());

        if (cost > budget)
        {
            path = PathfinderHelper.TrimPathToBudget(gridManager, path, um.GetMovementType(), budget);
        }

        pathfinderVFX.ClearHighlights();
        pathfinderVFX.HighlightPath(path);

        um.MoveAlongPath(path);

        var finalTile = path[^1];
        gridManager.unitPositions.Remove(um.GetCurrentTile());
        gridManager.unitPositions[finalTile] = selectedUnit;

        selectedUnit = null;
        pathfinderVFX.ClearHighlights();
    }
}
