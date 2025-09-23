using UnityEngine;

public class BattleIdleState : IBattleMapState
{
    private BattleMapStateMachine stateMachine;
    private TaskManager taskManager;

    public BattleIdleState(BattleMapStateMachine machine, TaskManager task)
    {
        stateMachine = machine;
        taskManager = task;
    }

    public void Enter()
    {
       // Debug.Log("EnteredIdleState");
        taskManager.OnGridPositionChanged.AddListener(taskManager.HandleGridChangeIdle);
        taskManager.Interact.AddListener(taskManager.HandleInteractIdle);
        taskManager.Cancel.AddListener(taskManager.HandleCancel);
        taskManager.EndTurn.AddListener(taskManager.HandleEndturn);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            stateMachine.ChangeState(stateMachine.actionState);
        }
    }

    public void Exit()
    {
        //Debug.Log("ExitIdleState");
        taskManager.OnGridPositionChanged.RemoveListener(taskManager.HandleGridChangeIdle);
        taskManager.Interact.RemoveListener(taskManager.HandleInteractIdle);
        taskManager.Cancel.RemoveListener(taskManager.HandleCancel);
        taskManager.EndTurn.RemoveListener(taskManager.HandleEndturn);
    }

    

}
