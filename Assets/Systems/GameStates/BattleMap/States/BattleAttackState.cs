using UnityEngine;

public class BattleAttackState : IBattleMapState
{
    private BattleMapStateMachine stateMachine;
    private TaskManager taskManager;

    public BattleAttackState(BattleMapStateMachine machine, TaskManager task)
    {
        stateMachine = machine;
        taskManager = task;
    }

    public void Enter()
    {
       // Debug.Log("EnteredIdleState");
        taskManager.OnGridPositionChanged.AddListener(taskManager.HandleGridChangeAttack);
        taskManager.Interact.AddListener(taskManager.HandleInteractAttack);
        taskManager.Cancel.AddListener(taskManager.HandleCancel);
        //taskManager.EndTurn.AddListener(taskManager.HandleEndturn);
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
        taskManager.OnGridPositionChanged.RemoveListener(taskManager.HandleGridChangeAttack);
        taskManager.Interact.RemoveListener(taskManager.HandleInteractAttack);
        taskManager.Cancel.RemoveListener(taskManager.HandleCancel);
        //taskManager.EndTurn.RemoveListener(taskManager.HandleEndturn);
    }

    

}
