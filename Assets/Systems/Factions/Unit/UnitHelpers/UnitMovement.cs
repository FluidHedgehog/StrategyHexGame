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
    [SerializeField] private TaskManager taskManager;
    [SerializeField] float speedFactor;
    private Pathfinding pathfinding;

    void Start()
    {
        if (taskManager == null) taskManager = FindFirstObjectByType<TaskManager>();
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
        if (unitManager == null) unitManager = FindFirstObjectByType<UnitManager>();
        if (tilemap == null) tilemap = FindFirstObjectByType<Tilemap>();

        unitInstance = GetComponent<UnitInstance>();
        UpdateUnitTilePosition();
        pathfinding = new Pathfinding(gridManager, taskManager);
        speedFactor = 5f;
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
        //Debug.Log($"FollowPath started. Path count: {path.Count}, pathCost: {pathCost}");
        
        if (path.Count == 0)
        {
            //Debug.LogWarning("Path is empty - unit cannot move!");
            yield break;
        }

        var currentTile = new Vector3Int();
        foreach (var tile in path)
        {
            //Debug.Log($"Moving to tile: {tile}");
            Vector3 worldPos = tilemap.GetCellCenterWorld(tile);
            while (Vector3.Distance(transform.position, worldPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, worldPos, Time.deltaTime * speedFactor);
                yield return null;
            }
            currentTile = tile;
            //Debug.Log($"Reached tile: {tile}");

            unitInstance.NotifyStatsChanged();
        }
        
        //Debug.Log($"Movement complete. Final tile: {currentTile}");
        unitInstance.currentTile = currentTile;
        
        if (gridManager != null && unitManager?.previouslySelectedUnit != null)
        {
            gridManager.unitPositions[currentTile] = unitManager.previouslySelectedUnit;
            //Debug.Log($"Updated grid position for unit at: {currentTile}");
        }
        else
        {
            //Debug.LogError("GridManager or UnitManager.previouslySelectedUnit is null!");
        }
        
        unitInstance.currentActionPoints -= pathCost;

        if (unitInstance.currentActionPoints <= 0)
        {
            unitInstance.isActive = false;
            //Debug.Log("Unit is now inactive - no action points left");
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



