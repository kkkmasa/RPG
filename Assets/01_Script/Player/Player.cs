using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class Player : Entry
{

    [Header("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12;
    public float swordRetrunImpact = 10;
    float defaultMoveSpeed;
    float defaultJumpForce;

    [Header("Attack")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;

    public SkillManager skill;
    public GameObject sword { get; private set; }


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
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    public PlayerDeadState deadState { get; private set; }


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
        this.counterAttackState = new PlayerCounterAttackState(this, this.stateMachine, "CounterAttack");

        this.aimSwordState = new PlayerAimSwordState(this, this.stateMachine, "AimSword");
        this.catchSwordState = new PlayerCatchSwordState(this, this.stateMachine, "CatchSword");
        this.blackholeState = new PlayerBlackholeState(this, this.stateMachine, "Jump");

        this.deadState = new PlayerDeadState(this, this.stateMachine, "Die");

        this.skill = SkillManager.instance;
    }


    protected override void Start()
    {
        base.Start();
        this.stateMachine.Initialize(idleState);

        this.defaultMoveSpeed = moveSpeed;
        this.defaultJumpForce = jumpForce;
        this.defaultDashSpeed = dashSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        this.stateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F))
            skill.crystal.CanUseSkill();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        float percent = (1 - _slowPercentage);
        moveSpeed = moveSpeed * percent;
        jumpForce = jumpForce * percent;
        dashSpeed = dashSpeed * percent;
        anim.speed = anim.speed * percent;

        Invoke("ReturnDefultSpeed", _slowDuration);
    }

    public override void ReturnDefultSpeed()
    {
        base.ReturnDefultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
        
    }

    public void AssignTheSword(GameObject _newSword)
    {
        this.sword = _newSword;
    }
    public void CatchTheSword()
    {
        this.stateMachine.ChangeState(this.catchSwordState);
        Destroy(this.sword);
    }


    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;


        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            this.dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            this.stateMachine.ChangeState(dashState);
        }
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public override void Die()
    {
        base.Die();
        this.stateMachine.ChangeState(deadState);
    }
}
