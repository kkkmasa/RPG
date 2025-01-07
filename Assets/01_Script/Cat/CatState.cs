using UnityEngine;

public class CatState
{
    protected CatStateMachine stateMachine;
    protected Cat cat;
    protected string boolAnimName;
    protected float stateTime;
    protected bool triggerCalled;

    public CatState(Cat _cat,CatStateMachine _stateMachine, string _boolAnimName) {
        this.cat = _cat;
        this.stateMachine = _stateMachine;
        this.boolAnimName = _boolAnimName;
    }
    public virtual void Enter()
    {
        cat.anim.SetBool(boolAnimName, true);
    }
    
    public virtual void Update()
    {
        this.stateTime -= Time.deltaTime;
    }
    
    public virtual void Exit()
    {
        cat.anim.SetBool(boolAnimName, false);
    }
}
