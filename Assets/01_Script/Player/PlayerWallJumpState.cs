using UnityEngine;


    public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .4f;
        player.SetVelocity(-5*player.facingDir, player.jumpForce);
    }
    public override void Update()
    {
        base.Update();
        if (this.stateTimer < 0)
            this.stateMachine.ChangeState(player.airState);

        if (this.player.IsGroundDetected())
            this.stateMachine.ChangeState(player.idleState);

    }
    public override void Exit()
    {
        base.Exit();
    }
}

