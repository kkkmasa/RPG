using System.Collections;
using UnityEngine;

public class Enemy : Entry
{
    [SerializeField] protected LayerMask whatIsPlayer;



    [Header("Move Info")]
    public float moveSpeed = 2;
    float defaultMoveSpeed;
    public float idleTime;

    [Header("Stun Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;
    public float battleTime;
    public string lastAnimBoolName { get; private set; }
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        this.stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();

    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefultSpeed", _slowDuration);
    }
    public override void ReturnDefultSpeed()
    {
        base.ReturnDefultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        this.stateMachine.currentState.Update();

    }

    public virtual void AssignLastAnimName(string _animBoolName) {
        lastAnimBoolName = _animBoolName;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen) {
            moveSpeed = 0;
            anim.speed = 0;
        } else {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }
    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            wallCheck.position,
            wallCheck.position + new Vector3(facingDir * 50, 0f, 0f)
        );


    }

    #region Counter Attack
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }


    public void AnimationFinishTrigger() => this.stateMachine.currentState.AnimationFinishTrigger();
}
