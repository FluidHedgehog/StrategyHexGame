using UnityEngine;

public enum MovementType { Walk, Swim, Fly }

[CreateAssetMenu(fileName = "UnitData", menuName = "Scriptable Objects/Factions/UnitData")]
public class UnitData : ScriptableObject
{
    [Header("Identity")]
    public string unitName;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stats")]
    public int maxHealth;
    public int maxExperience;
    public int maxActionPoints;

    public int skill;      // Damage/Healing modifier
    public int agility;    // Dodge chance
    public int armor;      // Damage neglected 
    public int luck;       // Additional functionality

    [Header("Movement")]
    public MovementType movementType; 

    [Header("Abilities")]
    public AbilityData[] abilities;

    [Header("Progression")]
    public UnitData[] advancementOptions; // for level-ups

}
