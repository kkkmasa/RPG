using UnityEngine;

public class HeroStateMachine
{
    public HeroState currentState;

    public void Init(HeroState _state) {
        currentState = _state;
        currentState.Enter();
    }
    public void ChangeState(HeroState _state) {
        currentState.Exit();
        currentState = _state;
        currentState.Enter();
    }

}
