using UnityEngine;

public class HeroJumpState : HeroState
{
    public HeroJumpState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        hero.SetVelocity(rb.linearVelocityX, hero.jumpForce);
    }
    public override void Update()
    {
        base.Update();

        if (rb.linearVelocityY < 0)
            this.stateMachine.ChangeState(hero.airState);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
