using UnityEngine;

public class PathUnitHelper : MonoBehaviour
{

    public static bool DoesTileHaveUnit(UnitManager grid, Vector3Int position)
    {
        return grid.unitPositions.ContainsKey(position);
    }

}
