using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "Scriptable Objects/Abilities")]
public class Abilities : ScriptableObject
{
    [Header("Ability Info")]
    [SerializeField] string abilityName;
    [SerializeField] string description;
    [SerializeField] AbilityType abilityType;
    [SerializeField] AbilityTarget abilityTarget;

    [Header("Visuals")]
    [SerializeField] Sprite icon;

    [Header("Ability Stats - Melee")]
    [SerializeField] int baseDamage;
    [SerializeField] int baseMeleeRange;
    [SerializeField] int baseCriticalHitChance;
    [SerializeField] int baseArmorPenetration;

    [Header("Ability Stats - Ranged")]
    [SerializeField] int baseRangedDamage;
    [SerializeField] int baseRangedRange;
    [SerializeField] int baseRangedCriticalHitChance;
    [SerializeField] int baseRangedArmorPenetration;

    [Header("Ability Stats - Magic")]
    [SerializeField] int baseMagicDamage;
    [SerializeField] int baseMagicRange;
    [SerializeField] int baseMagicCriticalHitChance;
    [SerializeField] int baseMagicArmorPenetration;

    [Header("Ability Stats - Support")]
    [SerializeField] int baseSupportRange;
    [SerializeField] int baseSupportHealing;
    [SerializeField] int baseSupportShielding;
    [SerializeField] int baseSupportBuffAmount;
    [SerializeField] int baseSupportDebuffAmount;
    [SerializeField] int baseSupportHasteAmount;


    public enum AbilityType { Melee, Ranged, Magic, Support };
    public enum AbilityTarget { Self, Ally, Enemy, Area };
    public enum AbilityEffect { Damage, Heal, Shield, Buff, Debuff, Haste, Stun, Slow, Poison, Burn, Freeze, Knockback, Pull };
}
