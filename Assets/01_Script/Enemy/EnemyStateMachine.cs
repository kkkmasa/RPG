using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState;

    public void Init(EnemyState _state) {
        this.currentState = _state;
        this.currentState.Enter();
    }
    public void ChangeState(EnemyState _state) {
        currentState.Exit();
        currentState = _state;
        currentState.Enter();
    }
}
