using UnityEngine;

public class CatIdleState : CatState
{
    public CatIdleState(Cat _cat, CatStateMachine _stateMachine, string _boolAnimName) : base(_cat, _stateMachine, _boolAnimName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
    
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
