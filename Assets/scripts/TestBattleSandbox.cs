using UnityEngine;

public class TestBattleSandbox : MonoBehaviour
{
    [Header("References")]
    public Animator monsterAnimator;   // Drag your monster's Animator here
    public HitEffect hitEffect;        // Drag your monster's HitEffect script here

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayAttack();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayHitEffect();
        }
    }

    public void PlayAttack()
    {
        if (monsterAnimator != null)
        {
            monsterAnimator.SetTrigger("Attack");
            Debug.Log("Attack animation triggered.");
        }
        else
        {
            Debug.LogWarning("Monster Animator not assigned!");
        }
    }

    public void PlayHitEffect()
    {
        if (hitEffect != null)
        {
            hitEffect.PlayHitEffect();
            Debug.Log("Hit effect triggered.");
        }
        else
        {
            Debug.LogWarning("HitEffect not assigned!");
        }
    }
}
