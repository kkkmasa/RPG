using UnityEngine;

public class CatStateMachine : MonoBehaviour
{
    public CatState currentState;

    public void Init(CatState _state) {
        currentState = _state;
        currentState.Enter();
    }
    public void ChangeState(CatState _state) {
        currentState.Exit();
        currentState = _state;
        currentState.Enter();
    }
}
