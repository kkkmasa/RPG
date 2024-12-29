using System.Collections;
using System.Data;
using UnityEditor.Rendering;
using UnityEngine;

public class Hero : Entry
{

    [Header("Attack Info")]
    public Vector2[] moveAttack;

    [HideInInspector]
    public bool isBusy;
    HeroStateMachine stateMachine;

    public HeroIdleState idleState { get; private set; }
    public HeroMoveState moveState { get; private set; }
    public HeroJumpState jumpState { get; private set; }
    public HeroAirState airState { get; private set; }
    public HeroDashState dashState { get; private set; }
    public HeroWallSliderState wallSliderState { get; private set; }
    public HeroWallJumpState wallJumpState { get; private set; }
    public HeroPrimaryAttackState primaryAttackState { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        stateMachine = new HeroStateMachine();

        idleState = new HeroIdleState(this, this.stateMachine, "Idle");
        moveState = new HeroMoveState(this, this.stateMachine, "Move");
        jumpState = new HeroJumpState(this, this.stateMachine, "Jump");
        airState = new HeroAirState(this, this.stateMachine, "Jump");
        dashState = new HeroDashState(this, this.stateMachine, "Dash");
        wallSliderState = new HeroWallSliderState(this, this.stateMachine, "WallSlider");
        wallJumpState = new HeroWallJumpState(this, this.stateMachine, "Jump");
        primaryAttackState = new HeroPrimaryAttackState(this, this.stateMachine, "Attack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }

    protected override void Update()
    {
        base.Update();
        this.stateMachine.currentState.Update();

        CheckForDashInput();
    }

    IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            this.dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            this.stateMachine.ChangeState(dashState);
        }
    }

    public void AnimationFinishTrigger() => this.stateMachine.currentState.AnimationFinishTrigger();

    public void SetVelocityZero()
    {
        SetVelocity(0, 0);
    }

}
