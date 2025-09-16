using System;
using System.Collections.Generic;
using UnityEngine;

public class SideManager : MonoBehaviour
{

    [SerializeField] TurnManager turnManager;

    public List<SideData> sideDatas;
    public int currentSideIndex = 0;


    public SideData currentSide => sideDatas[currentSideIndex];

    public void EndTurn()
    {
        if (currentSideIndex == (sideDatas.Count - 1))
        {
            currentSideIndex = 0;
            turnManager.currentTurn++;
        }
        currentSideIndex++;

        Debug.Log("Side changed to " + currentSide);
    }
}
