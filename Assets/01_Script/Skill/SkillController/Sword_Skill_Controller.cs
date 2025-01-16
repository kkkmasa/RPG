using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] float returnSpeed = 12;
    Animator anim;
    Rigidbody2D rb;
    CircleCollider2D cd;
    Player player;

    bool canRotation = true;
    bool isReturning;

    [Header("Pirers info")]
    [SerializeField] int pierceAmount;

    [Header("Bounc Info")]
    [SerializeField] float boundSpeed = 20;
    bool isBouncing;
    int amountOfBounce;
    List<Transform> enemyTargets = new List<Transform>();
    private int targetIndex;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }


    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        this.player = _player;
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;


        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);
    }

    public void SetupBounce(bool _isBouncing, int _amountOfBound)
    {
        this.isBouncing = _isBouncing;
        this.amountOfBounce = _amountOfBound;
    }
    public void SetupPires(int _amountOfPierce)
    {
        this.pierceAmount = _amountOfPierce;
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
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].transform.position, boundSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
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

        other.GetComponent<Enemy>()?.Damage();

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

        StuckInfo(other);
    }

    private void StuckInfo(Collider2D other)
    {

        if (pierceAmount > 0 && other.GetComponent<Enemy>() != null)
        {
            this.pierceAmount--;
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
