using UnityEngine;
using System.Collections.Generic;

public enum Side { Player, Enemy1, Enemy2, Enemy3, Enemy4, Enemy5, Enemy6, Enemy7, Enemy8 }
public enum InputFrom { Player, AI }

public class SideData : MonoBehaviour
{
    public InputFrom inputFrom;

    public FactionData factionData;

    public Side side;

    public List<UnitInstance> currentUnits;

    public List<SideData> friendlySides;

    public List<SideData> neutralSides;

    public List<SideData> hostileSides;

    [Header("References")]
    [SerializeField] GridManager unitManager;

    private void Start()
    {
        unitManager = FindFirstObjectByType<GridManager>();
        foreach (var unit in unitManager.unitsInGame)
        {

            UnitInstance unitInstance = unit.GetComponent<UnitInstance>();
            if (unitInstance.side == side)
            {
                currentUnits.Add(unitInstance);
                unitInstance.sideData = this;
                //Debug.Log("Added unit: " + unit + " to site " + side);
            }

        }
    }
}

