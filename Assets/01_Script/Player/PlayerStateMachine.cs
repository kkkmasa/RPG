using UnityEngine;

public class PlayerStateMachine
{
    public PlayerState currentState;

    public void Initialize(PlayerState _state) {
        this.currentState = _state;
        this.currentState.Enter();
    }
    public void ChangeState(PlayerState _newState) {
        this.currentState.Exit();
        this.currentState = _newState;
        this.currentState.Enter();
        this.currentState.Log();

    }
}
