using System;
using System.Collections.Generic;
using UnityEngine;

public class SideManager : MonoBehaviour
{

    public List<SideData> sideDatas;
    private int currentSideIndex = 0;


    public SideData currentSide => sideDatas[currentSideIndex];

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("The " + currentSide + " is " + currentSideIndex);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {

            if (currentSideIndex == (sideDatas.Count - 1))
            {

                currentSideIndex = 0;
                Debug.Log("Side changed to " + currentSide);
                return;
            }
            currentSideIndex++;
            Debug.Log("Side changed to " + currentSide);

        }
    }

}
