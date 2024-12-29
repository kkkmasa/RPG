using UnityEngine;

public class HeroState
{
    protected Hero hero;
    protected HeroStateMachine stateMachine;
    string boolAnimName;
    protected float stateTimer;

    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;
    protected bool triggerCalled;

    public HeroState(Hero _hero, HeroStateMachine _stateMachine, string _boolAnimName)
    {
        this.hero = _hero;
        this.stateMachine = _stateMachine;
        this.boolAnimName = _boolAnimName;
    }
    public virtual void Enter()
    {
        hero.anim.SetBool(boolAnimName, true);
        rb = hero.rb;
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        
        hero.anim.SetFloat("yVelocity", rb.linearVelocityY);
        
    }
    public virtual void Exit()
    {
        hero.anim.SetBool(boolAnimName, false);
    }
    public void AnimationFinishTrigger() {
        triggerCalled = true;
    }
}
