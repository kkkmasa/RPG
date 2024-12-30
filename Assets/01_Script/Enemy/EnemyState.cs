using UnityEngine;

public class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    protected string boolAnimName;
    protected bool triggerCalled;
    protected float stateTimer;

    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _boolAnimName) {
        this.enemy = _enemy;
        this.stateMachine = _stateMachine;
        this.boolAnimName = _boolAnimName;
    }

    public virtual void Enter() {
        triggerCalled = false;
        enemy.anim.SetBool(boolAnimName, true);
        
    }
    public virtual void Update() {
        stateTimer -= Time.deltaTime;
    }
    public virtual void Exit() {
        enemy.anim.SetBool(boolAnimName, false);

    }

}
