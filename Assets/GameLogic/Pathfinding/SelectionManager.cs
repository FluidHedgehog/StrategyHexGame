using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Tilemaps;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] GridManager gridManager;
    [SerializeField] Tilemap highlightMap;
    [SerializeField] TileBase reachableTileHighlight;
    [SerializeField] TileBase pathTileHighlight;

    public GameObject selectedUnit;
    private PathfinderAStar pathfinder;

    void Start()
    {
        pathfinder = new PathfinderAStar(gridManager);
    }

    public GameObject GetUnitAt(Vector3Int gridPos)
    {
        gridManager.unitPositions.TryGetValue(gridPos, out var unit);

        if (unit != null)
        {
            SelectUnit(unit);
            return unit;
        }

        return null;
    }

    void SelectUnit(GameObject unit)
    {
        selectedUnit = unit;
        //HighlightReachableTiles(unit.GetComponent<UnitMovement>());
    }

    public void TryMoveUnit(Vector3Int targetTile)
    {
        var path = pathfinder.FindPath(
            selectedUnit.GetComponent<UnitMovement>().GetCurrentTile(),
            targetTile,
            selectedUnit.GetComponent<UnitMovement>().GetMovementType()
        );

        if (path == null) return;

        highlightMap.ClearAllTiles();

        foreach (var step in path)
        {
            highlightMap.SetTile(step, pathTileHighlight);
        }
        selectedUnit.GetComponent<UnitMovement>().MoveAlongPath(path);
        highlightMap.ClearAllTiles();
    }

    void HighlightReachableTiles(UnitMovement unit)
    {
        highlightMap.ClearAllTiles();

        var reachable = PathfinderHelper.GetReachableTiles(
            gridManager, 
            unit.GetCurrentTile(),
            unit.GetMovementType(),
            (int)unit.unitObject.currentActionPoints
        );

        foreach (var tile in reachable)
        {
            highlightMap.SetTile(tile, reachableTileHighlight);
        }
    }
}