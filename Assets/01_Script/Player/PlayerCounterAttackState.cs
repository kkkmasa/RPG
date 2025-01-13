using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessCounterAttack", false);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocityZero();

        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned()) {
                    stateTimer = 10;
                    player.anim.SetBool("SuccessCounterAttack", true);
                }
            }
        }
        if (this.stateTimer < 0 || triggerCalled)
            this.stateMachine.ChangeState(player.idleState);


    }

    public override void Exit()
    {
        base.Exit();
    }
}
