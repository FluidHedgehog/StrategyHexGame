using UnityEngine;

public enum Type { Melee, Ranged, Support, Magic }
public enum Target { Enemy, Ally, Self, Terrain }
public enum ReturnValue { Damage, Healing }
public enum SpecialEffects { None, Shield, Haste, Wound, Poison, Slow, Stun }

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/Factions/AbilityData")]
public class AbilityData : ScriptableObject
{
    [Header("Identity")]
    public string abilityName;
    public Sprite icon;

    [Header("Classification")]
    public Type type;
    public Target target;
    public ReturnValue returnValue;
    public SpecialEffects specialEffects;

    [Header("Base Values")]
    public int baseDamage;
    public int baseHealing;
    public int baseRange;
    public int baseEffectTime;

    [Header("Limits")]
    public int cooldown;
    public int usesPerTurn;
}
