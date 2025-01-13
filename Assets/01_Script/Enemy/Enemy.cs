using UnityEngine;

public class Enemy : Entry
{
    [SerializeField] protected LayerMask whatIsPlayer;



    [Header("Move Info")]
    public float moveSpeed = 2;
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
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        this.stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        this.stateMachine.currentState.Update();

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
