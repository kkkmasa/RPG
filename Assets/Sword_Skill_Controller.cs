using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    CircleCollider2D cd;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }


    public void Setup(Vector2 _dir, float _gravityScale)
    {
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;
    }


}
