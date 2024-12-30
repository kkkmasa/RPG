using UnityEngine;

public class Enemy : Entry
{
    
    public EnemyStateMachine stateMachine {  get; private set; }
    public EnemyIdleState idleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        this.stateMachine = new EnemyStateMachine();

        this.idleState = new EnemyIdleState(this, this.stateMachine, "Idle");
    }

    protected override void Start()
    {
        base.Start();
        this.stateMachine.Init(this.idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        this.stateMachine.currentState.Update();
    }
}
