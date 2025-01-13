using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected Enemy_Skeleton enemy;
    protected Transform player;
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _boolAnimName, Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _boolAnimName)
    {
        this.enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (player != null)
        {
            if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
                this.stateMachine.ChangeState(enemy.battleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
