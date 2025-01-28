using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] CharacterStats targetStats;
    int damage;
    [SerializeField] float speed;
    Animator anim;
    bool triggered;

    public void SetupThunderStrike(int _damage, CharacterStats _targetStats) {
        this.damage = _damage;
        this.targetStats = _targetStats;
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!targetStats) 
            return;
            
        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;


        if (Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);
            transform.localPosition += new Vector3(0, 1.5f);


            triggered = true;
            anim.SetTrigger("Hit");
            Invoke("DamageAndSelfDestory", .2f);
            
        }
    }
    void DamageAndSelfDestory()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(1);
        Destroy(gameObject, .4f);
    }
}
