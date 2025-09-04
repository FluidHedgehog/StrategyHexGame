using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfinderInitializer : MonoBehaviour
{
    [SerializeField] public Tilemap map;

    public Dictionary<TileBase, TileData> dataFromTerrain;
    [SerializeField] List<TileData> tileDatas;

    public Dictionary<Vector3Int, GameObject> unitPositions;
    [SerializeField] List<GameObject> unitsInGame;


    void Awake()
    {
        FindAndAddAllTilesInScene();
        FindAndAddAllUnitsInScene();
    }

    private void FindAndAddAllTilesInScene()
    {
        dataFromTerrain = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTerrain.Add(tile, tileData);
            }
        }
    }

    private void FindAndAddAllUnitsInScene()
    {
        unitPositions = new Dictionary<Vector3Int, GameObject>();
        unitsInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));

        foreach (var unit in unitsInGame)
        {
            
            Vector3Int unitTilePos = map.WorldToCell(unit.transform.position);

            unit.transform.position = map.GetCellCenterWorld(unitTilePos);

            unitPositions.Add(unitTilePos, unit);
        }

    }

    public TileData GetTileData(Vector3Int gridPosition)
    {
        var tile = map.GetTile(gridPosition);
        if (tile != null && dataFromTerrain.ContainsKey(tile))
        {
            return dataFromTerrain[tile];
        }
        return null;
    }

    public bool IsWalkable(Vector3Int gridPosition)
    {
        var tileData = GetTileData(gridPosition);
        return tileData != null && tileData.walkable;
    }

    public bool GetMovementCost(Vector3Int gridPosition, Unit.MovementType movementType, out int cost)
    {
        if (PathfinderHelper.DoesTileHaveUnit(this, gridPosition))
        {
            cost = int.MaxValue;
            return false;
        }
        var tileData = GetTileData(gridPosition);
        if (tileData == null)
        {
            cost = int.MaxValue;
            return false;
        }

        switch (movementType)
        {
            case Unit.MovementType.Walk:
                cost = tileData.walkCost;
                return tileData.walkable;
            case Unit.MovementType.Swim:
                cost = tileData.swimCost;
                return tileData.swimable;
            case Unit.MovementType.Fly:
                cost = tileData.flyCost;
                return tileData.flyable;
            default:
                cost = int.MaxValue;
                return false;
        }
    }
  
   /* public void GetTileData(Vector2 position)
           {
               position = Camera.main.ScreenToWorldPoint(position);
               Vector3Int gridPosition = map.WorldToCell(position);
               tile = map.GetTile(gridPosition);
               if (tile != null)
               {
                   Debug.Log(tile + " and " + gridPosition);
               }
               else
               {
                   Debug.Log("No tile found at " + gridPosition);
               }
           } */

}
