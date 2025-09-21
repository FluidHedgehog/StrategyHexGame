using Unity.VisualScripting;
using UnityEngine;

public class PathUnitHelper : MonoBehaviour
{
    [SerializeField] UnitManager unitManager;

    public bool DoesTileHaveUnit(Vector3Int position)
    {
        return unitManager.unitPositions.ContainsKey(position);
    }
}
