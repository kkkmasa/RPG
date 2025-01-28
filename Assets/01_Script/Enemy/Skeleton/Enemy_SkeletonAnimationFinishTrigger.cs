using UnityEngine;

public class Enemy_SkeletonAnimationFinishTrigger : MonoBehaviour
{
    Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();


    public void AnimationFinishTrigger() => enemy.AnimationFinishTrigger();

    private void AttackTrigger() {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in collider2Ds) {
            if (hit.GetComponent<Player>() != null) {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(_target);
            }
        }
    }
    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
