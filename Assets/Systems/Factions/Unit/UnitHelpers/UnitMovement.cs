using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public UnitInstance unitInstance;

    [SerializeField] InputManager inputManager;
    [SerializeField] GridManager gridManager;
    [SerializeField] UnitManager unitManager;
    [SerializeField] float speedFactor;
    private Pathfinding pathfinding;

    void Start()
    {
        unitInstance = GetComponent<UnitInstance>();
        UpdateUnitTilePosition();
        pathfinding = new Pathfinding(gridManager);
        unitManager = FindFirstObjectByType<UnitManager>();
    }

    void UpdateUnitTilePosition()
    {
        if (tilemap != null)
        {
            unitInstance.currentTile = tilemap.WorldToCell(transform.position);
        }
    }

    public Vector3Int GetCurrentTile() => unitInstance.currentTile;
    public MovementType GetMovementType() => (MovementType)unitInstance.unitData.movementType;
    public int GetActionPoints() => unitInstance.currentActionPoints;

    public IEnumerator FollowPath(Queue<Vector3Int> path, int pathCost)
    {
        var currentTile = new Vector3Int();
        foreach (var tile in path)
        {
            Vector3 worldPos = tilemap.GetCellCenterWorld(tile);
            while (Vector3.Distance(transform.position, worldPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, worldPos, Time.deltaTime * 5f);
                yield return null;
            }
            currentTile = tile;

            unitInstance.NotifyStatsChanged();
        }
        unitInstance.currentTile = currentTile;
        gridManager.unitPositions[currentTile] = unitManager.previouslySelectedUnit;
        unitInstance.currentActionPoints -= pathCost;

        if (unitInstance.currentActionPoints <= 0)
        {
            unitInstance.isActive = false;
        }
    }
    

        
}












        /*Vector3Int startPos = unitInstance.currentTile;

    foreach (var tilePos in path)
    {
        gridHelper.GetMovementCost(tilePos, GetMovementType(), out int cost);

        unitInstance.currentActionPoints -= cost;

        Vector3 worldPos = tilemap.GetCellCenterWorld(tilePos);
        while ((Vector3.Distance(transform.position, worldPos)) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, worldPos, Time.deltaTime * 5f);
            yield return null;
        }

        unitInstance.currentTile = tilePos;
        unitInstance.NotifyStatsChanged();
    }
    unitManager.UpdateUnitPositions(gameObject, startPos, unitInstance.currentTile);

    */


    /*public void MoveTo(Vector3Int targetPosition)
    {
        var path = pathfinding.AStar(unitManager.selectedUnit, targetPosition).ToList();
        if (path != null)
        {
            StartCoroutine(FollowPath(path));


        }
    }*/



