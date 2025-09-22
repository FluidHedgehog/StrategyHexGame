using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridManager : MonoBehaviour
{
    [SerializeField] public Tilemap tilemap;

    public Dictionary<TileBase, TileData> dataFromTerrain;
    [SerializeField] List<TileData> tileDatas;

    public Dictionary<Vector3Int, GameObject> unitPositions;
    [SerializeField] public List<GameObject> unitsInGame;

    private void Awake()
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

    public void FindAndAddAllUnitsInScene()
    {
        unitPositions = new Dictionary<Vector3Int, GameObject>();
        unitsInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));

        foreach (var unit in unitsInGame)
        {
            Vector3Int unitTilePos = tilemap.WorldToCell(unit.transform.position);

            unit.transform.position = tilemap.GetCellCenterWorld(unitTilePos);

            unitPositions.Add(unitTilePos, unit);
            //Debug.Log("Unit " + unit + " at " + unitTilePos);
        }
    }

    public void UpdateUnitPositions(GameObject unit, Vector3Int oldPos, Vector3Int newPos)
    {
        if (unitPositions.ContainsKey(oldPos))
        {
            unitPositions.Remove(oldPos);
        }

        unitPositions[newPos] = unit;
    }
}
