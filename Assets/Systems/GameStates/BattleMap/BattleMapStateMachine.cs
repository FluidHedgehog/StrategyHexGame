using UnityEngine;

public class BattleMapStateMachine : MonoBehaviour
{
    [SerializeField] TaskManager taskManager;

    private IBattleMapState currentState;

    public BattleIdleState idleState;
    public BattleActionState actionState;

    void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();


        idleState = new BattleIdleState(this, taskManager);
        actionState = new BattleActionState(this, taskManager);

        ChangeState(idleState);
    }

    public void ChangeState(IBattleMapState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void Update()
    {
        if (currentState != null) currentState.Update();
    }


}
