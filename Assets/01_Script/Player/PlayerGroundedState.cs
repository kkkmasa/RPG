using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
            this.stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            this.stateMachine.ChangeState(player.jumpState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            this.stateMachine.ChangeState(player.primaryAttack);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
