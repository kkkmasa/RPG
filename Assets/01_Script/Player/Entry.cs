using System.Collections;
using UnityEngine;

public class Entry : MonoBehaviour
{
    #region Components
    public Animator anim;
    public Rigidbody2D rb;
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr;
    public CharacterStats stats { get; private set; }

    public CapsuleCollider2D cd { get; private set; }

    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    [SerializeField] protected bool isKnocked;

    [Header("Dash Info")]

    protected float dashUsageTimer;
    public float dashDuration = 1f;
    public float dashSpeed = 15;
    public float dashDir;
    public float defaultDashSpeed;

    public System.Action onFilped;

    [Header("Collision info")]

    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;



    public int facingDir = 1;
    protected bool facingRight = true;

    public bool isBusy;

    protected virtual void Awake()
    {

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        this.fx = GetComponent<EntityFX>();
        this.anim = GetComponentInChildren<Animator>();
        this.rb = GetComponent<Rigidbody2D>();
        this.sr = GetComponentInChildren<SpriteRenderer>();
        this.stats = GetComponent<CharacterStats>();
        this.cd = GetComponent<CapsuleCollider2D>();
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }
    public virtual void ReturnDefultSpeed()
    {
        anim.speed = 1;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked) return;

        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public virtual void SetVelocityZero()
    {
        if (isKnocked) return;

        SetVelocity(0, 0);
    }
    protected IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public virtual void DamageImpact() =>  StartCoroutine(HitKnockback());

    IEnumerator HitKnockback()
    {
        this.isKnocked = true;
        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(this.knockbackDuration);
        this.isKnocked = false;
    }

    #region Flip

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);

        if (onFilped != null)
        {
            onFilped();
        }
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    public virtual void Die()
    {

    }
}
