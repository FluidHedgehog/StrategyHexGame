using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackManager : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Core References
    //------------------------------------------------------------------------------  

    [SerializeField] UnitManager unitManager;
    [SerializeField] TaskManager taskManager;
    [SerializeField] GridManager gridManager;
    [SerializeField] Button[] buttons;

    Pathfinding pathfinding;

    //------------------------------------------------------------------------------
    // Initialization
    //------------------------------------------------------------------------------  

    Dictionary<int, AbilityData> abilities;
    public GameObject attackingUnit;
    public GameObject attackedUnit;
    public AbilityData currentAbility;

    void Start()
    {
        if (unitManager == null) unitManager = FindFirstObjectByType<UnitManager>();
        if (taskManager == null) taskManager = FindFirstObjectByType<TaskManager>();
        if (gridManager == null) gridManager = FindFirstObjectByType<GridManager>();

        pathfinding = new Pathfinding(gridManager, taskManager);
        abilities = new Dictionary<int, AbilityData>();
    }

    //------------------------------------------------------------------------------
    // Initialization of Abilities
    //------------------------------------------------------------------------------  

    public void InitializeAbilities(GameObject unit)
    {
        attackingUnit = unit;
        abilities.Clear();
        var abilityList = unit.GetComponent<UnitInstance>().unitData.abilities;
        for (int i = 0; i < abilityList.Length; i++)
        {
            abilities[i] = abilityList[i];
        }
        SetupAbilityButtons();
    }

    public void InitializeAbility(int abilityIndex)
    {
        currentAbility = abilities[abilityIndex];
        if (ValidateCost())
        {
            DetectRange(currentAbility);
            Debug.Log("Using " + currentAbility + " on " + attackedUnit);
            Debug.Log("Attacked unit: " + attackedUnit);
        }
        else Debug.LogWarning("Not enough Action Points!");
    }

    //------------------------------------------------------------------------------
    // Button Setup
    //------------------------------------------------------------------------------  

    public void SetupAbilityButtons()
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        for (int i = 0; i < abilities.Count; i++)
        {
            buttons[i].gameObject.SetActive(true);

            TMP_Text buttonText = buttons[i].GetComponentInChildren<TMP_Text>();
            buttonText.text = abilities[i].abilityName;
        }

    }

    //------------------------------------------------------------------------------
    // Range Detection
    //------------------------------------------------------------------------------  

    public void DetectRange(AbilityData ability)
    {
        int range = ability.baseRange;
        var start = attackingUnit.GetComponent<UnitInstance>().currentTile;
        var evaluated = pathfinding.Floodfill(start, range);

        taskManager.Attack(evaluated);
    }

    //------------------------------------------------------------------------------
    // Validation
    //------------------------------------------------------------------------------  
    public bool ValidateCost()
    {
        return currentAbility.cost <= attackingUnit.GetComponent<UnitInstance>().currentActionPoints;
    }

    public bool ValidateTarget(int currentCase, Vector3Int clickPos)
    {
        bool isTrue = false;
        switch (currentCase)
        {
            case 0: // You click on empty tile (no unit)
                isTrue = currentAbility.target == Target.Terrain;
                attackedUnit = gridManager.unitPositions[clickPos];
                return isTrue;
            case 1: // You click on your unit, when is active
                isTrue = currentAbility.target == Target.Ally || currentAbility.target == Target.Self;
                attackedUnit = gridManager.unitPositions[clickPos];
                return isTrue;
            case 2: // You click on your unit, when is not active
                isTrue = currentAbility.target == Target.Ally || currentAbility.target == Target.Self;
                attackedUnit = gridManager.unitPositions[clickPos];
                return isTrue;
            case 3: // You click on hostile unit.
                isTrue = currentAbility.target == Target.Enemy;
                attackedUnit = gridManager.unitPositions[clickPos];
                return isTrue;
            case 4: // You click on neutral unit.
                isTrue = currentAbility.target == Target.Enemy || currentAbility.target == Target.Ally;
                attackedUnit = gridManager.unitPositions[clickPos];
                return isTrue;
            case 5: // You click on friendly unit.
                isTrue = currentAbility.target == Target.Ally;
                attackedUnit = gridManager.unitPositions[clickPos];
                return isTrue;
        }
        return isTrue;
    }

    public ReturnValue ValidateReturnValue(ReturnValue returnValue)
    {
        switch (returnValue)
        {
            case ReturnValue.Damage:
                return 0;
            case ReturnValue.Healing:
                return 0;
        }
        return 0;
    }

    public SpecialEffects ValidateSpecialEffects(AbilityData ability)
    {
        return ability.specialEffects;
    }

    public void ValidateAreaOfEffect(AbilityData ability)
    {
        switch (ability.aoE)
        {
            case AoE.Tile:

            case AoE.Circle:

            case AoE.Cone:

            case AoE.Line:
                GetLine();
                return;
        }
    }

    //------------------------------------------------------------------------------
    // Attack Logic
    //------------------------------------------------------------------------------  

    public void Attack()
    {
        //ToDo:
        //  Getting attacker, and defender 
        //  Calculating attacker values from ability data and unit data
        //  Comparing to defender defense/armor and luck
        //  Divide damage from health + assign additional effects
    }

    //------------------------------------------------------------------------------
    // Assign Special Effects
    //------------------------------------------------------------------------------  

    public void AssignEffect(SpecialEffects effect)
    {
        switch (effect)
        {
            case SpecialEffects.Shield:

            case SpecialEffects.Haste:

            case SpecialEffects.Wound:

            case SpecialEffects.Poison:

            case SpecialEffects.Slow:

            case SpecialEffects.Stun:

            case SpecialEffects.None:
                return;
        }
    }

    //------------------------------------------------------------------------------
    // Calculating Area of Effect
    //------------------------------------------------------------------------------  

    public void GetLine()
    {
        //pathfinding.GetHexDistance()
    }

}
