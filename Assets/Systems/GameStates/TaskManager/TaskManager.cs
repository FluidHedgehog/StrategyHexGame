using System.Collections.Generic;
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
            case 1:
                // Unit is yours and active.
                SelectUnit(validatedUnitIdle);
                return;
            case 2:
                // Unit is yours and not active.
                Debug.Log(validatedUnitIdle + " not active!");
                return;
            case 3:
                // Unit is hostile.
                Debug.Log(validatedUnitIdle + " is hostile!");
                return;
            case 4:
                // Unit is neutral.
                Debug.Log(validatedUnitIdle + " is neutral!");
                return;
            case 5:
                // Unit is friendly.
                Debug.Log(validatedUnitIdle + " is friendly!");
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
            case 0:
                // No unit detected.
                (path, pathCost) = pathController.DetectPath(mousePos);
                pathController.HighlightPathTiles(path);
                ActionStateCurrentCase = 0;

                return;

            case 1:
                // Unit detected
                int currentCaseAction;
                (validatedUnit, currentCaseAction) = unitManager.ValidateUnit(unitManager.DetectUnit(mousePos));
                switch (currentCaseAction)
                {
                    case 0:
                        ActionStateCurrentCase = int.MaxValue;
                        return;
                    case 1:
                        // Unit is yours and active.
                        Debug.Log(validatedUnit + " is yours!");
                        ActionStateCurrentCase = 1;
                        return;
                    case 2:
                        // Unit is yours and not active.
                        Debug.Log(validatedUnit + " not active!");
                        ActionStateCurrentCase = 2;
                        return;
                    case 3:
                        // Unit is hostile.
                        Debug.Log(validatedUnit + " is hostile!");
                        ActionStateCurrentCase = 3;
                        return;
                    case 4:
                        // Unit is neutral.
                        Debug.Log(validatedUnit + " is neutral!");
                        ActionStateCurrentCase = 4;
                        return;
                    case 5:
                        // Unit is friendly.
                        Debug.Log(validatedUnit + " is friendly!");
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
            case 0:
                MoveUnit();
                return;
            case 1:
                SelectUnit(validatedUnit);
                return;
            case 2:
                // Popup - no active!
                return;
            case 3:
                MoveUnit();
                attackManager.Attack();
                return;
            case 4:
                MoveUnit();
                attackManager.Attack();
                return;
            case 5:
                //Popup - is friendly
                return;
        }
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

}
