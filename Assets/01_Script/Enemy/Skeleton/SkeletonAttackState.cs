using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _boolAnimName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _boolAnimName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();

        enemy.SetVelocityZero();

        if (triggerCalled)
            this.stateMachine.ChangeState(enemy.battleState);
    }
    
    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time; 
    }
}
