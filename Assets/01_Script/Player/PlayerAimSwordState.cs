using UnityEngine;

public class PlayerAimSwordState : PlayerState
{
    public PlayerAimSwordState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName) : base(_player, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        player.skill.sword.DotsActive(true);
    }
    
    public override void Update()
    {
        base.Update();

        player.SetVelocityZero();

        if (Input.GetKeyUp(KeyCode.Mouse1))
            this.stateMachine.ChangeState(player.idleState);
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
