using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] PathController pathController;

    public void Controller(Vector3Int pos)
    {
        if (pathController.selectedUnit == null)
        {
            pathController.DetectUnit(pos);
        }
        else
        {
            pathController.MoveUnit(pos);
        }
    }
}
