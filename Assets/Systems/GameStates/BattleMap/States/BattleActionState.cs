using UnityEngine;

public class BattleActionState : IBattleMapState
{
    private BattleMapStateMachine stateMachine;
    private TaskManager taskManager;

    public BattleActionState(BattleMapStateMachine machine, TaskManager task)
    {
        stateMachine = machine;
        taskManager = task;
    }

    public void Enter()
    {
        taskManager.OnGridPositionChanged.AddListener(taskManager.HandleGridChangeAction);
        taskManager.Interact.AddListener(taskManager.HandleInteractAction);
        taskManager.Cancel.AddListener(taskManager.HandleCancel);
        taskManager.EndTurn.AddListener(taskManager.HandleEndturn);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            stateMachine.ChangeState(new BattleActionState(stateMachine, taskManager));
            Debug.Log(stateMachine);
        }
    }

    public void Exit()
    {
        taskManager.OnGridPositionChanged.RemoveListener(taskManager.HandleGridChangeAction);
        taskManager.Interact.RemoveListener(taskManager.HandleInteractAction);
        taskManager.Cancel.RemoveListener(taskManager.HandleCancel);
        taskManager.EndTurn.RemoveListener(taskManager.HandleEndturn);
    }


}
