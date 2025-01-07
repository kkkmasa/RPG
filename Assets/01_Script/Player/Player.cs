using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class Player : Entry
{

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12;

    [Header("Attack")]
    public Vector2[] attackMovement;

    

    #region State

    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerWallSliderState wallSliderState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }


    #endregion

    protected override void Awake()
    {
        base.Awake();
        this.stateMachine = new PlayerStateMachine();

        this.idleState = new PlayerIdleState(this, this.stateMachine, "Idle");
        this.moveState = new PlayerMoveState(this, this.stateMachine, "Move");
        this.jumpState = new PlayerJumpState(this, this.stateMachine, "Jump");
        this.airState = new PlayerAirState(this, this.stateMachine, "Jump");
        this.dashState = new PlayerDashState(this, this.stateMachine, "Dash");
        this.wallSliderState = new PlayerWallSliderState(this, this.stateMachine, "WallSlider");
        this.wallJumpState = new PlayerWallJumpState(this, this.stateMachine, "Jump");
        this.primaryAttack = new PlayerPrimaryAttackState(this, this.stateMachine, "Attack");
    }


    protected override void Start()
    {
        base.Start();
        this.stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        this.stateMachine.currentState.Update();
        CheckForDashInput();
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

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
