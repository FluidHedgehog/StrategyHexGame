using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public void ActivateSideUnits(List<UnitInstance> unitInstances)
    {
        ResetUnits(unitInstances);
        foreach (var unit in unitInstances)
        {
            unit.isActive = true;
            Debug.Log("Unit " + unit + " activated!");
        }
    }

    public void ResetUnits(List<UnitInstance> unitInstances)
    {
        foreach (var unit in unitInstances)
        {
            unit.currentActionPoints = unit.unitData.maxActionPoints;
        }
    }



    public void DeactivateSideUnits(List<UnitInstance> unitInstances)
    {
        foreach (var unit in unitInstances)
        {
            unit.isActive = false;
            Debug.Log("Unit " + unit + " deactivated!");
        }
    }
}
