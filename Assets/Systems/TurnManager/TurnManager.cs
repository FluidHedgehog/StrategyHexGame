
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] UnitController unitController;

    public SideData currentSide => sideDatas[currentSideIndex];
    public List<SideData> sideDatas;
    public int currentSideIndex = 0;
    public int turnLimit;
    public int currentTurn;

    public void Start()
    {
        StartTurn();
        currentTurn = 1;
    }

    void StartTurn()
    {
        unitController.ActivateSideUnits(currentSide.currentUnits);
    }

    public void EndTurn()
    {
        unitController.DeactivateSideUnits(currentSide.currentUnits);
        currentSideIndex++;
        if (currentSideIndex == sideDatas.Count)
        {
            currentSideIndex = 0;
            currentTurn++;
        }
        
        unitController.ActivateSideUnits(currentSide.currentUnits);
        Debug.Log("Side changed to " + currentSide);
    }
}

