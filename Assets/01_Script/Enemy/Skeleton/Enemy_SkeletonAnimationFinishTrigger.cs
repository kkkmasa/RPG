using UnityEngine;

public class Enemy_SkeletonAnimationFinishTrigger : MonoBehaviour
{
    Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();


    public void AnimationFinishTrigger() => enemy.AnimationFinishTrigger();

    private void AttackTrigger() {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach(var hit in collider2Ds) {
            if (hit.GetComponent<Player>() != null) {
                hit.GetComponent<Player>().Damage();
            }
        }
    }
    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
