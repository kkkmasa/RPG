using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _boolAnimName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _boolAnimName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        enemy.SetVelocityZero();
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            this.stateMachine.ChangeState(enemy.moveState);
    }
    public override void Exit()
    {
        base.Exit();
    }
}
