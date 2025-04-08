using UnityEngine;

public class TestBattleSandbox : MonoBehaviour
{
    public Animator monsterAnimator;
    public HitEffect hitEffect;

    public void PlayAttack()
    {
        monsterAnimator.SetTrigger("Attack");
    }

    public void PlayHitEffect()
    {
        hitEffect.PlayHitEffect();
    }
}
