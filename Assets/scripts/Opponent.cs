using System.Collections;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    public int health = 1000;
    public int attackPower = 10;
    public MoveScriptableObject[] availableMoves;
    public MoveScriptableObject[] opponentMoves;

    private Animator animator;
    public HitEffect hitEffect;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
                player.TakeDamage(move.power);
                break;
            case MoveType.Heal:
                health += move.power;
                break;
            case MoveType.Buff:
                attackPower += move.power;
                break;
            case MoveType.Debuff:
                player.attackPower -= move.power;
                player.attackPower = Mathf.Max(player.attackPower, 0);
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

    public void ApplyDebuff(int power)
    {
        attackPower -= power;
        attackPower = Mathf.Max(attackPower, 0);
    }
}
