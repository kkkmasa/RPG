using UnityEngine;

public class HeroIdleState : HeroGroundedState
{
    public HeroIdleState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();

        hero.SetVelocityZero();
    }
    public override void Update()
    {
        base.Update();
        if (hero.IsWallDetected() && xInput == hero.facingDir)
            return;

        if (xInput != 0 && !hero.isBusy)
            this.stateMachine.ChangeState(hero.moveState);



    }
    public override void Exit()
    {
        base.Exit();
    }
}
