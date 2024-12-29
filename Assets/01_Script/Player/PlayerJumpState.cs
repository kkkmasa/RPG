using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce);

    }
    public override void Update()
    {
        base.Update();
        if (rb.linearVelocityY < 0)
            this.stateMachine.ChangeState(player.airState);            
    }
    public override void Exit()
    {
        base.Exit();
    }
}
