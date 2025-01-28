using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    Player player;
    SpriteRenderer sr;
    Animator anim;
    float colorLossingSpeed;
    float cloneTimer;
    [SerializeField] Transform attackCheck;
    [SerializeField] float attackCheckRadius = .8f;
    Transform closestEnemy;
    bool canDuplicate;
    int facingDir = 1;
    float chanceToDuplicate;

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
    public void Setup(Transform _newTransform, float cloneDuration, float _colorLossingSpeed, bool _canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicate, float _chanceToDuplicate, Player _player)
    {

        if (_canAttack)
            this.anim.SetInteger("AttackNumber", Random.Range(1, 3));

        this.transform.position = _newTransform.position + _offset;
        cloneTimer = cloneDuration;
        this.colorLossingSpeed = _colorLossingSpeed;
        this.closestEnemy = _closestEnemy;
        this.canDuplicate = _canDuplicate;
        this.chanceToDuplicate = _chanceToDuplicate;
        this.player = _player;

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

                // hit.GetComponent<Enemy>().DamageEffect();
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                if (canDuplicate)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    void FacingClosestTarget()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
