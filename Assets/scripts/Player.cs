using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 1000;
    public int attackPower = 10;
    public MoveScriptableObject[] availableMoves;
    public MoveScriptableObject[] playerMoves;

    private Animator animator;
    public HitEffect hitEffect;
    private SpriteRenderer spriteRenderer;

    private Vector3 originalScale;
    private float sizeChangeDuration = 2f;
    private float buffScaleFactor = 1.2f;
    private float debuffScaleFactor = 0.8f;

    private float healEffectDuration = 0.5f;
    private Color healColor = Color.green;

    private int baseAttackPower = 10;
    private int maxAttackPower = 50;
    private class StatusEffect
    {
        public bool IsBuff;
        public int Power;
        public int TurnsRemaining;
    }
    private List<StatusEffect> activeEffects = new List<StatusEffect>();
    private int effectDuration = 2;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        baseAttackPower = attackPower;
    }

    public void AssignMoves()
    {
        playerMoves = MoveAssigner.AssignUniqueTypeMoves(availableMoves);

        for (int i = 0; i < playerMoves.Length; i++)
        {
            Debug.Log($"[ASSIGNED] Player Move Slot {i}: {playerMoves[i].moveName} ({playerMoves[i].moveType})");
        }
    }

    public void ExecuteMove(MoveScriptableObject move, Opponent opponent)
    {
        StartCoroutine(AttackSequence(move, opponent));
    }

    private IEnumerator AttackSequence(MoveScriptableObject move, Opponent opponent)
    {
        if (move.moveType == MoveType.Damage && animator != null)
            animator.SetTrigger("Attack");

        if (move.moveType == MoveType.Damage && AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX("attack");

        yield return new WaitForSeconds(0.4f);

        switch (move.moveType)
        {
            case MoveType.Damage:
                int totalDamage = move.power + (attackPower - baseAttackPower);
                opponent.TakeDamage(totalDamage);
                break;
            case MoveType.Heal:
                health += move.power;
                StartCoroutine(ApplyHealEffect());
                break;
            case MoveType.Buff:
                ApplyBuff(move.power);
                StartCoroutine(ApplySizeChangeEffect(true));
                break;
            case MoveType.Debuff:
                opponent.ApplyDebuff(move.power);
                opponent.StartCoroutine(opponent.ApplySizeChangeEffect(false));
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        health = Mathf.Max(health, 0);

        if (hitEffect != null)
            hitEffect.PlayHitEffect();
    }

    public void ApplyBuff(int power)
    {
        attackPower += power;
        attackPower = Mathf.Min(attackPower, maxAttackPower);
        activeEffects.Add(new StatusEffect { IsBuff = true, Power = power, TurnsRemaining = effectDuration });
        Debug.Log($"Player applied buff: +{power} attackPower, now {attackPower}");
    }

    public void ApplyDebuff(int power)
    {
        attackPower -= power;
        attackPower = Mathf.Max(attackPower, 0);
        activeEffects.Add(new StatusEffect { IsBuff = false, Power = power, TurnsRemaining = effectDuration });
        Debug.Log($"Player applied debuff: -{power} attackPower, now {attackPower}");
    }

    public void UpdateEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            effect.TurnsRemaining--;

            if (effect.TurnsRemaining <= 0)
            {
                if (effect.IsBuff)
                {
                    attackPower -= effect.Power;
                    Debug.Log($"Player buff expired: -{effect.Power} attackPower, now {attackPower}");
                }
                else
                {
                    attackPower += effect.Power;
                    attackPower = Mathf.Min(attackPower, maxAttackPower);
                    Debug.Log($"Player debuff expired: +{effect.Power} attackPower, now {attackPower}"); // Fixed typo: attackPOWER to attackPower
                }
                activeEffects.RemoveAt(i);
            }
        }
    }

    public IEnumerator ApplySizeChangeEffect(bool isBuff)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(isBuff ? "buff" : "debuff");
        }

        Vector3 targetScale = originalScale * (isBuff ? buffScaleFactor : debuffScaleFactor);
        transform.localScale = targetScale;
        yield return new WaitForSeconds(sizeChangeDuration);
        transform.localScale = originalScale;
    }

    private IEnumerator ApplyHealEffect()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX("heal");
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = healColor;
        }

        yield return new WaitForSeconds(healEffectDuration);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }
}