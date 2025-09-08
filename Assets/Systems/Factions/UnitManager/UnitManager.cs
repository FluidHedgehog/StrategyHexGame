using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{

    [SerializeField] public Tilemap map;

    public Dictionary<Vector3Int, GameObject> unitPositions;
    [SerializeField] List<GameObject> unitsInGame;

    void Awake()
    {
        FindAndAddAllUnitsInScene();
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
    
}
