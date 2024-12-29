using UnityEngine;

public class HeroPrimaryAttackState : HeroState
{
    int combCounter;
    float latestTime;
    float combWindow = 2;

    public HeroPrimaryAttackState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName) : base(_hero, _stateMachine, _boolAnimName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        
        if (combCounter > 2 || Time.time > latestTime + combWindow) 
            combCounter = 0;

        hero.anim.SetInteger("CombCounter", combCounter);
        this.stateTimer = .1f;
        hero.SetVelocity(hero.moveAttack[combCounter].x*hero.facingDir, hero.moveAttack[combCounter].y);
    }
    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            this.stateMachine.ChangeState(hero.idleState);

        if (this.stateTimer < 0)
            hero.SetVelocityZero();
    }
    public override void Exit()
    {
        base.Exit();
        combCounter++;
        latestTime = Time.time;
        hero.StartCoroutine("BusyFor", 0.15f);
    }
}
