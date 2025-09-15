using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public UnitInstance unitInstance;

    [SerializeField] InputManager inputManager;
    [SerializeField] GridManager gridManager;
    [SerializeField] PathGridHelper gridHelper;
    [SerializeField] UnitManager unitManager;

    private PathFinder pathfinder;

    void Start()
    {
        unitInstance = GetComponent<UnitInstance>();
        UpdateUnitTilePosition();
        pathfinder = new PathFinder(gridManager, gridHelper);
        unitManager = FindFirstObjectByType<UnitManager>();
    }

    public Vector3Int GetCurrentTile() => unitInstance.currentTile;
    public MovementType GetMovementType() => (MovementType)unitInstance.unitData.movementType;

    public void MoveAlongPath(List<Vector3Int> path)
    {
        StartCoroutine(FollowPath(path));    
    }

    private IEnumerator FollowPath(List<Vector3Int> path)
    {
        Vector3Int startPos = unitInstance.currentTile;

        foreach (var tilePos in path)
        {
            if (gridHelper.GetMovementCost(tilePos, GetMovementType(), out int cost))
                if (unitInstance.currentActionPoints < cost)
                    yield break;
            Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);

            while ((Vector3.Distance(transform.position, worldPos)) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, worldPos, Time.deltaTime * 5f);
                yield return null;
            }

            unitInstance.currentTile = tilePos;
            unitInstance.currentActionPoints -= cost;
        }
        unitManager.UpdateUnitPositions(gameObject, startPos, unitInstance.currentTile);
    }

    public void MoveTo(Vector3Int targetPosition)
    {
        var path = pathfinder.FindPath(unitInstance.currentTile, targetPosition, (MovementType)unitInstance.unitData.movementType);
        if (path != null)
        {
            StartCoroutine(FollowPath(path));


        }
    }

    void UpdateUnitTilePosition()
    {
        if (tilemap != null)
        {
            unitInstance.currentTile = tilemap.WorldToCell(transform.position);
        }
    }
}
