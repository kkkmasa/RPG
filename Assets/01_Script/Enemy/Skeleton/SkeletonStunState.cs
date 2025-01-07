using UnityEngine;

public class SkeletonStunState : EnemyState
{
    Enemy_Skeleton enemy;
    public SkeletonStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _boolAnimName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _boolAnimName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        this.stateTimer = enemy.stunDuration;

        rb.linearVelocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

    }
    
    public override void Update()
    {
        base.Update();

        if (this.stateTimer < 0)
            this.stateMachine.ChangeState(enemy.idleState);
    }
    
    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelRedBlink", 0);
    }
}
