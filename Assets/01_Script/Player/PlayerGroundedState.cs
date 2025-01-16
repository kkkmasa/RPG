using Unity.VisualScripting;
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

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            this.stateMachine.ChangeState(player.aimSwordState);

    }
    public override void Exit()
    {
        base.Exit();
    }
    bool HasNoSword() {
        if (!player.sword) {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
