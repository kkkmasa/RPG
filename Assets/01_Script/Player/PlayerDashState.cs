using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = player.dashDuration;

    }
    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected()) {
            this.stateMachine.ChangeState(player.wallSliderState);
        }

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (stateTimer < 0)
            this.stateMachine.ChangeState(player.idleState);

    }
    public override void Exit()
    {
        base.Exit();
        player.SetVelocity(0, rb.linearVelocityY);
    }
}
