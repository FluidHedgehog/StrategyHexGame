using UnityEngine;

[CreateAssetMenu(fileName = "FactionData", menuName = "Scriptable Objects/Factions/FactionData")]
public class FactionData : ScriptableObject
{
    [Header("Identity")]
    public string factionName;
    public Color factionColor;
    public Sprite banner;

    [Header("Units")]
    public UnitData[] availableUnits;
}
