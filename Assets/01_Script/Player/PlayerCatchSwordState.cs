using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    Transform sword;
    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        sword = player.sword.transform;

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Flip();

        rb.linearVelocity = new Vector2(player.swordRetrunImpact * -player.facingDir, rb.linearVelocityY);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            this.stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
    }
}
