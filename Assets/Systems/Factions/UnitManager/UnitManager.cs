using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    [SerializeField] TurnManager turnManager;
    [SerializeField] GridManager gridManager;

    public GameObject previouslySelectedUnit;
    public GameObject selectedUnit;

    void Start()
    {
        if (turnManager == null) turnManager = FindFirstObjectByType<TurnManager>();
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();
    }
    public void UpdateUnitUI(GameObject unit)
    {
        var ui = unit.GetComponent<UnitUI>();
        ui.UpdateUI();
    }

    public (GameObject, int) ValidateUnit(GameObject unit)
    {
        var uI = unit.GetComponent<UnitInstance>();

        if (turnManager.currentSide.currentUnits.Contains(uI))
        {
            if (uI.isActive) return (unit, 1);
            else return (unit, 2);
        }
        else if (turnManager.currentSide.hostileSides.Contains(uI.sideData))
        {
            return (unit, 3);
        }
        else if (turnManager.currentSide.neutralSides.Contains(uI.sideData))
        {
            return (unit, 4);
        }
        else if (turnManager.currentSide.friendlySides.Contains(uI.sideData))
        {
            return (unit, 5);
        }
        else return (null, 0);
    }

    public GameObject DetectUnit(Vector3Int checkedPosition)
    {
        var unit = gridManager.unitPositions[checkedPosition];
        return unit;
    }
}
