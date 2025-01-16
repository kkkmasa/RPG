using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _boolAnimName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _boolAnimName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, this.rb.linearVelocityY);
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected()) {
            enemy.Flip();
            this.stateMachine.ChangeState(enemy.idleState);
        }
        Debug.Log(enemy.IsGroundDetected());
    }

    public override void Exit()
    {
        base.Exit();
    }

}
