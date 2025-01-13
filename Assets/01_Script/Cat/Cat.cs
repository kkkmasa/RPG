using UnityEngine;

public class Cat : Entry
{
    public CatStateMachine stateMachine { get; private set; }
    public CatIdleState idleState { get; private set; }
    public CatMoveState moveState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        this.stateMachine = new CatStateMachine();
        this.idleState = new CatIdleState(this, this.stateMachine, "Idle");
        this.moveState = new CatMoveState(this, this.stateMachine, "Move");
    }
    protected override void Start()
    {
        base.Start();
        this.stateMachine.Init(this.idleState);
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.I))
            this.stateMachine.ChangeState(moveState);
    }
}
