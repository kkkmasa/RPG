using UnityEngine;

public class HeroMoveState : HeroGroundedState
{
    public HeroMoveState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        hero.SetVelocity(xInput * hero.moveSpeed, rb.linearVelocityY);
        
        if (xInput == 0 || hero.IsWallDetected())
            this.stateMachine.ChangeState(this.hero.idleState);

        if (!hero.IsGroundDetected())
            this.stateMachine.ChangeState(hero.airState);

    }
    public override void Exit()
    {
        base.Exit();
    }
}
