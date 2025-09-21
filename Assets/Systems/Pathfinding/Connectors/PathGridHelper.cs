using UnityEngine;

public class PathGridHelper : MonoBehaviour
{
    [SerializeField] private GridManager grid;
    [SerializeField] private UnitManager unitManager;

    public void Awake()
    {
        // Prefer serialized refs; otherwise try to find on the same GO, then globally
        if (grid == null) grid = GetComponent<GridManager>();
        if (grid == null) grid = FindFirstObjectByType<GridManager>();

        if (unitManager == null) unitManager = GetComponent<UnitManager>();
        if (unitManager == null) unitManager = FindFirstObjectByType<UnitManager>();
    }

    public TileData GetTileData(Vector3Int gridPosition)
    {
        var tile = grid.tilemap.GetTile(gridPosition);
        if (tile != null && grid.dataFromTerrain.ContainsKey(tile))
        {
            return grid.dataFromTerrain[tile];
        }
        return null;
    }

    public bool IsWalkable(Vector3Int gridPosition)
    {
        var tileData = GetTileData(gridPosition);
        return tileData != null && tileData.walkable;
    }

    public bool GetMovementCost(Vector3Int gridPosition, MovementType movementType, out int cost)
    {
        var tileData = GetTileData(gridPosition);
        if (tileData == null)
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
}

