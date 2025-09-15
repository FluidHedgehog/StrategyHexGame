using UnityEngine;
using System.Collections.Generic;

public enum Site { Player, Enemy1, Enemy2, Enemy3, Enemy4, Enemy5, Enemy6, Enemy7, Enemy8 }
public enum InputFrom { Player, AI }

public class SiteData : MonoBehaviour
{
    public InputFrom inputFrom;

    public FactionData factionData;

    public Site site;

    public List<UnitInstance> currentUnits;


    [Header("References")]
    [SerializeField] UnitManager unitManager;

    private void Start()
    {
        foreach (var unit in unitManager.unitsInGame)
        {

            UnitInstance unitInstance = unit.GetComponent<UnitInstance>();
            if (unitInstance.site == site)
            {
                currentUnits.Add(unitInstance);
                Debug.Log("Added unit: " + unit + " to site " + site);
            }

        }
    }
}
