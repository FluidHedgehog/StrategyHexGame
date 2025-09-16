using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // Core References
    //------------------------------------------------------------------------------
    [SerializeField] InputManager inputManager;

    [SerializeField] PathController pathController;
    
    [SerializeField] BattleMapStateMachine stateMachine;

    [SerializeField] TurnManager turnManager;

    List<Vector3Int> validatedPath;

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
        pathController.selectedUnit = null;
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
        var reachable = pathController.DetectReachableTiles(um);
        pathController.HighlightReachableTiles(reachable);

        stateMachine.ChangeState(new BattleActionState(stateMachine, this));
    }

    //------------------------------------------------------------------------------
    // BattleActionState Events
    //------------------------------------------------------------------------------

    public void HandleGridChangeAction(Vector3Int mousePos)
    {
        var path = pathController.DetectPath(mousePos);
        validatedPath = pathController.ValidatePath(path, mousePos);
        pathController.HighlightPathTiles(validatedPath);
    }

    public void HandleInteractAction(Vector3Int clickPos)
    {
        pathController.MoveUnit(clickPos, validatedPath);
        stateMachine.ChangeState(new BattleIdleState(stateMachine, this));
    }

}
