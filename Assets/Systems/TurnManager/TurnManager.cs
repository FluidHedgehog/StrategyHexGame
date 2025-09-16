using Unity.VisualScripting;
using UnityEngine;

public enum TurnPhase { StartingPhase, ActionPhase, ClosingPhase }

public class TurnManager : MonoBehaviour
{

    [SerializeField] SideManager sideManager;
    [SerializeField] UnitController unitController;
 

    public int turnLimit;
    public int currentTurn;
    public TurnPhase currentPhase;

    public void Start()
    {
        StartPhase();
    }

    void StartPhase()
    {
        currentPhase = TurnPhase.StartingPhase;
        unitController.ActivateSideUnits(sideManager.currentSide.currentUnits);
    }





}
