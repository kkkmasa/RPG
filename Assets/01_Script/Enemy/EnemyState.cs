using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected string boolAnimName;
    protected bool triggerCalled;
    protected float stateTimer;
    protected Rigidbody2D rb;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _boolAnimName) {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.boolAnimName = _boolAnimName;
    }

    public virtual void Enter() {
        triggerCalled = false;
        enemyBase.anim.SetBool(boolAnimName, true);
        this.rb = enemyBase.rb;
        
    }
    public virtual void Update() {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit() {
        enemyBase.anim.SetBool(boolAnimName, false);

    }

    public virtual void AnimationFinishTrigger() {
        this.triggerCalled = true;
    }

}
