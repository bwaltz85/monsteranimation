using System.Collections;
using UnityEngine;

public class BattleMonster : MonoBehaviour
{
    [Header("Stats")]
    public int health = 1000;
    public int attackPower = 10;

    [Header("Animation & Effects")]
    public Animator animator;
    public HitEffect hitEffect;

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);
    }

    public void ApplyDebuff(int power)
    {
        attackPower -= power;
        attackPower = Mathf.Max(attackPower, 0);
    }

    public IEnumerator AttackSequence(string triggerName = "Attack")
    {
        if (animator != null)
        {
            animator.SetTrigger(triggerName);
            yield return new WaitForSeconds(0.5f); // Animation delay
        }
    }

    public IEnumerator HitSequence()
    {
        if (hitEffect != null)
        {
            hitEffect.PlayHitEffect();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
