using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public int health = 1000;
    public int attackPower = 10;
    public MoveScriptableObject[] availableMoves;
    public MoveScriptableObject[] opponentMoves;

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
        opponentMoves = MoveAssigner.AssignUniqueTypeMoves(availableMoves);

        for (int i = 0; i < opponentMoves.Length; i++)
        {
            Debug.Log($"[ASSIGNED] Opponent Move Slot {i}: {opponentMoves[i].moveName} ({opponentMoves[i].moveType})");
        }
    }

    public MoveScriptableObject SelectRandomMove()
    {
        return opponentMoves[Random.Range(0, opponentMoves.Length)];
    }

    public void ExecuteMove(MoveScriptableObject move, Player player)
    {
        StartCoroutine(AttackSequence(move, player));
    }

    private IEnumerator AttackSequence(MoveScriptableObject move, Player player)
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
                player.TakeDamage(totalDamage);
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
                player.ApplyDebuff(move.power);
                player.StartCoroutine(player.ApplySizeChangeEffect(false));
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
        Debug.Log($"Opponent applied buff: +{power} attackPower, now {attackPower}");
    }

    public void ApplyDebuff(int power)
    {
        attackPower -= power;
        attackPower = Mathf.Max(attackPower, 0);
        activeEffects.Add(new StatusEffect { IsBuff = false, Power = power, TurnsRemaining = effectDuration });
        Debug.Log($"Opponent applied debuff: -{power} attackPower, now {attackPower}");
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
                    Debug.Log($"Opponent buff expired: -{effect.Power} attackPower, now {attackPower}");
                }
                else
                {
                    attackPower += effect.Power;
                    attackPower = Mathf.Min(attackPower, maxAttackPower);
                    Debug.Log($"Opponent debuff expired: +{effect.Power} attackPower, now {attackPower}"); // Fixed typo: attackPOWER to attackPower
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