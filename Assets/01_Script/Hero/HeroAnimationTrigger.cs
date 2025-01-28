using UnityEngine;

public class HeroAnimationTrigger : MonoBehaviour
{
    Hero hero => GetComponentInParent<Hero>();
    public void AnimationFinishTrigger() => hero.AnimationFinishTrigger();
    public void AttackFinishTrigger() {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(hero.attackCheck.position, hero.attackCheckRadius);
        foreach(var hit in collider2Ds) {
            if (hit.GetComponent<Enemy>() != null) {
                hit.GetComponent<Enemy>().DamageImpact();
            }
        }
    }
}
