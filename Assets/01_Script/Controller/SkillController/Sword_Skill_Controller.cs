using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    float returnSpeed = 12;
    Animator anim;
    Rigidbody2D rb;
    CircleCollider2D cd;
    Player player;

    bool canRotation = true;
    bool isReturning;

    float freezeTimeDuration;

    
    int pierceAmount;

    [Header("Bounc Info")]
    float boundSpeed = 20;
    bool isBouncing;
    int amountOfBounce;
    List<Transform> enemyTargets = new List<Transform>();
    private int targetIndex;

    [Header("Spin info")]
    float maxTravelDistance;
    float spinDuration;
    float spinTimer;
    bool wasStopped;
    bool isSpinning;

    float hitTimer;
    float hitCooldown;

    float spinDirection;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    void DestroyMe() {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        this.player = _player;
        this.returnSpeed = _returnSpeed;
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;
        this.freezeTimeDuration = _freezeTimeDuration;

        spinDirection = Mathf.Clamp(rb.linearVelocity.x, -1, 1);

        Invoke("DestroyMe", 7f)
;
        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBound, float _bounceSpeed)
    {
        this.isBouncing = _isBouncing;
        this.amountOfBounce = _amountOfBound;
        this.boundSpeed = _bounceSpeed;
    }
    public void SetupPires(int _amountOfPierce)
    {
        this.pierceAmount = _amountOfPierce;
    }

    public void SetupSpin(bool _isSpining, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        this.isSpinning = _isSpining;
        this.maxTravelDistance = _maxTravelDistance;
        this.spinDuration = _spinDuration;
        this.hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        // rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    void Update()
    {
        if (canRotation)
            transform.right = rb.linearVelocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, this.returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                this.player.CatchTheSword();
            }
        }
        BounceLogic();

        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopSpin();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer <= 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in collider2Ds)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            SwordSkillDamage(hit.GetComponent<Enemy>()); 
                        }
                    }
                }
            }
        }
    }

    private void StopSpin()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].transform.position, boundSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                Enemy enemy = enemyTargets[targetIndex].GetComponent<Enemy>();
                if (enemy != null) {
                    SwordSkillDamage(enemy);
                }
                
                targetIndex++;
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex >= enemyTargets.Count)
                    targetIndex = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
            return;

        if (other.GetComponent<Enemy>() != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        SetupForBounceLogic(other);

        StuckInfo(other);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.StartCoroutine("FreezeTimerFor", this.freezeTimeDuration);
    }

    private void SetupForBounceLogic(Collider2D other)
    {

        if (other.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in collider2Ds)
                {
                    if (hit.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(hit.transform);
                    }
                }
            }
        }
    }

    private void StuckInfo(Collider2D other)
    {

        if (pierceAmount > 0 && other.GetComponent<Enemy>() != null)
        {
            this.pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopSpin();
            return;
        }

        canRotation = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTargets.Count > 0)
            return;

        transform.parent = other.transform;

        anim.SetBool("Rotation", false);
    }
}
