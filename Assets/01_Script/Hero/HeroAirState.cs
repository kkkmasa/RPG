using UnityEngine;

public class HeroAirState : HeroState
{
    public HeroAirState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        if (hero.IsGroundDetected())
            this.stateMachine.ChangeState(hero.idleState);

        if (xInput != 0)
            hero.SetVelocity(hero.moveSpeed * 0.8f * xInput, rb.linearVelocityY);

        if (hero.IsWallDetected())
            this.stateMachine.ChangeState(hero.wallSliderState);

    }
    public override void Exit()
    {
        base.Exit();
    }

}
