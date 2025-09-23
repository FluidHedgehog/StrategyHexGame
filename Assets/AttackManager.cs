using UnityEngine;

public class AttackManager : MonoBehaviour
{

    [SerializeField] UnitManager unitManager;
    [SerializeField] TaskManager taskManager;

    public GameObject attackedUnit;
    void Start()
    {
        if (unitManager == null) unitManager = FindFirstObjectByType<UnitManager>();
        if (taskManager == null) taskManager = FindFirstObjectByType<TaskManager>();
    }

    public void Attack()
    {
        Debug.Log("Attack!");
    }

}
