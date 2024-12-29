using UnityEngine;

public class HeroGroundedState : HeroState
{
    public HeroGroundedState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space) && hero.IsGroundDetected()) {
            this.stateMachine.ChangeState(hero.jumpState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            this.stateMachine.ChangeState(hero.primaryAttackState);
        }

    }
    public override void Exit()
    {
        base.Exit();
    }


}
