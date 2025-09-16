using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    public void ActivateSideUnits(List<UnitInstance> unitInstances)
    {
        foreach (var unit in unitInstances)
        {
            unit.isActive = true;
            Debug.Log("Unit " + unit + " activated!");
        }
    }
}
