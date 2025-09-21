using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    public GameObject previouslySelectedUnit;
    public GameObject selectedUnit;

    [SerializeField] public Tilemap map;

    public Dictionary<Vector3Int, GameObject> unitPositions;
    [SerializeField] public List<GameObject> unitsInGame;

    void Awake()
    {
        FindAndAddAllUnitsInScene();
    }

    public void FindAndAddAllUnitsInScene()
    {
        unitPositions = new Dictionary<Vector3Int, GameObject>();
        unitsInGame = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));

        foreach (var unit in unitsInGame)
        {
            Vector3Int unitTilePos = map.WorldToCell(unit.transform.position);

            unit.transform.position = map.GetCellCenterWorld(unitTilePos);

            unitPositions.Add(unitTilePos, unit);
            Debug.Log("Unit " + unit + " at " + unitTilePos);
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

    public void UpdateUnitUI(GameObject unit)
    {
        var ui = unit.GetComponent<UnitUI>();
        ui.UpdateUI();
    }
}
