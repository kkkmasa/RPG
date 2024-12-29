using UnityEngine;

public class HeroDashState : HeroState
{
    public HeroDashState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        

        stateTimer = hero.dashDuration;
    }
    public override void Update()
    {
        base.Update();

        hero.SetVelocity(hero.dashSpeed * hero.dashDir, 0);

        if (this.stateTimer < 0)
            this.stateMachine.ChangeState(hero.idleState);

        if (hero.IsWallDetected() && !hero.IsGroundDetected())
            this.stateMachine.ChangeState(hero.wallSliderState);

    }
    public override void Exit()
    {
        base.Exit();
        hero.SetVelocity(0, rb.linearVelocityY);
    }


}
