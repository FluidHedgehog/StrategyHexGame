/*using UnityEngine;

public class BattleActionStateManager : MonoBehaviour
{
    [SerializeField] PathController pathController;
    [SerializeField] PathVFX pathVFX;
    [SerializeField] InputManager inputManager;

    public InputManager.GridPositionEvent OnGridPositionChanged => inputManager.OnGridPositionChanged;
    public InputManager.GridPositionEvent Interact => inputManager.Interact;

    public void HandleGridChange(Vector3Int newPos)
    {
        
    }

    public void HandleInteract(Vector3Int clickPos)
    {
        var unit = pathController.DetectUnit(clickPos);
        if (unit == null) return;

        var validatedUnit = pathController.ValidateUnit(unit);
        if (validatedUnit == null) return;

        var um = validatedUnit.GetComponent<UnitMovement>();
        var reachable = pathController.DetectReachableTiles(unit);
        pathController.HighlightReachableTiles(reachable);
    }



}
*/