using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;
    protected string boolAnimName;
    protected bool triggerCalled;

    protected float stateTimer;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _boolAnimName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.boolAnimName = _boolAnimName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(boolAnimName, true);
        rb = player.rb;
        triggerCalled = false;
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.linearVelocityY);
    }
    public virtual void Exit()
    {
        player.anim.SetBool(boolAnimName, false);

    }
    public virtual void AnimationFinishTrigger() {
        this.triggerCalled = true;
    }
    public void Log() {
        // Debug.Log(this.boolAnimName);
    }

}
