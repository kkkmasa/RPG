using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    SpriteRenderer sr;
    Animator anim;
    float colorLossingSpeed;
    float cloneTimer;
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius = .8f;
    Transform closestEnemy;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLossingSpeed));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }
    public void Setup(Transform _newTransform, float cloneDuration, float _colorLossingSpeed, bool _canAttack)
    {

        if (_canAttack)
            this.anim.SetInteger("AttackNumber", Random.Range(1, 3));

        this.transform.position = _newTransform.position;
        cloneTimer = cloneDuration;
        this.colorLossingSpeed = _colorLossingSpeed;

        FacingClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }
    private void AttackTrigger()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().Damage();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    void FacingClosestTarget()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
