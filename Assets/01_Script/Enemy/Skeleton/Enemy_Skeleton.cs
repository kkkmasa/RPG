using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    #region State
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunState stunState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }
    
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        this.idleState = new SkeletonIdleState(this, this.stateMachine, "Idle", this);
        this.moveState = new SkeletonMoveState(this, this.stateMachine, "Move", this);
        this.battleState = new SkeletonBattleState(this, this.stateMachine, "Move", this);
        this.attackState = new SkeletonAttackState(this, this.stateMachine, "Attack", this);
        this.stunState = new SkeletonStunState(this, this.stateMachine, "Stun", this);
        this.deadState = new SkeletonDeadState(this, this.stateMachine, "Idle", this);
    }
    protected override void Start()
    {
        base.Start();
        this.stateMachine.Init(idleState);
    }
    protected override void Update()
    {
        base.Update();

            if (Input.GetKeyDown(KeyCode.S))
            this.stateMachine.ChangeState(stunState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned()) {
            this.stateMachine.ChangeState(this.stunState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        
        if (this.stateMachine.currentState != deadState) 
            this.stateMachine.ChangeState(deadState);
    }

}
