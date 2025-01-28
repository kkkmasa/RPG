using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    Player player;
    Animator anim => GetComponent<Animator>();
    CircleCollider2D circleCollider2D => GetComponent<CircleCollider2D>();
    float crystalExitTimer;

    bool canExplode;
    bool canMoveToEnemy;
    float moveSpeed;

    bool canGrow;
    [SerializeField] float growSpeed;
    [SerializeField] LayerMask whatIsEnemy;

    Transform closestTarget;
    public void SetupCrystal(float _duration, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        this.crystalExitTimer = _duration;
        this.canExplode = _canExplode;
        this.canMoveToEnemy = _canMoveToEnemy;
        this.moveSpeed = _moveSpeed;
        this.closestTarget = _closestTarget;
        this.player = _player;

    }
    void Update()
    {
        crystalExitTimer -= Time.deltaTime;
        if (crystalExitTimer <= 0)
        {
            FinishCrystal();
        }

        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

        if (canMoveToEnemy)
        {
            if (closestTarget == null)
                return;
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                canMoveToEnemy = false;
                FinishCrystal();
            }
        }
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackhole.GetBlackholeRadius();
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if (collider2Ds.Length > 0)
        {
            this.closestTarget = collider2Ds[Random.Range(0, collider2Ds.Length)].transform;
        }

    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
        {
            SelfDestory();
        }
    }

    void SelfDestory() => Destroy(gameObject);

    public void AnimationExplodeEvent()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                // hit.GetComponent<Enemy>().DamageEffect();
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
            }
        }
    }
}
