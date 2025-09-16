using UnityEngine;

public class BattleMapStateMachine : MonoBehaviour
{
    [SerializeField] IBattleMapState currentState;
    [SerializeField] TaskManager taskManager;

    void Start()
    {
        ChangeState(new BattleIdleState(this, taskManager));
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
