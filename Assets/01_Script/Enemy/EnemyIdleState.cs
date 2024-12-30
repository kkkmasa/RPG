using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _boolAnimName) : base(_enemy, _stateMachine, _boolAnimName)
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
