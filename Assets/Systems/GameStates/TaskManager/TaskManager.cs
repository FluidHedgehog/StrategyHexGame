using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Core References
    //------------------------------------------------------------------------------
    [SerializeField] InputManager inputManager;

    [SerializeField] TurnManager turnManager;

    [SerializeField] UnitManager unitManager;

    [SerializeField] PathController pathController;
    
    [SerializeField] BattleMapStateMachine stateMachine;

    void Start()
    {
        inputManager = FindFirstObjectByType<InputManager>();
        turnManager = FindFirstObjectByType<TurnManager>();
        unitManager = FindFirstObjectByType<UnitManager>();
        pathController = FindFirstObjectByType<PathController>();
        stateMachine = FindFirstObjectByType<BattleMapStateMachine>();
    }
    
    Queue<Vector3Int> path;
    int pathCost;


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

        var validatedUnit = pathController.ValidateUnit(unit);
        if (validatedUnit == null) return;

        var um = validatedUnit.GetComponent<UnitMovement>();
        var reachable = pathController.DetectReachableTiles(unit);
        pathController.HighlightReachableTiles(reachable);

        stateMachine.ChangeState(new BattleActionState(stateMachine, this));
    }

    //------------------------------------------------------------------------------
    // BattleActionState Events
    //------------------------------------------------------------------------------

    public void HandleGridChangeAction(Vector3Int mousePos)
    {
        (path, pathCost) = pathController.DetectPath(mousePos);
        pathController.HighlightPathTiles(path);
    }

    public void HandleInteractAction(Vector3Int clickPos)
    {
        pathController.MoveUnit(path, pathCost);
        //unitManager.UpdateUnitUI(unitManager.previouslySelectedUnit);
        stateMachine.ChangeState(stateMachine.idleState);
    }

}
