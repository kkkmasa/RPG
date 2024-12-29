using UnityEngine;

public class HeroWallJumpState : HeroState
{
    public HeroWallJumpState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        hero.SetVelocity(-5 * hero.facingDir, hero.jumpForce);
        this.stateTimer = .4f;
    }
    public override void Update()
    {
        base.Update();

        if (this.stateTimer < 0)
            this.stateMachine.ChangeState(hero.airState);

        if (hero.IsGroundDetected())
            this.stateMachine.ChangeState(hero.idleState);

   

    }
    public override void Exit()
    {
        base.Exit();
    }


}
