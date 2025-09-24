using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Core References
    //------------------------------------------------------------------------------
    [SerializeField] InputManager inputManager;
    [SerializeField] TurnManager turnManager;
    [SerializeField] UnitManager unitManager;
    [SerializeField] GridManager gridManager;
    [SerializeField] AttackManager attackManager;

    [SerializeField] PathController pathController;
    [SerializeField] PathVFX pathVFX;

    [SerializeField] BattleMapStateMachine stateMachine;

    void Start()
    {
        inputManager = FindFirstObjectByType<InputManager>();
        turnManager = FindFirstObjectByType<TurnManager>();
        unitManager = FindFirstObjectByType<UnitManager>();
        gridManager = FindFirstObjectByType<GridManager>();
        attackManager = FindFirstObjectByType<AttackManager>();
        pathController = FindFirstObjectByType<PathController>();
        stateMachine = FindFirstObjectByType<BattleMapStateMachine>();
    }

    Queue<Vector3Int> path;
    GameObject validatedUnit;

    int pathCost;

    int ActionStateCurrentCase;
    int AttackStateCurrentCase;

    //------------------------------------------------------------------------------
    // Events Initialization
    //------------------------------------------------------------------------------

    public InputManager.ButtonEvent Cancel => inputManager.Cancel;
    public InputManager.ButtonEvent EndTurn => inputManager.EndTurn;

    public InputManager.GridPositionEvent OnGridPositionChanged => inputManager.OnGridPositionChanged;
    public InputManager.GridPositionEvent Interact => inputManager.Interact;

    //------------------------------------------------------------------------------
    // Versatile Events
    //------------------------------------------------------------------------------ 

    public void HandleCancel()
    {
        unitManager.selectedUnit = null;
        pathController.ClearTiles();
        stateMachine.ChangeState(stateMachine.idleState);
    }

    public void HandleEndturn()
    {
        turnManager.EndTurn();
    }

    //------------------------------------------------------------------------------
    // BattleIdleState Events
    //------------------------------------------------------------------------------    

    public void HandleGridChangeIdle(Vector3Int mousePos)
    {

    }

    public void HandleInteractIdle(Vector3Int clickPos)
    {
        var unit = pathController.DetectUnit(clickPos);
        if (unit == null) return;
        var (validatedUnitIdle, currentCase) = unitManager.ValidateUnit(unit);
        if (validatedUnitIdle == null) return;

        switch (currentCase)
        {
            case 0:
                return;
            case 1: // You click on your unit, when it is active.
                
                SelectUnit(validatedUnitIdle);
                attackManager.InitializeAbilities(validatedUnitIdle);
                return;
            case 2: // You click on your unit, when it is not active.
                
                return;
            case 3: // You click on hostile unit.
                
                return;
            case 4: // You click on neutral unit.
                
                return;
            case 5: // You click on friendly unit.
                
                return;
        }
    }

    //------------------------------------------------------------------------------
    // BattleActionState Events
    //------------------------------------------------------------------------------

    public void HandleGridChangeAction(Vector3Int mousePos)
    {
        int currentCase = gridManager.ValidateTile(mousePos);
        switch (currentCase)
        {
            case 0: //You hover over empty tile (no unit)
                (path, pathCost) = pathController.DetectPath(mousePos);
                pathController.HighlightPathTiles(path);
                ActionStateCurrentCase = 0;
                pathVFX.ClearUnit();

                return;
            case 1://You hover over tile with unit
                
                int currentCaseAction;
                (validatedUnit, currentCaseAction) = unitManager.ValidateUnit(unitManager.DetectUnit(mousePos));
                var unitInstance = validatedUnit.GetComponent<UnitInstance>();
                switch (currentCaseAction)
                {
                    case 0:
                        ActionStateCurrentCase = int.MaxValue;
                        return;
                    case 1: // Unit is yours and active.
                        //pathVFX.HighlightOwn(unitInstance.currentTile);

                        ActionStateCurrentCase = 1;
                        return;
                    case 2: // Unit is yours and not active.
                        //pathVFX.HighlightOwn(unitInstance.currentTile);

                        ActionStateCurrentCase = 2;
                        return;
                    case 3: // Unit is hostile.
                        //pathVFX.HighlightEnemy(unitInstance.currentTile);

                        ActionStateCurrentCase = 3;
                        return;
                    case 4: // Unit is neutral.
                        //pathVFX.HighlightEnemy(unitInstance.currentTile);

                        ActionStateCurrentCase = 4;
                        return;
                    case 5: // Unit is friendly.
                        //pathVFX.HighlightFriend(unitInstance.currentTile);

                        ActionStateCurrentCase = 5;
                        return;
                }
                ActionStateCurrentCase = int.MaxValue;
                return;
        }
        ActionStateCurrentCase = int.MaxValue;
        return;
    }

    public void HandleInteractAction(Vector3Int clickPos)
    {
        switch (ActionStateCurrentCase)
        {
            case 0: // You click on empty tile (no unit)
                MoveUnit();
                return;
            case 1: // You click on your unit, when is active
                SelectUnit(validatedUnit);
                return;
            case 2: // You click on your unit, when is not active

                return;
            case 3: // You click on hostile unit.
                MoveUnit();
                return;
            case 4: // You click on neutral unit.
                MoveUnit();
                return;
            case 5: // You click on friendly unit.
                return;
        }
    }

    //------------------------------------------------------------------------------
    // BattleAttackState Events
    //------------------------------------------------------------------------------

    public void HandleGridChangeAttack(Vector3Int mousePos)
    {
        int currentCase = gridManager.ValidateTile(mousePos);
        var unitInstance = validatedUnit.GetComponent<UnitInstance>();

        switch (currentCase)
        {
            case 0: //You hover over empty tile (no unit)
                pathVFX.HighlightEnemy(unitInstance.currentTile);
                return;
            case 1://You hover over tile with unit

                int currentCaseAction;
                (validatedUnit, currentCaseAction) = unitManager.ValidateUnit(unitManager.DetectUnit(mousePos));
                switch (currentCaseAction)
                {
                    case 0:
                        AttackStateCurrentCase = int.MaxValue;
                        return;
                    case 1: // Unit is yours and active.
                        pathVFX.HighlightOwn(unitInstance.currentTile);

                        AttackStateCurrentCase = 1;
                        return;
                    case 2: // Unit is yours and not active.
                        pathVFX.HighlightOwn(unitInstance.currentTile);

                        AttackStateCurrentCase = 2;
                        return;
                    case 3: // Unit is hostile.
                        pathVFX.HighlightEnemy(unitInstance.currentTile);

                        AttackStateCurrentCase = 3;
                        return;
                    case 4: // Unit is neutral.
                        pathVFX.HighlightEnemy(unitInstance.currentTile);

                        AttackStateCurrentCase = 4;
                        return;
                    case 5: // Unit is friendly.
                        pathVFX.HighlightFriend(unitInstance.currentTile);

                        AttackStateCurrentCase = 5;
                        return;
                }
                AttackStateCurrentCase = int.MaxValue;
                return;
        }
        AttackStateCurrentCase = int.MaxValue;
        return;
    }

    public void HandleInteractAttack(Vector3Int clickPos)
    {
        var isCorrect = attackManager.ValidateTarget(AttackStateCurrentCase, clickPos);

        if (isCorrect)
        {
            attackManager.Attack();
        }
        else Debug.LogWarning("Wrong target!");
    }

    //------------------------------------------------------------------------------
    // Helper Actions
    //------------------------------------------------------------------------------

    //------------------------------------------------------------------------------
    // Selection
    //------------------------------------------------------------------------------

    private void SelectUnit(GameObject unit)
    {
        unitManager.selectedUnit = null;
        pathController.ClearTiles();
        unitManager.selectedUnit = unit;
        pathController.HighlightReachableTiles(pathController.DetectReachableTiles(unit));

        stateMachine.ChangeState(new BattleActionState(stateMachine, this));
    }

    //------------------------------------------------------------------------------
    // Movement
    //------------------------------------------------------------------------------

    private void MoveUnit()
    {
        pathController.MoveUnit(path, pathCost);
        unitManager.UpdateUnitUI(unitManager.previouslySelectedUnit);
        stateMachine.ChangeState(stateMachine.idleState);
    }

    //------------------------------------------------------------------------------
    // Hover
    //------------------------------------------------------------------------------

    //------------------------------------------------------------------------------
    // Attack
    //------------------------------------------------------------------------------

    public void Attack(List<Vector3Int> range)
    {
        pathVFX.HighlightAttackRangeTiles(range);
    }
}
