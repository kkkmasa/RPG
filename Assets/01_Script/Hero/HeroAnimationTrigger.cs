using UnityEngine;

public class HeroAnimationTrigger : MonoBehaviour
{
    Hero hero => GetComponentInParent<Hero>();
    public void AnimationFinishTrigger() => hero.AnimationFinishTrigger();
}
