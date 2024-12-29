using UnityEngine;

public class HeroWallSliderState : HeroState
{
    public HeroWallSliderState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

    }
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)) {
            this.stateMachine.ChangeState(hero.wallJumpState);
            return;
        }

        if (hero.IsGroundDetected())
            this.stateMachine.ChangeState(hero.idleState);

        if (xInput != 0 && xInput != hero.facingDir)
            this.stateMachine.ChangeState(hero.idleState);

        if (yInput < 0)
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);
        else
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY * 0.7f);

    }
    public override void Exit()
    {
        base.Exit();
    }


}
