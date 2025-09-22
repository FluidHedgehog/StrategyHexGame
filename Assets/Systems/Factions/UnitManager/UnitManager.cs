using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitManager : MonoBehaviour
{
    public GameObject previouslySelectedUnit;
    public GameObject selectedUnit;
    
        public void UpdateUnitUI(GameObject unit)
    {
        var ui = unit.GetComponent<UnitUI>();
        ui.UpdateUI();
    }
}
